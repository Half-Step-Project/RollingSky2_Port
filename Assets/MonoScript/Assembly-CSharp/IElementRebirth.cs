public interface IElementRebirth
{
	bool IfRebirthRecord { get; }

	object RebirthWriteData();

	void RebirthReadData(object rd_data);

	void RebirthStartGame(object rd_data);

	byte[] RebirthWriteByteData();

	void RebirthReadByteData(byte[] rd_data);

	void RebirthStartByteGame(byte[] rd_data);
}
