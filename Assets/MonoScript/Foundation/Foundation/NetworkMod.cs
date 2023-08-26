using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Foundation
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Framework/Network")]
	public sealed class NetworkMod : ModBase
	{
		private sealed class Channel : IDisposable, INetworkChannel
		{
			private sealed class ConnectState
			{
				[CompilerGenerated]
				private readonly Socket _003CSocket_003Ek__BackingField;

				[CompilerGenerated]
				private readonly object _003CUserData_003Ek__BackingField;

				public Socket Socket
				{
					[CompilerGenerated]
					get
					{
						return _003CSocket_003Ek__BackingField;
					}
				}

				public object UserData
				{
					[CompilerGenerated]
					get
					{
						return _003CUserData_003Ek__BackingField;
					}
				}

				public ConnectState(Socket socket, object userData)
				{
					_003CSocket_003Ek__BackingField = socket;
					_003CUserData_003Ek__BackingField = userData;
				}
			}

			private sealed class HeartBeatState
			{
				public float HeartBeatElapseSeconds { get; set; }

				public int MissHeartBeatCount { get; set; }

				public void Reset(bool resetHeartBeatElapseSeconds)
				{
					if (resetHeartBeatElapseSeconds)
					{
						HeartBeatElapseSeconds = 0f;
					}
					MissHeartBeatCount = 0;
				}
			}

			private sealed class ReceiveState
			{
				private const int DefaultBufferLength = 8192;

				private readonly MemoryStream _stream = new MemoryStream(8192);

				private IPacketHeader _header;

				public MemoryStream Stream
				{
					get
					{
						return _stream;
					}
				}

				public IPacketHeader Header
				{
					get
					{
						return _header;
					}
				}

				public void PreparePacketHeader(int length)
				{
					Reset(length, null);
				}

				public void PreparePacket(IPacketHeader header)
				{
					if (header == null)
					{
						Log.Warning("Packet header is invalid.");
					}
					else
					{
						Reset(header.PacketLength, header);
					}
				}

				private void Reset(int length, IPacketHeader header)
				{
					if (length < 0)
					{
						Log.Warning("Length is invalid.");
						return;
					}
					_stream.Position = 0L;
					_stream.SetLength(length);
					_header = header;
				}
			}

			private sealed class SendState
			{
				public byte[] Bytes { get; private set; }

				public int Offset { get; set; }

				public int Length { get; private set; }

				public bool IsFree
				{
					get
					{
						return Bytes == null;
					}
				}

				public void Set(byte[] bytes)
				{
					Bytes = bytes;
					Offset = 0;
					Length = bytes.Length;
				}

				public void Reset()
				{
					Bytes = null;
					Offset = 0;
					Length = 0;
				}
			}

			private readonly Queue<Packet> _sendPackets = new Queue<Packet>();

			private readonly EventPool<Packet> _receivePackets = new EventPool<Packet>(EventMode.Default);

			private readonly INetworkChannelProxy _channelProxy;

			private Socket _socket;

			private readonly SendState _sendState = new SendState();

			private readonly ReceiveState _receiveState = new ReceiveState();

			private readonly HeartBeatState _heartBeatState = new HeartBeatState();

			private bool _active;

			private bool _disposed;

			[CompilerGenerated]
			private readonly string _003CName_003Ek__BackingField;

			public string Name
			{
				[CompilerGenerated]
				get
				{
					return _003CName_003Ek__BackingField;
				}
			}

			public bool ResetHeartBeatElapseSeconds { get; set; }

			public float HeartBeatInterval { get; set; }

			public IPFamily IPFamily { get; private set; }

			public bool Connected
			{
				get
				{
					Socket socket = _socket;
					if (socket == null)
					{
						return false;
					}
					return socket.Connected;
				}
			}

			public IPAddress LocalIP
			{
				get
				{
					Socket socket = _socket;
					IPEndPoint iPEndPoint = (IPEndPoint)((socket != null) ? socket.LocalEndPoint : null);
					if (iPEndPoint == null)
					{
						return null;
					}
					return iPEndPoint.Address;
				}
			}

			public int LocalPort
			{
				get
				{
					Socket socket = _socket;
					IPEndPoint iPEndPoint = (IPEndPoint)((socket != null) ? socket.LocalEndPoint : null);
					if (iPEndPoint == null)
					{
						return 0;
					}
					return iPEndPoint.Port;
				}
			}

			public IPAddress RemoteIP
			{
				get
				{
					Socket socket = _socket;
					IPEndPoint iPEndPoint = (IPEndPoint)((socket != null) ? socket.RemoteEndPoint : null);
					if (iPEndPoint == null)
					{
						return null;
					}
					return iPEndPoint.Address;
				}
			}

			public int RemotePort
			{
				get
				{
					Socket socket = _socket;
					IPEndPoint iPEndPoint = (IPEndPoint)((socket != null) ? socket.RemoteEndPoint : null);
					if (iPEndPoint == null)
					{
						return 0;
					}
					return iPEndPoint.Port;
				}
			}

			public int SendPacketCount
			{
				get
				{
					lock (_sendPackets)
					{
						return _sendPackets.Count;
					}
				}
			}

			public int ReceivePacketCount
			{
				get
				{
					return _receivePackets.Count;
				}
			}

			public int ReceiveBufferSize
			{
				get
				{
					Socket socket = _socket;
					if (socket == null)
					{
						return 0;
					}
					return socket.ReceiveBufferSize;
				}
				set
				{
					if (_socket != null)
					{
						_socket.ReceiveBufferSize = value;
					}
				}
			}

			public int SendBufferSize
			{
				get
				{
					Socket socket = _socket;
					if (socket == null)
					{
						return 0;
					}
					return socket.SendBufferSize;
				}
				set
				{
					if (_socket != null)
					{
						_socket.SendBufferSize = value;
					}
				}
			}

			public EventHandler<Packet> PacketHandler
			{
				get
				{
					return _receivePackets.Handler;
				}
			}

			public Channel(string name, INetworkChannelProxy channelProxy, float heartBeatInterval = 30f)
			{
				HeartBeatInterval = heartBeatInterval;
				_003CName_003Ek__BackingField = name ?? string.Empty;
				_channelProxy = channelProxy;
				channelProxy.Init(this);
			}

			~Channel()
			{
				Dispose();
			}

			public void SubscribePacketHandler(IPacketHandler handler)
			{
				if (handler == null)
				{
					Log.Warning("Packet handler is invalid.");
				}
				else
				{
					_receivePackets.Subscribe(handler.Id, handler.Action);
				}
			}

			public void Connect(IPAddress ip, int port, object userData = null)
			{
				if (_socket != null)
				{
					Close();
					_socket = null;
				}
				switch (ip.AddressFamily)
				{
				case AddressFamily.InterNetwork:
					IPFamily = IPFamily.IPv4;
					break;
				case AddressFamily.InterNetworkV6:
					IPFamily = IPFamily.IPv6;
					break;
				default:
					OnChannelError(NetworkErrorCode.AddressFamilyError, "Not supported address family " + ip.AddressFamily);
					return;
				}
				_socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				if (_socket == null)
				{
					OnChannelError(NetworkErrorCode.SocketError, "Initialize network channel failure.");
					return;
				}
				_receiveState.PreparePacketHeader(_channelProxy.PacketHeaderLength);
				try
				{
					_socket.BeginConnect(ip, port, OnSocketConnect, new ConnectState(_socket, userData));
				}
				catch (Exception ex)
				{
					OnChannelError(NetworkErrorCode.ConnectError, ex.Message);
				}
			}

			public void Send<T>(T packet) where T : Packet
			{
				if (_socket == null)
				{
					OnChannelError(NetworkErrorCode.SocketError, "You must connect first.");
					return;
				}
				if (packet == null)
				{
					OnChannelError(NetworkErrorCode.SendError, "Packet is invalid.");
					return;
				}
				lock (_sendPackets)
				{
					_sendPackets.Enqueue(packet);
				}
			}

			public void Tick(float realElapseSeconds)
			{
				Profiler.BeginSample("NetworkMod.Tick");
				if (_socket == null || !_active)
				{
					return;
				}
				ProcessSend();
				_receivePackets.Tick();
				if (HeartBeatInterval > 0f)
				{
					bool flag = false;
					int num = 0;
					lock (_heartBeatState)
					{
						_heartBeatState.HeartBeatElapseSeconds += realElapseSeconds;
						if (_heartBeatState.HeartBeatElapseSeconds >= HeartBeatInterval)
						{
							flag = true;
							num = _heartBeatState.MissHeartBeatCount;
							_heartBeatState.HeartBeatElapseSeconds = 0f;
							_heartBeatState.MissHeartBeatCount++;
						}
					}
					if (flag && _channelProxy.SendHeartBeat() && num > 0)
					{
						OnChannelMissHeartBeat(num);
					}
				}
				Profiler.EndSample();
			}

			public void Close()
			{
				lock (this)
				{
					if (_socket == null)
					{
						return;
					}
					lock (_sendPackets)
					{
						_sendPackets.Clear();
					}
					_receivePackets.RemoveAllEvents();
					_active = false;
					try
					{
						_socket.Shutdown(SocketShutdown.Both);
					}
					catch
					{
					}
					finally
					{
						_socket.Close();
						_socket = null;
						OnChannelClosed();
					}
				}
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			public void Destroy()
			{
				Close();
				_receivePackets.Destroy();
				_channelProxy.Destroy();
			}

			private void Dispose(bool disposing)
			{
				if (!_disposed)
				{
					if (disposing)
					{
						Close();
					}
					_disposed = true;
				}
			}

			private void Send()
			{
				try
				{
					_socket.BeginSend(_sendState.Bytes, _sendState.Offset, _sendState.Length, SocketFlags.None, OnSocketSend, _socket);
				}
				catch (Exception ex)
				{
					_active = false;
					OnChannelError(NetworkErrorCode.SendError, ex.Message);
				}
			}

			private void Receive()
			{
				try
				{
					byte[] buffer = _receiveState.Stream.GetBuffer();
					int offset = (int)_receiveState.Stream.Position;
					int size = (int)(_receiveState.Stream.Length - _receiveState.Stream.Position);
					_socket.BeginReceive(buffer, offset, size, SocketFlags.None, OnSocketReceive, _socket);
				}
				catch (Exception ex)
				{
					_active = false;
					OnChannelError(NetworkErrorCode.ReceiveError, ex.Message);
				}
			}

			private void ProcessSend()
			{
				Profiler.BeginSample("NetworkMod.Channel.ProcessSend");
				lock (_sendPackets)
				{
					if (_sendPackets.Count <= 0)
					{
						return;
					}
				}
				if (_sendState.IsFree)
				{
					Packet packet;
					lock (_sendPackets)
					{
						packet = _sendPackets.Dequeue();
					}
					byte[] array;
					try
					{
						array = _channelProxy.Serialize(packet);
					}
					catch (Exception ex)
					{
						_active = false;
						OnChannelError(NetworkErrorCode.SerializeError, ex.ToString());
						return;
					}
					if (array == null || array.Length == 0)
					{
						OnChannelError(NetworkErrorCode.SerializeError, "Serialized packet is invalid.");
						return;
					}
					_sendState.Set(array);
					Send();
					Profiler.EndSample();
				}
			}

			private bool ProcessPacketHeader()
			{
				try
				{
					object errorData;
					IPacketHeader packetHeader = _channelProxy.DeserializePacketHeader(_receiveState.Stream, out errorData);
					if (errorData != null)
					{
						OnChannelCustomError(errorData);
					}
					if (packetHeader == null)
					{
						OnChannelError(NetworkErrorCode.DeserializePacketHeaderError, "Packet header is invalid.");
						return false;
					}
					_receiveState.PreparePacket(packetHeader);
					if (packetHeader.PacketLength <= 0)
					{
						ProcessPacket();
					}
				}
				catch (Exception ex)
				{
					_active = false;
					OnChannelError(NetworkErrorCode.DeserializePacketHeaderError, ex.ToString());
					return false;
				}
				return true;
			}

			private bool ProcessPacket()
			{
				lock (_heartBeatState)
				{
					_heartBeatState.Reset(ResetHeartBeatElapseSeconds);
				}
				try
				{
					object errorData;
					Packet packet = _channelProxy.DeserializePacket(_receiveState.Header, _receiveState.Stream, out errorData);
					if (errorData != null)
					{
						OnChannelCustomError(errorData);
						return false;
					}
					if (packet != null)
					{
						_receivePackets.Fire(this, packet);
					}
					_receiveState.PreparePacketHeader(_channelProxy.PacketHeaderLength);
				}
				catch (Exception ex)
				{
					_active = false;
					OnChannelError(NetworkErrorCode.DeserializePacketError, ex.ToString());
					return false;
				}
				return true;
			}

			private void OnSocketConnect(IAsyncResult ar)
			{
				ConnectState connectState = (ConnectState)ar.AsyncState;
				try
				{
					connectState.Socket.EndConnect(ar);
				}
				catch (ObjectDisposedException)
				{
					return;
				}
				catch (Exception ex2)
				{
					_active = false;
					OnChannelError(NetworkErrorCode.ConnectError, ex2.Message);
					return;
				}
				_active = true;
				lock (_heartBeatState)
				{
					_heartBeatState.Reset(true);
				}
				OnChannelConnected(connectState.UserData);
				Receive();
			}

			private void OnSocketSend(IAsyncResult ar)
			{
				Socket socket = (Socket)ar.AsyncState;
				try
				{
					_sendState.Offset += socket.EndSend(ar);
				}
				catch (ObjectDisposedException)
				{
					return;
				}
				catch (Exception ex2)
				{
					_active = false;
					OnChannelError(NetworkErrorCode.SendError, ex2.Message);
					return;
				}
				if (_sendState.Offset < _sendState.Length)
				{
					Send();
				}
				else
				{
					_sendState.Reset();
				}
			}

			private void OnSocketReceive(IAsyncResult ar)
			{
				Socket socket = (Socket)ar.AsyncState;
				int num;
				try
				{
					num = socket.EndReceive(ar);
				}
				catch (ObjectDisposedException)
				{
					return;
				}
				catch (Exception ex2)
				{
					_active = false;
					OnChannelError(NetworkErrorCode.ReceiveError, ex2.Message);
					return;
				}
				if (num <= 0)
				{
					Close();
					return;
				}
				_receiveState.Stream.Position += num;
				if (_receiveState.Stream.Position < _receiveState.Stream.Length)
				{
					Receive();
					return;
				}
				_receiveState.Stream.Position = 0L;
				if ((_receiveState.Header != null) ? ProcessPacket() : ProcessPacketHeader())
				{
					Receive();
				}
			}

			private void OnChannelConnected(object userData)
			{
				NetworkMod network = Mod.Network;
				network.OnChannelConnected(this, userData);
			}

			private void OnChannelCustomError(object errorData)
			{
				NetworkMod network = Mod.Network;
				network.OnChannelCustomError(this, errorData);
			}

			private void OnChannelClosed()
			{
				NetworkMod network = Mod.Network;
				network.OnChannelClosed(this);
			}

			private void OnChannelError(NetworkErrorCode errorCode, string message)
			{
				NetworkMod network = Mod.Network;
				network.OnChannelError(this, errorCode, message);
			}

			private void OnChannelMissHeartBeat(int missCount)
			{
				NetworkMod network = Mod.Network;
				network.OnChannelMissHeartBeat(this, missCount);
			}
		}

		public sealed class ClosedEventArgs : EventArgs<ClosedEventArgs>
		{
			public INetworkChannel Channel { get; private set; }

			public static ClosedEventArgs Make(INetworkChannel channel)
			{
				ClosedEventArgs closedEventArgs = Mod.Reference.Acquire<ClosedEventArgs>();
				closedEventArgs.Channel = channel;
				return closedEventArgs;
			}

			protected override void OnRecycle()
			{
				Channel = null;
			}
		}

		public sealed class ConnectedEventArgs : EventArgs<ConnectedEventArgs>
		{
			public INetworkChannel Channel { get; private set; }

			public object UserData { get; private set; }

			public static ConnectedEventArgs Make(INetworkChannel channel, object userData)
			{
				ConnectedEventArgs connectedEventArgs = Mod.Reference.Acquire<ConnectedEventArgs>();
				connectedEventArgs.Channel = channel;
				connectedEventArgs.UserData = userData;
				return connectedEventArgs;
			}

			protected override void OnRecycle()
			{
				Channel = null;
				UserData = null;
			}
		}

		public sealed class CustomErrorEventArgs : EventArgs<CustomErrorEventArgs>
		{
			public INetworkChannel Channel { get; private set; }

			public object ErrorData { get; private set; }

			public static CustomErrorEventArgs Make(INetworkChannel channel, object errorData)
			{
				CustomErrorEventArgs customErrorEventArgs = Mod.Reference.Acquire<CustomErrorEventArgs>();
				customErrorEventArgs.Channel = channel;
				customErrorEventArgs.ErrorData = errorData;
				return customErrorEventArgs;
			}

			protected override void OnRecycle()
			{
				Channel = null;
				ErrorData = null;
			}
		}

		public sealed class ErrorEventArgs : EventArgs<ErrorEventArgs>
		{
			public INetworkChannel Channel { get; private set; }

			public NetworkErrorCode ErrorCode { get; private set; }

			public string Message { get; private set; }

			public static ErrorEventArgs Make(INetworkChannel channel, NetworkErrorCode errorCode, string message)
			{
				ErrorEventArgs errorEventArgs = Mod.Reference.Acquire<ErrorEventArgs>();
				errorEventArgs.Channel = channel;
				errorEventArgs.ErrorCode = errorCode;
				errorEventArgs.Message = message;
				return errorEventArgs;
			}

			protected override void OnRecycle()
			{
				Channel = null;
				Message = null;
			}
		}

		public sealed class MissHeartBeatEventArgs : EventArgs<MissHeartBeatEventArgs>
		{
			public INetworkChannel Channel { get; private set; }

			public int MissCount { get; private set; }

			public static MissHeartBeatEventArgs Make(INetworkChannel channel, int missCount)
			{
				MissHeartBeatEventArgs missHeartBeatEventArgs = Mod.Reference.Acquire<MissHeartBeatEventArgs>();
				missHeartBeatEventArgs.Channel = channel;
				missHeartBeatEventArgs.MissCount = missCount;
				return missHeartBeatEventArgs;
			}

			protected override void OnRecycle()
			{
				Channel = null;
			}
		}

		private readonly Dictionary<string, Channel> _channels = new Dictionary<string, Channel>();

		public int ChannelCount
		{
			get
			{
				return _channels.Count;
			}
		}

		public INetworkChannel[] Channels
		{
			get
			{
				int num = 0;
				INetworkChannel[] array = new INetworkChannel[_channels.Count];
				foreach (KeyValuePair<string, Channel> channel in _channels)
				{
					array[num++] = channel.Value;
				}
				return array;
			}
		}

		public bool ContainsChannel(string name)
		{
			return _channels.ContainsKey(name ?? string.Empty);
		}

		public INetworkChannel GetChannel(string name)
		{
			Channel value;
			if (!_channels.TryGetValue(name ?? string.Empty, out value))
			{
				return null;
			}
			return value;
		}

		public INetworkChannel CreateChannel(string name, INetworkChannelProxy channelProxy)
		{
			if (channelProxy == null)
			{
				Log.Error("Network channel proxy is invalid.");
				return null;
			}
			if (channelProxy.PacketHeaderLength <= 0)
			{
				Log.Warning("Packet header length is invalid.");
				return null;
			}
			if (ContainsChannel(name))
			{
				Log.Warning("Already exist network channel " + (name ?? string.Empty) + ".");
				return null;
			}
			Channel channel = new Channel(name, channelProxy);
			_channels.Add(name, channel);
			return channel;
		}

		public bool DestroyChannel(string name)
		{
			Channel value;
			if (_channels.TryGetValue(name ?? string.Empty, out value))
			{
				value.Destroy();
				return _channels.Remove(name ?? string.Empty);
			}
			return false;
		}

		protected override void Awake()
		{
			Mod.Network = this;
		}

		internal override void OnTick(float elapseSeconds, float realElapseSeconds)
		{
			Profiler.BeginSample("NetworkMod.OnTick");
			foreach (KeyValuePair<string, Channel> channel in _channels)
			{
				channel.Value.Tick(realElapseSeconds);
			}
			Profiler.EndSample();
		}

		internal override void OnExit()
		{
			foreach (KeyValuePair<string, Channel> channel in _channels)
			{
				channel.Value.Destroy();
			}
			_channels.Clear();
		}

		private void OnChannelConnected(INetworkChannel channel, object userData)
		{
			ConnectedEventArgs args = ConnectedEventArgs.Make(channel, userData);
			Mod.Event.Fire(this, args);
		}

		private void OnChannelClosed(INetworkChannel channel)
		{
			ClosedEventArgs args = ClosedEventArgs.Make(channel);
			Mod.Event.Fire(this, args);
		}

		private void OnChannelMissHeartBeat(INetworkChannel channel, int missCount)
		{
			MissHeartBeatEventArgs args = MissHeartBeatEventArgs.Make(channel, missCount);
			Mod.Event.Fire(this, args);
		}

		private void OnChannelError(INetworkChannel channel, NetworkErrorCode errorCode, string message)
		{
			ErrorEventArgs args = ErrorEventArgs.Make(channel, errorCode, message);
			Mod.Event.Fire(this, args);
		}

		private void OnChannelCustomError(INetworkChannel channel, object errorData)
		{
			CustomErrorEventArgs args = CustomErrorEventArgs.Make(channel, errorData);
			Mod.Event.Fire(this, args);
		}
	}
}
