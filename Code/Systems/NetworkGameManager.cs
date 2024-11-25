using System;
using Orizon.Core;
using Orizon.Core.Network;
using Sandbox.Diagnostics;
using Sandbox.Network;

namespace Orizon.Systems;

public sealed class NetworkGameManager : Component, Component.INetworkListener
{
	[Property] public GameObject PlayerPrefab { get; private init; } = null!;

	[Property, Feature( "Lobby" )] public string LobbyName { get; private set; } = "Orizon";
	[Property, Feature( "Lobby" )] public int MaxPlayers { get; private set; } = 64;
	[Property, Feature( "Lobby" )] public bool AutoSwitchToBestHost { get; private set; } = false;
	[Property, Feature( "Lobby" )] public bool DestroyWhenHostLeaves { get; private set; } = true;

	private LobbyConfig LobbyConfig => new()
	{
		Name = LobbyName,
		MaxPlayers = MaxPlayers,
		AutoSwitchToBestHost = AutoSwitchToBestHost,
		DestroyWhenHostLeaves = DestroyWhenHostLeaves
	};

	protected override void OnAwake()
	{
		Assert.NotNull( PlayerPrefab, $"{nameof(PlayerPrefab)} is not set" );
	}

	protected override void OnStart()
	{
		if ( Networking.IsActive ) return;
		Networking.CreateLobby( LobbyConfig );
	}

	void INetworkListener.OnActive( Connection channel )
	{
		Log.Info( $"Player {channel.SteamId} has joined the game" );

		var player = GetOrCreatePlayer( channel );

		if ( !player.IsValid() )
		{
			throw new Exception(
				$"Something went wrong when trying to create {nameof(PlayerPrefab)} for {channel.DisplayName}" );
		}

		OnPlayerJoined( player, channel );

		// TODO - Create a PlayerData for the new player and insert it into the database
		// (Only if the player is not already in the database)
	}

	private void OnPlayerJoined( Player player, Connection connection )
	{
		Scene.RunEvent<INetworkEvents>( x => x.OnPlayerConnected( player ) );

		if ( !player.Network.Active ) player.GameObject.NetworkSpawn( connection );
		else player.Network.AssignOwnership( connection );

		player.HostInit();
		player.ClientInit();

		Scene.RunEvent<INetworkEvents>( x => x.OnPlayerJoined( player ) );
	}

	private Player? GetOrCreatePlayer( Connection connection )
	{
		var players = Scene.GetAllComponents<Player>();
		var possiblePlayer = players.FirstOrDefault( x => x.Connection is null && x.SteamId == connection.SteamId );

		if ( possiblePlayer.IsValid() )
		{
			Log.Warning( $"Found existing player for {connection.DisplayName} that we can reuse ({possiblePlayer})" );
			return possiblePlayer;
		}

		if ( !PlayerPrefab.IsValid() )
		{
			Log.Warning( $"Could not spawn player as no {nameof(PlayerPrefab)} assigned." );
			return null;
		}

		var playerGo = PlayerPrefab.Clone();
		playerGo.BreakFromPrefab();
		playerGo.Name = $"Player ({connection.DisplayName})";
		playerGo.Network.SetOrphanedMode( NetworkOrphaned.ClearOwner );

		var player = playerGo.GetComponent<Player>();
		return !player.IsValid() ? null : player;
	}
}
