using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mongo.Rest;

public readonly struct MongoFilter
{
	public Dictionary<string, object> Filter { get; init; }

	[JsonConstructor]
	internal MongoFilter( object filter )
	{
		Filter = JsonSerializer.Deserialize<Dictionary<string, object>>( JsonSerializer.Serialize( filter ) )!;
		Filter.Remove("_id");
	}

	
	[JsonConstructor]
	internal MongoFilter( Dictionary<string, object> filter )
	{
		Filter = filter;
		Filter.Remove("_id");
	}

	[JsonConstructor]
	public MongoFilter( string filter )
	{
		Filter = JsonSerializer.Deserialize<Dictionary<string, object>>( filter )!;
		Filter.Remove("_id");
	}

	public static MongoFilter All => new("{}");

	public override string ToString() => JsonSerializer.Serialize( Filter );

	public static implicit operator MongoFilter( string filter ) => new(filter);

	public static implicit operator MongoFilter( Dictionary<string, object> filter ) =>
		new(JsonSerializer.Serialize( filter ));
}
