namespace Foundation
{
	public interface IRecord
	{
		int TheId { get; }

		int GetId();

		int Parse(byte[] bytes, int position);
	}
}
