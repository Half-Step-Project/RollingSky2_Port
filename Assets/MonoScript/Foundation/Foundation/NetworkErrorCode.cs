namespace Foundation
{
	public enum NetworkErrorCode
	{
		AddressFamilyError = 0,
		SocketError = 1,
		SerializeError = 2,
		DeserializePacketHeaderError = 3,
		DeserializePacketError = 4,
		ConnectError = 5,
		SendError = 6,
		ReceiveError = 7
	}
}
