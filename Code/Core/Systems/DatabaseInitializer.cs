using Mongo.Rest;

namespace Orizon.Core.Systems;

/// <summary>
/// Defines how the database connection should be initialized.
/// </summary>
/// <remarks>
/// The database connection is initialized based on the value of this enum.
/// </remarks>
public enum DatabaseConnectionKind
{
	/// <summary>
	/// Connect to a local instance of the database.
	/// </summary>
	/// <remarks>
	/// This is the default connection kind.
	/// </remarks>
	Localhost,

	/// <summary>
	/// Connect to a remote instance of the database, using the <see cref="DatabaseInitializer.ConnectionUrl"/> property.
	/// </summary>
	/// <remarks>
	/// The <see cref="DatabaseInitializer.ConnectionUrl"/> property must be set if this connection kind is used.
	/// </remarks>
	Remote
}

[Description( "Initializes the database connection" )]
public sealed class DatabaseInitializer : Component
{
	public const string DefaultApiUrl = "https://localhost:443";
	
	/// <summary>
	/// Defines how the database connection should be initialized.
	/// <list type="bullet">
	/// <item><see cref="DatabaseConnectionKind.Localhost"/>: Connect to a local instance of the database.</item>
	/// <item><see cref="DatabaseConnectionKind.Remote"/>: Connect to a remote instance of the database, using the <see cref="ConnectionUrl"/> property.</item>
	/// </list>
	/// </summary>
	[Property] public DatabaseConnectionKind ConnectionKind { get; private set; }

	/// <summary>
	/// The URL of the remote database connection.
	/// Only applicable if <see cref="ConnectionKind"/> is set to <see cref="DatabaseConnectionKind.Remote"/>.
	/// </summary>
	[Property, ShowIf( nameof(ConnectionKind), DatabaseConnectionKind.Remote )]
	public string ConnectionUrl { get; private set; } = DefaultApiUrl;

	protected override void OnAwake()
	{
		var url = ConnectionKind switch
		{
			DatabaseConnectionKind.Localhost => DefaultApiUrl,
			DatabaseConnectionKind.Remote => ConnectionUrl,
			_ => string.Empty
		};

		var system = Scene.GetSystem<MongoRestSystem>();

		system.Configure( x =>
		{
			x.Url = url;
		} );
	}
}
