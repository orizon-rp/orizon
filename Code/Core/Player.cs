namespace Orizon.Core;

public sealed class Player : Component
{
	[HostSync, Property] public SteamId SteamId { get; set; }
	[HostSync, Property] public string SteamName { get; set; } = null!;

	public static Player Local { get; private set; } = null!;
	public Connection? Connection => Network.Owner;
	public bool IsConnected => Connection is not null && (Connection.IsActive || Connection.IsHost);
	public bool IsLocalPlayer => !IsProxy && Connection == Connection.Local;

	internal void HostInit()
	{
		if ( Connection is null )
		{
			Log.Error( "Failed to get player connection" );
			return;
		}

		SteamId = Connection.SteamId;
		SteamName = Connection.DisplayName;
	}

	[Authority]
	internal void ClientInit()
	{
		Local = this;
	}
}
