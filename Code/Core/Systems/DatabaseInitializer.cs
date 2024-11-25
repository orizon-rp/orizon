using Mongo.Rest;

namespace Orizon.Core.Systems;

public enum DatabaseConnectionKind
{
	Localhost,
	Remote
}

[Description( "Initializes the database connection" )]
public sealed class DatabaseInitializer : Component
{
	[Property] public DatabaseConnectionKind ConnectionKind { get; private set; }

	[Property, ShowIf( nameof(ConnectionKind), DatabaseConnectionKind.Remote )]
	public string ConnectionUrl { get; private set; } = "https://localhost:443";

	protected override void OnAwake()
	{
		var url = ConnectionKind switch
		{
			DatabaseConnectionKind.Localhost => "https://localhost:443",
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
