namespace Orizon.Core.Network;

public interface INetworkEvents : ISceneEvent<INetworkEvents>
{
	void OnPlayerConnected( Player player );
	void OnPlayerJoined( Player player );
	void OnPlayerDisconnected( Player player, DisconnectedReason reason );
}
