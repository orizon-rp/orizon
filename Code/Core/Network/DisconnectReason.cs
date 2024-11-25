namespace Orizon.Core.Network;

/// <summary>
/// Represents the reason for a disconnection from the server
/// </summary>
public enum DisconnectedReason
{
	/// <summary>
	/// No reason provided
	/// </summary>
	None,

	/// <summary>
	/// Disconnected from the server
	/// </summary>
	Disconnected,

	/// <summary>
	/// Banned from the server
	/// </summary>
	Banned,

	/// <summary>
	/// Kicked due to inactivity
	/// </summary>
	Afk,
	
	/// <summary>
	/// Kicked due to a high ping
	/// </summary>
	Ping
}
