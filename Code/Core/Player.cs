using System;
using System.Threading.Tasks;
using Mongo.Rest;
using Orizon.Core.Models;
using Orizon.Core.Network;
using Sandbox.Diagnostics;

namespace Orizon.Core;

public sealed class Player : Component
{
	[RequireComponent] public Character Character { get; private set; } = null!;

	/// <summary>
	/// The SteamID of the player.
	/// </summary>
	/// <remarks>
	/// This is synced on the host.
	/// </remarks>
	[HostSync, ReadOnly, Property] public SteamId SteamId { get; private set; }

	/// <summary>
	/// The Steam name of the player.
	/// </summary>
	/// <remarks>
	/// This is synced on the host.
	/// </remarks>
	[HostSync, ReadOnly, Property] public string SteamName { get; private set; } = null!;

	/// <summary>
	/// The player data.
	/// </summary>
	/// <remarks>
	/// This is synced on the host.
	/// </remarks>
	[HostSync] public PlayerData PlayerData { get; private set; } = null!;

	/// <summary>
	/// The connection of the player.
	/// </summary>
	/// <remarks>
	/// This is the connection of the player on the server.
	/// </remarks>
	public Connection? Connection => Network.Owner;

	/// <summary>
	/// Whether the player is connected to the server.
	/// </summary>
	/// <remarks>
	/// This is true if the player is connected to the server, either as the host or a client.
	/// </remarks>
	public bool IsConnected => Connection is not null && (Connection.IsActive || Connection.IsHost);

	/// <summary>
	/// Whether the player is the local player.
	/// </summary>
	/// <remarks>
	/// This is true if the player is running on the local machine.
	/// </remarks>
	public bool IsLocalPlayer => !IsProxy && Connection == Connection.Local;

	public static Player Local { get; private set; } = null!;

	internal async Task HostInit()
	{
		if ( Connection is null )
		{
			Log.Error( "Failed to get player connection" );
			return;
		}

		SteamId = Connection.SteamId;
		SteamName = Connection.DisplayName;

		await LoadPlayer( Connection );
	}

	[Authority]
	internal void ClientInit()
	{
		Local = this;
	}

	/// <summary>
	/// Kicks the player from the server. If the player is not connected, nothing happens.
	/// </summary>
	/// <param name="reason">The reason for the kick. Defaults to <see cref="DisconnectedReason.None"/>.</param>
	public void Kick( DisconnectedReason reason = DisconnectedReason.None )
	{
		Scene.RunEvent<INetworkEvents>( x => x.OnPlayerDisconnected( this, reason ) );
		Connection?.Kick( reason.ToMessage() );
	}

	/// <summary>
	/// Loads a player from the database or creates a new one.
	/// </summary>
	/// <param name="channel">The connection of the player to load.</param>
	/// <returns>A task that completes when the player is loaded.</returns>
	private async Task LoadPlayer( Connection channel )
	{
		var players = Scene.GetRepository<PlayerRepository>()!;
		var playerData = (await players.GetAsync( x => x.Owner = channel.SteamId )).FirstOrDefault();

		if ( playerData is null )
		{
			var newPlayer = new PlayerData
			{
				Id = Guid.NewGuid().ToString(),
				Owner = channel.SteamId,
				Character = null,
				Characters = new List<CharacterId>(),
				LastLogin = DateTime.UtcNow,
				CreatedAt = DateTime.UtcNow
			};

			PlayerData = playerData!;

			Log.Info( $"Creating new player data for {channel.DisplayName}" );

			var created = await players.InsertAsync( newPlayer );
			Assert.True( created, $"Failed to create player data for {channel.DisplayName}" );

			Log.Info( "Created new player data for " + channel.DisplayName );
		}
		else
		{
			playerData.LastLogin = DateTime.UtcNow;
			PlayerData = playerData;

			await players.UpdateAsync( x => x.Id = playerData.Id, x => x.LastLogin = playerData.LastLogin );
			Log.Info("Updated player data for " + channel.DisplayName);
		}
	}
}
