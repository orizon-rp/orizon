using System;
using System.Text.Json.Serialization;
using Mongo.Rest;

namespace Orizon.Core.Models;

[MongoCollection( "players" )]
public record PlayerData
{
	[JsonPropertyName("_id")] public string Id { get; set; } = null!;
	
	public SteamId Owner { get; set; }
	public CharacterId? Character { get; set; }
	public List<CharacterId> Characters { get; set; } = null!;
	public DateTime LastLogin { get; set; }
	public DateTime CreatedAt { get; set; }
}

public sealed class PlayerRepository : MongoRepository<PlayerData>
{
}
