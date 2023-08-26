namespace Foundation
{
	public interface IPacketHandler
	{
		int Id { get; }

		void Action(object sender, Packet packet);
	}
}
