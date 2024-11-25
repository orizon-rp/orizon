using Sandbox;

namespace Mongo.Rest;

public static class SceneExtensions
{
	public static IMongoRepository<T>? GetRepositoryFrom<T>( this Scene scene ) where T : class
	{
		var system = scene.GetSystem<MongoRestSystem>();
		return system.GetRepositoryFrom<T>();
	}

	public static T? GetRepository<T>( this Scene scene ) where T : class, IMongoRepository
	{
		var system = scene.GetSystem<MongoRestSystem>();
		return system.GetRepository<T>();
	}
}
