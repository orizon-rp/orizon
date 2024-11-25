using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Rest;

public interface IMongoRepository
{
	/// <summary>
	/// Gets the name of the collection in the database.
	/// </summary>
	/// <value>The name of the collection.</value>
	string CollectionName { get; }

	/// <summary>
	/// Gets the underlying type of the repository.
	/// </summary>
	/// <returns>The underlying type of the repository.</returns>
	Type GetInnerType();
}

public interface IMongoRepository<T> : IMongoRepository where T : class
{
	/// <summary>
	/// Inserts multiple documents into the collection.
	/// </summary>
	/// <param name="values">The documents to insert.</param>
	/// <returns>
	/// true if the insert operation is successful, false otherwise.
	/// </returns>
	ValueTask<bool> InsertAsync( params T[] values );

	/// <summary>
	/// Retrieves a list of documents from the collection that match the filter.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <param name="limit">The maximum number of documents to return. Defaults to 100.</param>
	/// <returns>
	/// A list of documents that match the filter.
	/// </returns>
	ValueTask<IEnumerable<T>> GetAsync( Action<T>? filter = null, int limit = 1 );
	
	/// <summary>
	/// Updates multiple documents in the collection.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <param name="update">The update to apply to the documents.</param>
	/// <returns>
	/// A boolean indicating whether the update was successful.
	/// </returns>
	ValueTask<bool> UpdateAsync( Action<T> filter, Action<T> update );

	/// <summary>
	/// Deletes multiple documents in the collection that match the filter.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <returns>
	/// A boolean indicating whether the delete was successful.
	/// </returns>
	ValueTask<bool> DeleteAsync( Action<T>? filter = null );
	
	/// <summary>
	/// Counts the number of documents that match the filter.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <returns>The number of documents that match the filter.</returns>
	ValueTask<int> CountAsync( Action<T>? filter = null );

	/// <summary>
	/// Checks if a document exists in the collection that matches the filter.
	/// </summary>
	/// <param name="filter">The filter to apply to the documents.</param>
	/// <returns>
	/// A boolean indicating whether the document exists.
	/// </returns>
	ValueTask<bool> ExistsAsync( Action<T> filter );
}
