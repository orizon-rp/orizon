using System.Text.Json.Serialization;
using Mongo.Rest;
using Sandbox;

[MongoCollection( "users" )]
public record User
{
	[JsonPropertyName( "_id" )] public string Id { get; set; } = null!;
	public string Name { get; set; } = null!;
	public int Age { get; set; }
}

public sealed class UserRepository : MongoRepository<User>
{
	public UserRepository( Scene scene ) : base( scene ) { }
}
