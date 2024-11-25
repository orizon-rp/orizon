using System;

namespace Orizon.Core;

public readonly struct CharacterId
{
	private readonly SteamId _steamId;
	private readonly ushort _characterId;

	public SteamId Owner => _steamId;
	public ushort Id => _characterId;
	
	public CharacterId( SteamId steamId, ushort characterId ) => (_steamId, _characterId) = (steamId, characterId);

	private CharacterId( string characterId )
	{
		var parts = characterId.Split( ':' );

		if ( parts.Length != 2 )
			throw new ArgumentException( "Invalid character ID format." );

		var steamId = SteamId.Parse( parts[0] );
		_steamId = steamId;

		if ( steamId is 0 )
			throw new ArgumentException( "Invalid Steam ID." );

		var parsed = ushort.TryParse( parts[1], out _characterId );

		if ( !parsed )
			throw new ArgumentException( "Invalid character ID." );
	}

	public override string ToString() => $"{_steamId}:{_characterId}";

	public static CharacterId Parse( string characterId ) => new(characterId);

	public static implicit operator string( CharacterId characterId ) => characterId.ToString();
	public static implicit operator CharacterId( string characterId ) => new(characterId);
}
