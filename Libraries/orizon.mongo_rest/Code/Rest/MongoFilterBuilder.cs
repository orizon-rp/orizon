using System.Collections.Generic;

namespace Mongo.Rest;

public sealed class MongoFilterBuilder
{
	private readonly Dictionary<string, object> _filter = new();

	private MongoFilterBuilder Append( string name, object value )
	{
		_filter[name] = value;
		return this;
	}

	public MongoFilterBuilder WithId( string id ) => Append( "_id", id );

	public MongoFilterBuilder WithValue( string name, object value ) => Append( name, value );

	public MongoFilterBuilder WithValues( Dictionary<string, object> values )
	{
		foreach ( var (name, value) in values )
			Append( name, value );

		return this;
	}

	public MongoFilter Build() => new( _filter );
}
