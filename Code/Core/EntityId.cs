using System;
using System.Text.Json.Serialization;
using Orizon.Core.Json.Converters;

namespace Orizon.Core;

[JsonConverter( typeof(EntityIdConverter) )]
public readonly struct EntityId : IEquatable<EntityId>, IEquatable<ulong>, IComparable, IComparable<EntityId>,
	IComparable<ulong>
{
	private readonly ulong _value;

	private EntityId( ulong value ) => _value = value;

	public int CompareTo( object? obj )
	{
		if ( obj is EntityId other )
			return _value.CompareTo( other._value );

		return _value.CompareTo( obj );
	}

	public bool Equals( EntityId other )
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
		return obj is EntityId other && Equals( other );
	}

	public override int GetHashCode()
	{
		return _value.GetHashCode();
	}

	public int CompareTo( EntityId other )
	{
		return _value.CompareTo( other._value );
	}

	public override string ToString() => _value.ToString();

	public static implicit operator EntityId( ulong value ) => new(value);
	public static implicit operator ulong( EntityId value ) => value._value;

	public static bool operator ==( EntityId left, EntityId right ) => left._value == right._value;
	public static bool operator !=( EntityId left, EntityId right ) => left._value != right._value;
}
