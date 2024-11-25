using System;
using System.Text.Json.Serialization;
using Orizon.Core.Json.Converters;

namespace Orizon.Core;

/// <summary>
/// Represents a entitiy identifier.
/// </summary>
[JsonConverter( typeof(EntityIdConverter) )]
public readonly struct EntityId : IEquatable<EntityId>, IEquatable<ulong>, IComparable, IComparable<EntityId>,
	IComparable<ulong>
{
	private readonly ulong _value;

	/// <summary>
	/// Initializes a new instance of the <see cref="EntityId"/> struct.
	/// </summary>
	/// <param name="value">The value of the ID.</param>
	private EntityId( ulong value ) => _value = value;

	/// <summary>
	/// Compares the current instance with another object and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
	/// </summary>
	/// <param name="obj">An object to compare with this instance.</param>
	/// <returns>
	/// A value that indicates the relative position of the objects being compared. The return value is less than zero if the current instance precedes <paramref name="obj"/>, zero if the current instance is in the same position as <paramref name="obj"/>, and greater than zero if the current instance follows <paramref name="obj"/>.
	/// </returns>
	public int CompareTo( object? obj )
	{
		if ( obj is EntityId other )
			return _value.CompareTo( other._value );

		return _value.CompareTo( obj );
	}

	/// <summary>
	/// Compares the current instance with another <see cref="EntityId"/> and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other <see cref="EntityId"/>.
	/// </summary>
	/// <param name="other">An <see cref="EntityId"/> to compare with this instance.</param>
	/// <returns>
	/// A value that indicates the relative position of the objects being compared. The return value is less than zero if the current instance precedes <paramref name="other"/>, zero if the current instance is in the same position as <paramref name="other"/>, and greater than zero if the current instance follows <paramref name="other"/>.
	/// </returns>
	public int CompareTo( ulong other )
	{
		return _value.CompareTo( other );
	}

	/// <summary>
	/// Compares the current instance with another <see cref="EntityId"/> and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other <see cref="EntityId"/>.
	/// </summary>
	/// <param name="other">An <see cref="EntityId"/> to compare with this instance.</param>
	/// <returns>
	/// A value that indicates the relative position of the objects being compared. The return value is less than zero if the current instance precedes <paramref name="other"/>, zero if the current instance is in the same position as <paramref name="other"/>, and greater than zero if the current instance follows <paramref name="other"/>.
	/// </returns>
	public int CompareTo( EntityId other )
	{
		return _value.CompareTo( other._value );
	}

	/// <summary>
	/// Determines whether the specified <see cref="EntityId"/> is equal to the current instance.
	/// </summary>
	/// <param name="other">An <see cref="EntityId"/> to compare with this instance.</param>
	/// <returns>
	/// <see langword="true"/> if the specified <see cref="EntityId"/> is equal to the current instance; otherwise, <see langword="false"/>.
	/// </returns>
	public bool Equals( EntityId other )
	{
		return _value == other._value;
	}

	/// <summary>
	/// Determines whether the specified <see cref="ulong"/> is equal to the current instance.
	/// </summary>
	/// <param name="other">A <see cref="ulong"/> to compare with this instance.</param>
	/// <returns>
	/// <see langword="true"/> if the specified <see cref="ulong"/> is equal to the current instance; otherwise, <see langword="false"/>.
	/// </returns>
	public bool Equals( ulong other )
	{
		return _value == other;
	}

	/// <summary>
	/// Determines whether the specified <see cref="object"/> is equal to the current instance.
	/// </summary>
	/// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
	/// <returns>
	/// <see langword="true"/> if the specified <see cref="object"/> is equal to the current instance; otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals( object? obj )
	{
		return obj is EntityId other && Equals( other );
	}

	/// <summary>
	/// Returns a hash code for the current instance.
	/// </summary>
	/// <returns>
	/// A hash code for the current instance.
	/// </returns>
	public override int GetHashCode()
	{
		return _value.GetHashCode();
	}

	/// <summary>
	/// Converts the value of the current instance to its equivalent string representation.
	/// </summary>
	/// <returns>
	/// The string representation of the value of the current instance.
	/// </returns>
	public override string ToString() => _value.ToString();

	public static implicit operator ulong( EntityId value ) => value._value;
	public static implicit operator EntityId( ulong value ) => new(value);

	public static bool operator ==( EntityId left, EntityId right ) => left._value == right._value;
	public static bool operator !=( EntityId left, EntityId right ) => left._value != right._value;
}
