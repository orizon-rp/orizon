namespace Orizon.Core.Systems;

public sealed class GameManager : Component, Component.INetworkListener
{
	void INetworkListener.OnActive( Connection channel )
	{
		// TODO - Create a PlayerData for the new player and insert it into the database
		// (Only if the player is not already in the database)
	}
}
