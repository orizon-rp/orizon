using System;
using System.Text.Json.Serialization;
using Mongo.Rest;

namespace Orizon.Core.Models;

[MongoCollection( "characters" )]
public record CharacterData
{
	[JsonPropertyName( "_id" )] public CharacterId Id { get; set; }

	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public DateTime DateOfBirth { get; set; }
	public Gender Gender { get; set; }
	public VitalsData Vitals { get; set; } = null!;
	public Vector3 Position { get; set; }
	public float Heading { get; set; }
	public DateTime CreatedAt { get; set; }

	public record VitalsData
	{
		public float Health { get; set; } = 100f;
		public float Stamina { get; set; } = 100f;
		public float Hunger { get; set; } = 100f;
		public float Thirst { get; set; } = 100f;
	}
}

public sealed class CharacterRepository : MongoRepository<CharacterData>
{
}
