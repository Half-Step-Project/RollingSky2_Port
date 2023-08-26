using System.IO;

namespace Foundation
{
	public interface INetworkChannelProxy
	{
		int PacketHeaderLength { get; }

		void Init(INetworkChannel channel);

		bool SendHeartBeat();

		byte[] Serialize<T>(T packet) where T : Packet;

		IPacketHeader DeserializePacketHeader(Stream stream, out object errorData);

		Packet DeserializePacket(IPacketHeader header, Stream stream, out object errorData);

		void Destroy();
	}
}
