using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.Rest;

internal static class MongoHelper
{
	private static IMongoRepository CreateRepository( Type type ) => TypeLibrary.Create<IMongoRepository>( type );

	public static IEnumerable<IMongoRepository> GetRepositories()
	{
		var types = TypeLibrary.GetTypes<IMongoRepository>()
			.Where( x => x is { IsAbstract: false, IsInterface: false } );

		foreach ( var type in types )
			yield return CreateRepository( type.TargetType );
	}
}
