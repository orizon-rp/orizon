using System;
using System.Reflection;

namespace Mongo.Rest;

internal static class MongoCollectionHelper
{
	public static MongoCollectionAttribute? GetAttribute( Type type )
	{
		return type.GetCustomAttribute<MongoCollectionAttribute>();
	}

	public static void EnsureCollectionNameNotNull( Type type )
	{
		var attribute = GetAttribute( type );

		if ( attribute is null )
			throw new Exception( $"Type {type.Name} does not have a {nameof(MongoCollectionAttribute)}" );

		if ( string.IsNullOrEmpty( attribute.Name ) )
			throw new Exception(
				$"Type {type.Name} has a {nameof(MongoCollectionAttribute)} with a null {nameof(MongoCollectionAttribute.Name)}" );
	}
}
