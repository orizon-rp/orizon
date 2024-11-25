namespace Orizon.Core.Network.Extensions;

public static class KickReasonExtensions
{
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
