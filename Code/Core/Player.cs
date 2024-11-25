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

	[HostSync, Property] public SteamId SteamId { get; private set; }
	[HostSync, Property] public string SteamName { get; set; } = null!;
	[HostSync] public PlayerData PlayerData { get; private set; } = null!;

	public Connection? Connection => Network.Owner;
	public bool IsConnected => Connection is not null && (Connection.IsActive || Connection.IsHost);
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

		await LoadPlayer( this, Connection );
	}

	[Authority]
	internal void ClientInit()
	{
		Local = this;
	}

	public void Kick( DisconnectedReason reason = DisconnectedReason.None )
	{
		Scene.RunEvent<INetworkEvents>( x => x.OnPlayerDisconnected( this, reason ) );
		Connection?.Kick( reason.ToMessage() );
	}

	private async Task LoadPlayer( Player player, Connection channel )
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

			player.PlayerData = playerData!;

			Log.Info( $"Creating new player data for {channel.DisplayName}" );

			var created = await players.InsertAsync( newPlayer );
			Assert.True( created, $"Failed to create player data for {channel.DisplayName}" );

			Log.Info( "Created new player data for " + channel.DisplayName );
		}
		else
		{
			playerData.LastLogin = DateTime.UtcNow;
			player.PlayerData = playerData;

			await players.UpdateAsync( x => x.Id = playerData.Id, x => x.LastLogin = playerData.LastLogin );
			Log.Info("Updated player data for " + channel.DisplayName);
		}
	}
}
