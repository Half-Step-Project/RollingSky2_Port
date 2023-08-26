using System;

public interface IOriginRebirth
{
	bool IsRecordOriginRebirth { get; }

	[Obsolete("this is Obsolete,please  please use GetOriginRebirthBsonData !")]
	object GetOriginRebirthData(object obj = null);

	[Obsolete("this is Obsolete,please  please use SetOriginRebirthBsonData !")]
	void SetOriginRebirthData(object dataInfo);

	[Obsolete("this is Obsolete,please  please use StartRunByOriginRebirthBsonData !")]
	void StartRunByOriginRebirthData(object dataInfo);

	byte[] GetOriginRebirthBsonData(object obj = null);

	void SetOriginRebirthBsonData(byte[] dataInfo);

	void StartRunByOriginRebirthBsonData(byte[] dataInfo);
}
