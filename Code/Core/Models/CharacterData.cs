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
	public DateTime CreatedAt { get; init; }
}
