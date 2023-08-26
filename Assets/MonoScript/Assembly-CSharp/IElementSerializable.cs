public interface IElementSerializable
{
	void ReadBytes(byte[] bytes);

	byte[] WriteBytes();
}
