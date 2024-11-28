using System;
using System.Text.Json.Serialization;
using Orizon.Core.Json.Converters;

namespace Orizon.Core;

/// <summary>
/// Represents a SteamID
/// </summary>
[JsonConverter( typeof(SteamIdConverter) )]
public readonly struct SteamId : IEquatable<SteamId>, IEquatable<ulong>, IComparable, IComparable<SteamId>,
	IComparable<ulong>
{
	private readonly ulong _value;

	/// <summary>
	/// Get your own SteamId
	/// </summary>
	public static SteamId Local => Game.SteamId;

	/// <summary>
	/// Initializes a new SteamID from an ulong
	/// </summary>
	/// <param name="value"></param>
	private SteamId( ulong value )
	{
		_value = value;
	}

	/// <summary>
	/// Checks if two SteamIDs are equal
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	public bool Equals( SteamId other )
	{
		return _value == other._value;
	}

	/// <summary>
	/// Checks if a SteamID is equal to an ulong
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	public bool Equals( ulong other )
	{
		return _value == other;
	}

	/// <summary>
	/// Compares a SteamID to an ulong
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	public int CompareTo( ulong other )
	{
		return _value.CompareTo( other );
	}

	/// <summary>
	/// Checks if a SteamID is equal to an object
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public override bool Equals( object? obj )
	{
		return obj is SteamId other && Equals( other );
	}

	/// <summary>
	/// Gets the hash code for the SteamID
	/// </summary>
	/// <returns></returns>
	public override int GetHashCode()
	{
		return _value.GetHashCode();
	}

	/// <summary>
	/// Compares a SteamID to an object
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public int CompareTo( object? obj )
	{
		if ( obj is SteamId other )
			return CompareTo( other );

		return _value.CompareTo( obj );
	}

	/// <summary>
	/// Compares a SteamID to another SteamID
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	public int CompareTo( SteamId other )
	{
		return _value.CompareTo( other._value );
	}

	/// <summary>
	/// Parses a string into a SteamID
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static ulong Parse( string value )
	{
		return ulong.TryParse( value, out var steamId ) ? steamId : 0;
	}

	/// <summary>
	/// Return what type os SteamId this is
	/// </summary>
	[JsonIgnore]
	public Sandbox.SteamId.AccountTypes AccountType
	{
		get => (Sandbox.SteamId.AccountTypes)(byte)(_value >> 52 & 15UL);
	}

	/// <summary>
	/// Converts the SteamID to a string
	/// </summary>
	/// <returns></returns>
	public override string ToString() => _value.ToString();

	public static implicit operator ulong( SteamId id ) => id._value;
	public static implicit operator SteamId( long id ) => new((ulong)id);
	public static implicit operator SteamId( ulong id ) => new(id);
	public static implicit operator SteamId( Sandbox.SteamId steamId ) => new(steamId.ValueUnsigned);
	public static implicit operator Sandbox.SteamId( SteamId steamId ) => new(steamId._value);

	public static bool operator ==( SteamId left, SteamId right ) => left._value == right._value;
	public static bool operator !=( SteamId left, SteamId right ) => left._value != right._value;
}
