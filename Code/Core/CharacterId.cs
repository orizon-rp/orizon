using System;
using System.Text.Json.Serialization;
using Orizon.Core.Json.Converters;

namespace Orizon.Core;

/// <summary>
/// Represents a character identifier
/// </summary>
[JsonConverter( typeof(CharacterIdConverter) )]
public readonly struct CharacterId
{
	/// <summary>
	/// The Steam ID of the character owner
	/// </summary>
	[JsonIgnore]
	public SteamId Owner { get; }

	/// <summary>
	/// The ID of the character
	/// </summary>
	[JsonIgnore]
	public ushort Id { get; }

	/// <summary>
	/// Initializes a new <see cref="CharacterId"/>
	/// </summary>
	/// <param name="steamId">The Steam ID of the character owner</param>
	/// <param name="characterId">The ID of the character</param>
	public CharacterId( SteamId steamId, ushort characterId ) => (Owner, Id) = (steamId, characterId);

	/// <summary>
	/// Parses a string into a new <see cref="CharacterId"/>
	/// </summary>
	/// <param name="characterId">The string to parse</param>
	/// <returns>The parsed <see cref="CharacterId"/></returns>
	/// <exception cref="ArgumentException">If the string is not a valid <see cref="CharacterId"/></exception>
	public static CharacterId Parse( string characterId )
	{
		var parts = characterId.Split( ':' );

		if ( parts.Length != 2 )
			throw new ArgumentException( $"Invalid {nameof(CharacterId)} format.", nameof(characterId) );

		var steamId = SteamId.Parse( parts[0] );
		var characterIdParsed = ushort.TryParse( parts[1], out var characterIdValue );

		if ( !characterIdParsed )
			throw new ArgumentException( $"Invalid {nameof(CharacterId)} format.", nameof(characterId) );

		return new CharacterId( steamId, characterIdValue );
	}

	/// <summary>
	/// Converts the <see cref="CharacterId"/> to a string
	/// </summary>
	/// <returns>The string representation of the <see cref="CharacterId"/></returns>
	public override string ToString() => $"{Owner}:{Id}";

	public static implicit operator string( CharacterId characterId ) => characterId.ToString();
	public static implicit operator CharacterId( string characterId ) => Parse( characterId );
}
