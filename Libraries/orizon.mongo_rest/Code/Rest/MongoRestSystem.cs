using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace Mongo.Rest;

public sealed class MongoRestSystem : GameObjectSystem
{
	private bool _initialized;

	public readonly Dictionary<Type, IMongoRepository> Repositories = new();

	public IMongoRestOptions Options { get; private set; } = new MongoRestOptions
	{
		Url = "https://localhost:443",
		Database = "Orizon"
	};
	
	public MongoRestSystem( Scene scene ) : base( scene )
	{
		Listen( Stage.SceneLoaded, -1, Initialize, nameof(MongoRestSystem) );
	}

	public void Initialize()
	{
		if ( _initialized ) return;
		_initialized = true;

		Repositories.Clear();
		
		var repositories = MongoHelper.GetRepositories().ToList();
		Log.Info( $"Registered {repositories.Count} repositories" );
		
		foreach ( var repository in repositories )
			Repositories.Add( repository.GetInnerType(), repository );
	}

	public void Configure( Action<MongoRestOptions> options )
	{
		var opt = new MongoRestOptions();
		options( opt );

		Options = opt;
	}

	public IMongoRepository<T>? GetRepositoryFrom<T>() where T : class
	{
		Repositories.TryGetValue( typeof(T), out var repository );
		return repository as IMongoRepository<T>;
	}
	
	public T? GetRepository<T>() where T : class, IMongoRepository
	{
		return Repositories.Values.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
	}
}
