using System;
using System.Linq;
using System.Threading.Tasks;
using Mongo.Rest;
using Sandbox;

[TestClass]
public partial class LibraryTests
{
	[TestInitialize]
	public void Initialize()
	{
		var scene = new Scene();
		Configure( scene );
	}

	[TestCleanup]
	public void Cleanup()
	{
		var scene = new Scene();

		Configure( scene );
		DeleteAllRecords( scene ).GetAwaiter().GetResult();
	}

	[TestMethod]
	public void CreateUserTest()
	{
		var scene = new Scene();

		Configure( scene );
		CreateUser( scene ).GetAwaiter().GetResult();
	}

	[TestMethod]
	public void CreateMultipleUsersTest()
	{
		var scene = new Scene();

		Configure( scene );
		CreateMultipleUsers( scene ).GetAwaiter().GetResult();
	}

	[TestMethod]
	public void DeleteUserTest()
	{
		var scene = new Scene();

		Configure( scene );

		var user = CreateUser( scene ).GetAwaiter().GetResult();
		DeleteUser( scene, user ).GetAwaiter().GetResult();
	}

	[TestMethod]
	public void UpdateUserTest()
	{
		var scene = new Scene();

		Configure( scene );

		var user = CreateUser( scene ).GetAwaiter().GetResult();
		UpdateUser( scene, user ).GetAwaiter().GetResult();
	}

	[TestMethod]
	public void GetUserTest()
	{
		var scene = new Scene();

		Configure( scene );

		var user = CreateUser( scene ).GetAwaiter().GetResult();
		GetUser( scene, user ).GetAwaiter().GetResult();
	}

	[TestMethod]
	public void CountUsersTest()
	{
		var scene = new Scene();

		Configure( scene );
		CreateMultipleUsers( scene ).GetAwaiter().GetResult();
		CountAllUsers( scene, 3 ).GetAwaiter().GetResult();
	}

	[TestMethod]
	public void CountUserTest()
	{
		var scene = new Scene();

		Configure( scene );
		CreateUser( scene ).GetAwaiter().GetResult();
		CountUser( scene, 1 ).GetAwaiter().GetResult();
	}

	[TestMethod]
	public void GetUsersTest()
	{
		var scene = new Scene();

		Configure( scene );
		CreateMultipleUsers( scene ).GetAwaiter().GetResult();
		GetAllUsers( scene ).GetAwaiter().GetResult();
	}

	[TestMethod]
	public void ExistsUserTest()
	{
		var scene = new Scene();

		Configure( scene );

		var user = CreateUser( scene ).GetAwaiter().GetResult();
		ExistsUser( scene, user ).GetAwaiter().GetResult();
	}

	private static async Task<User> CreateUser( Scene scene )
	{
		var users = new UserRepository( scene );

		var user = new User { Id = Guid.NewGuid().ToString(), Name = "John Doe", Age = Random.Shared.Next( 1, 100 ) };
		var inserted = await users.InsertAsync( user );

		Assert.AreEqual( inserted, true );
		return user;
	}

	private static async Task CreateMultipleUsers( Scene scene )
	{
		var users = new UserRepository( scene );

		var user1 = new User { Id = Guid.NewGuid().ToString(), Name = "John Doe", Age = Random.Shared.Next( 1, 100 ) };
		var user2 = new User
		{
			Id = Guid.NewGuid().ToString(), Name = "Marry Jane", Age = Random.Shared.Next( 1, 100 )
		};
		var user3 = new User { Id = Guid.NewGuid().ToString(), Name = "Lola Doe", Age = Random.Shared.Next( 1, 100 ) };
		var inserted = await users.InsertAsync( user1, user2, user3 );

		Assert.AreEqual( inserted, true );
	}

	private static async Task DeleteAllRecords( Scene scene )
	{
		var users = new UserRepository( scene );
		await users.DeleteAsync();
	}

	private static async Task DeleteUser( Scene scene, User user )
	{
		var users = new UserRepository( scene );
		var deleted = await users.DeleteAsync( x => x.Name = user.Name );

		Assert.AreEqual( deleted, true );
	}

	private static async Task UpdateUser( Scene scene, User user )
	{
		var users = new UserRepository( scene );
		var updated = await users.UpdateAsync( x => x.Name = user.Name, x => x.Age = Random.Shared.Next( 1, 100 ) );

		Assert.AreEqual( updated, true );
	}

	private static async Task GetUser( Scene scene, User user )
	{
		var repo = new UserRepository( scene );

		var users = (await repo.GetAsync( x => x.Name = user.Name )).ToList();
		Assert.AreEqual( users.Count, 1 );

		var other = users.FirstOrDefault();
		Assert.IsNotNull( other );
	}

	private static async Task GetAllUsers( Scene scene )
	{
		var users = new UserRepository( scene );
		var usersList = await users.GetAsync( limit: 3 );

		Assert.AreEqual( usersList.Count(), 3 );
	}

	private static async Task ExistsUser( Scene scene, User user )
	{
		var users = new UserRepository( scene );
		var exists = await users.ExistsAsync( x => x.Name = user.Name );

		Assert.AreEqual( exists, true );
	}

	private static async Task CountAllUsers( Scene scene, int count )
	{
		var users = new UserRepository( scene );
		var total = await users.CountAsync();

		Assert.AreEqual( total, count );
	}

	private static async Task CountUser( Scene scene, int count )
	{
		var users = new UserRepository( scene );
		var total = await users.CountAsync( x => x.Name = "John Doe" );

		Assert.AreEqual( total, count );
	}

	private static void Configure( Scene scene )
	{
		var system = scene.GetSystem<MongoRestSystem>();
		system.Initialize();

		system.Configure( options =>
		{
			options.Url = "https://localhost:443";
			options.Database = "Orizon";
		} );
	}
}
