using System;
using System.Text.Json.Serialization;
using Mongo.Rest;

namespace Orizon.Core.Models;

[MongoCollection( "characters" )]
public record CharacterData
{
	[JsonPropertyName( "_id" )] public CharacterId Id { get; init; } = null!;

	public string FirstName { get; init; } = null!;
	public string LastName { get; init; } = null!;
	public DateTime DateOfBirth { get; init; }
	public Gender Gender { get; init; }
	public VitalsData Vitals { get; init; } = null!;
	public DateTime CreatedAt { get; init; }

	public record VitalsData
	{
		public float Health { get; set; }
		public float Stamina { get; set; }
		public float Hunger { get; set; }
		public float Thirst { get; set; }
	}
}
