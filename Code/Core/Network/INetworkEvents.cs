namespace Orizon.Core.Network;

public interface INetworkEvents : ISceneEvent<INetworkEvents>
{
	void OnPlayerConnected( Player player );
	void OnPlayerJoined( Player player );
}
