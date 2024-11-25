using System;

namespace Mongo.Rest;

[AttributeUsage( AttributeTargets.Class )]
public sealed class MongoCollectionAttribute : Attribute
{
	public string Name { get; init; }

	public MongoCollectionAttribute( string name )
	{
		Name = name;
	}
}
