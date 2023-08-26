using System.Net;

namespace Foundation
{
	public interface INetworkChannel
	{
		string Name { get; }

		bool ResetHeartBeatElapseSeconds { get; set; }

		float HeartBeatInterval { get; set; }

		IPFamily IPFamily { get; }

		bool Connected { get; }

		IPAddress LocalIP { get; }

		int LocalPort { get; }

		IPAddress RemoteIP { get; }

		int RemotePort { get; }

		int SendPacketCount { get; }

		int ReceivePacketCount { get; }

		int ReceiveBufferSize { get; set; }

		int SendBufferSize { get; set; }

		EventHandler<Packet> PacketHandler { get; }

		void SubscribePacketHandler(IPacketHandler handler);

		void Connect(IPAddress ip, int port, object userData);

		void Send<T>(T packet) where T : Packet;

		void Close();
	}
}
