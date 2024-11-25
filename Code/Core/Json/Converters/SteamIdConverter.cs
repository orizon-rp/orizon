using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Orizon.Core.Json.Converters;

public class SteamIdConverter : JsonConverter<SteamId>
{
	public override SteamId Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		if ( reader.TokenType is JsonTokenType.Number && reader.TryGetUInt64( out var value ) )
			return value;

		throw new JsonException( "Invalid value for SteamId." );
	}

	public override void Write( Utf8JsonWriter writer, SteamId value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}
}
