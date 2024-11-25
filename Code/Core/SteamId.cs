using System;

namespace Orizon.Core;

public readonly struct SteamId : IEquatable<SteamId>, IEquatable<ulong>, IComparable, IComparable<SteamId>,
	IComparable<ulong>
{
	private readonly ulong _value;

	public static SteamId Local => Game.SteamId;

	public SteamId( ulong value )
	{
		_value = value;
	}

	public bool Equals( SteamId other )
	{
		return _value == other._value;
	}

	public bool Equals( ulong other )
	{
		return _value == other;
	}

	public int CompareTo( ulong other )
	{
		return _value.CompareTo( other );
	}

	public override bool Equals( object? obj )
	{
		return obj is SteamId other && Equals( other );
	}

	public override int GetHashCode()
	{
		return _value.GetHashCode();
	}

	public int CompareTo( object? obj )
	{
		if ( obj is SteamId other )
			return CompareTo( other );

		return _value.CompareTo( obj );
	}

	public override string ToString() => _value.ToString();

	public static implicit operator ulong( SteamId id ) => id._value;
	public static implicit operator SteamId( long id ) => new((ulong)id);
	public static implicit operator SteamId( ulong id ) => new(id);

	public static bool operator ==( SteamId id1, SteamId id2 ) => id1._value == id2._value;
	public static bool operator !=( SteamId id1, SteamId id2 ) => id1._value != id2._value;

	public int CompareTo( SteamId other )
	{
		return _value.CompareTo( other._value );
	}
}
