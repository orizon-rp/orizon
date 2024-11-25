namespace Mongo.Rest;

public interface IMongoRestOptions
{
	string Url { get; }
	MongoRestVersion Version { get; }
	
	/// <summary>
	/// Not implemented yet
	/// </summary>
	string Database { get; }
}

public sealed class MongoRestOptions : IMongoRestOptions
{
	public string Url { get; set; } = null!;
	public MongoRestVersion Version { get; set; }
	public string Database { get; set; } = null!;
}
