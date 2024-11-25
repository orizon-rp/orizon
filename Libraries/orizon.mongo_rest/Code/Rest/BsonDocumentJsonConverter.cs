using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mongo.Rest;

public class BsonDocumentJsonConverter : JsonConverter<BsonDocument>
{
	public override BsonDocument Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		using var document = JsonDocument.ParseValue( ref reader );

		var jsonObject = document.RootElement;
		var json = jsonObject.GetRawText();

		return BsonDocument.Parse( json );
	}

	public override void Write( Utf8JsonWriter writer, BsonDocument value, JsonSerializerOptions options )
	{
		var data = value.Data;
		var json = JsonSerializer.Serialize( data, options );

		var dict = JsonSerializer.Deserialize<Dictionary<string, object>>( json );
		if ( dict is null ) return;

		var orderedDict = new Dictionary<string, object> { { "_id", value.Id }, { "_type", value.TypeName } };

		foreach ( var kvp in dict )
		{
			if ( kvp.Key is "_id" or "_type" ) continue;
			orderedDict[kvp.Key] = kvp.Value;
		}

		JsonSerializer.Serialize( writer, orderedDict, options );
	}
}
