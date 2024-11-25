using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Orizon.Core.Json.Converters;

public class EntityIdConverter : JsonConverter<EntityId>
{
	public override EntityId Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		if ( reader.TokenType is JsonTokenType.Number && reader.TryGetUInt64( out var value ) )
			return value;

		throw new JsonException( $"Invalid value for {nameof(SteamId)}." );
	}

	public override void Write( Utf8JsonWriter writer, EntityId value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}
}
