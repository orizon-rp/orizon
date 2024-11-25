using System.Collections.Generic;
using System.Text.Json;

namespace Mongo.Rest;

public record UpdateRequest( BsonDocument Filter, BsonDocument Update )
{
	public override string ToString()
	{
		var dict = new Dictionary<string, object> { { "Filter", Filter.ToObject() }, { "Update", Update.ToObject() } };
		return JsonSerializer.Serialize( dict );
	}
}
