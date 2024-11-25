using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sandbox;

namespace Mongo.Rest;

public abstract class MongoRepository<T> : IMongoRepository<T> where T : class, new()
{
	private readonly JsonSerializerOptions _options = new()
	{
		WriteIndented = false,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
	};

	public string CollectionName => MongoCollectionHelper.GetAttribute( typeof(T) )?.Name ?? string.Empty;

	/// <summary>
	/// Initializes a new instance of the <see cref="MongoRepository{T}"/> class.
	/// Mainly used for unit tests
	/// </summary>
	/// <param name="scene">The scene.</param>
	protected MongoRepository( Scene? scene = null )
	{
		System = scene is null
			? Game.ActiveScene.GetSystem<MongoRestSystem>()
			: scene.GetSystem<MongoRestSystem>();
	}

	private MongoRestSystem System { get; }
	public Type GetInnerType() => typeof(T);

	private string Url =>
		$"{System.Options.Url}/api/{System.Options.Version.ToString().ToLower()}/collections/{CollectionName}";

	/// <summary>
	/// Inserts a list of documents into the collection.
	/// </summary>
	/// <param name="values">The documents to insert.</param>
	/// <returns>
	/// A boolean indicating whether the insert was successful.
	/// </returns>
	public virtual async ValueTask<bool> InsertAsync( params T[] values )
	{
		var url = $"{Url}/insert";
		var documents = new List<BsonDocument>();

		foreach ( var value in values )
		{
			var document = BsonDocument.Parse( value );
			documents.Add( document );
		}

		var json = JsonSerializer.Serialize( documents );
		Log.Info( "Json: " + json );

		var response =
			await Http.RequestAsync( url, "POST", new StringContent( json, Encoding.UTF8, "application/json" ) );

		return response.StatusCode is HttpStatusCode.OK;
	}

	/// <summary>
	/// Retrieves a list of documents from the collection that match the filter.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <param name="limit">The maximum number of documents to return. Defaults to 100.</param>
	/// <returns>
	/// A list of documents that match the filter.
	/// </returns>
	public virtual async ValueTask<IEnumerable<T>> GetAsync( Action<T>? filter = null, int limit = 1 )
	{
		var configureFilter = new T();
		filter?.Invoke( configureFilter );

		var url = $"{Url}/get?limit={limit}";
		var json = filter is not null
			? JsonSerializer.Serialize( configureFilter, _options )
			: MongoFilter.All.ToString();
		
		var response =
			await Http.RequestAsync( url, "POST",
				new StringContent( json, Encoding.UTF8, "application/json" ) );

		if ( response.StatusCode is not HttpStatusCode.OK )
			return Array.Empty<T>();

		var jsonResult = await response.Content.ReadAsStringAsync();

		var array = JsonNode.Parse( jsonResult );
		if ( array is null ) return Array.Empty<T>();

		var documents = new List<T>();

		foreach ( var node in array.AsArray() )
		{
			if ( node is null ) continue;

			var document = node.Deserialize<BsonDocument>( _options );
			documents.Add( document.Data as T );
		}

		return documents;
	}

	/// <summary>
	/// Updates multiple documents in the collection.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <param name="update">The update to apply to the documents.</param>
	/// <returns>
	/// A boolean indicating whether the update was successful.
	/// </returns>
	public virtual async ValueTask<bool> UpdateAsync( Action<T> filter, Action<T> update )
	{
		var configureFilter = new T();
		var configureUpdate = new T();

		filter( configureFilter );
		update( configureUpdate );

		var url = $"{Url}/update";
		var filterDocument = BsonDocument.Parse( configureFilter );

		var updateDocument = BsonDocument.Parse( configureUpdate );
		updateDocument.RemoveId();

		var json = new UpdateRequest( filterDocument, updateDocument ).ToString();

		var response =
			await Http.RequestAsync( url, "PUT", new StringContent( json, Encoding.UTF8, "application/json" ) );

		return response.StatusCode is HttpStatusCode.OK;
	}

	/// <summary>
	/// Deletes multiple documents in the collection that match the filter.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <returns>
	/// A boolean indicating whether the delete was successful.
	/// </returns>
	public virtual async ValueTask<bool> DeleteAsync( Action<T>? filter = null )
	{
		var configureFilter = new T();
		filter?.Invoke( configureFilter );

		var url = $"{Url}/delete";

		var json = filter is not null
			? JsonSerializer.Serialize( configureFilter, _options )
			: MongoFilter.All.ToString();
		var content = new StringContent( json, Encoding.UTF8, "application/json" );

		var response = await Http.RequestAsync( url, "POST", content );
		return response.StatusCode is HttpStatusCode.OK;
	}

	/// <summary>
	/// Counts the number of documents that match the filter.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <returns>The number of documents that match the filter.</returns>
	public virtual async ValueTask<int> CountAsync( Action<T>? filter = null )
	{
		var configureFilter = new T();
		filter?.Invoke( configureFilter );

		var url = $"{Url}/count";

		var json = filter is not null
			? JsonSerializer.Serialize( configureFilter, _options )
			: MongoFilter.All.ToString();
		var content = new StringContent( json, Encoding.UTF8, "application/json" );

		var response = await Http.RequestAsync( url, "POST", content );
		if ( response.StatusCode is not HttpStatusCode.OK ) return 0;

		var result = await response.Content.ReadAsStringAsync();
		return int.Parse( result );
	}

	/// <summary>
	/// Checks if a document that matches the filter exists in the collection.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <returns>
	/// true if a document that matches the filter exists in the collection, false otherwise.
	/// </returns>
	public async ValueTask<bool> ExistsAsync( Action<T> filter ) => await CountAsync( filter ) > 0;
}
