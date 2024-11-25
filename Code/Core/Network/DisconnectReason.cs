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

/// <summary>
/// Extensions for the <see cref="DisconnectedReason"/> enum
/// </summary>
public static class DisconnectReasonExtensions
{
	/// <summary>
	/// Converts the <see cref="DisconnectedReason"/> to a human-readable message
	/// </summary>
	/// <param name="disconnectReason">The <see cref="DisconnectedReason"/> to convert</param>
	/// <returns>The human-readable message</returns>
	public static string ToMessage( this DisconnectedReason disconnectReason ) => disconnectReason switch
	{
		DisconnectedReason.None => "No reason provided for kick.",
		DisconnectedReason.Disconnected => "Disconnected from the server.",
		DisconnectedReason.Banned => "Banned from the server.",
		DisconnectedReason.Afk => "AFK for to long.",
		DisconnectedReason.Ping => "High ping.",
		_ => "Unknown reason"
	};
}
