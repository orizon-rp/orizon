using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Orizon.Core.Json.Converters;

public class CharacterIdConverter : JsonConverter<CharacterId>
{
	public override CharacterId Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		if ( reader.TokenType is not JsonTokenType.String )
			throw new JsonException( $"Invalid {nameof(CharacterId)} format." );

		var characterIdString = reader.GetString();

		if ( characterIdString is not null )
			return CharacterId.Parse( characterIdString );

		throw new JsonException( $"Invalid {nameof(CharacterId)} format." );
	}

	public override void Write( Utf8JsonWriter writer, CharacterId value, JsonSerializerOptions options )
	{
		writer.WriteStringValue( value.ToString() );
	}
}
