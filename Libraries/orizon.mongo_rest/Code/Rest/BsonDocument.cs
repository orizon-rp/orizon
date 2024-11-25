using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mongo.Rest;

/// <summary>
/// A BSON document that can be used to interact with the REST API.
/// </summary>
[JsonConverter( typeof(BsonDocumentJsonConverter) )]
public record struct BsonDocument
{
	private readonly JsonSerializerOptions _options = new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
	};

	/// <summary>
	/// The document id.
	/// </summary>
	[JsonIgnore]
	public string Id { get; private set; } = null!;

	/// <summary>
	/// The name of the type that this document represents.
	/// </summary>
	[JsonIgnore]
	public string TypeName { get; private set; } = null!;

	[JsonIgnore] public object Data { get; private set; } = null!;

	private BsonDocument( string json )
	{
		var data = JsonSerializer.Deserialize<Dictionary<string, object>>( json );

		if ( data is null )
		{
			Log.Error( "Failed to deserialize BsonDocument: " + json );
			return;
		}

		data.TryGetValue( "_id", out var id );
		Id = id?.ToString() ?? string.Empty;

		data.TryGetValue( "_type", out var typeName );
		TypeName = typeName?.ToString() ?? string.Empty;

		var type = TypeLibrary.GetType( TypeName );
		
		json = JsonSerializer.Serialize( data, _options );
		Data = JsonSerializer.Deserialize( json, type.TargetType )!;
	}

	private BsonDocument( object value )
	{
		Data = value;
		TypeName = value.GetType().Name;

		var type = TypeLibrary.GetType( TypeName );

		if ( type is null )
		{
			Log.Error( "Failed to find type: " + TypeName );
			return;
		}

		// Assign the id property from the value
		foreach ( var property in type.Properties.Where( x => x.IsPublic ) )
		{
			var propertyValue = property.GetValue( value );
			if ( propertyValue is null ) continue;

			var propertyAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>();

			if ( propertyAttr?.Name is "_id" )
				Id = propertyValue.ToString()!;
		}
	}

	/// <summary>
	/// Removes the "_id" field from the document.
	/// </summary>
	internal void RemoveId()
	{
		var type = TypeLibrary.GetType( TypeName );

		if ( type is null )
		{
			Log.Error( "Failed to find type: " + TypeName );
			return;
		}

		foreach ( var property in type.Properties )
		{
			var propertyValue = property.GetValue( Data );
			if ( propertyValue is null ) continue;

			var propertyAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>();

			if ( propertyAttr?.Name is "_id" )
				property.SetValue( Data, null );
		}
	}

	/// <summary>
	/// Converts the BSON document to a JSON string.
	/// </summary>
	/// <returns>A JSON string representation of the BSON document.</returns>
	public override string ToString() => JsonSerializer.Serialize( Data, _options );

	/// <summary>
	/// Converts the BSON document to an object.
	/// </summary>
	/// <returns>The object representation of the BSON document.</returns>
	public JsonDocument ToObject() => JsonDocument.Parse( ToString() );

	/// <summary>
	/// Parses an object into a BSON document.
	/// </summary>
	/// <param name="value">The object to parse.</param>
	/// <returns>The BSON document parsed from the object.</returns>
	public static BsonDocument Parse( object value ) => new(value);

	public static BsonDocument Parse( string value ) => new(value);

	public static implicit operator BsonDocument( string json ) => new(json);
}
