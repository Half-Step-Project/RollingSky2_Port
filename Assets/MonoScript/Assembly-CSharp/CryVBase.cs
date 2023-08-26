using System;

public class CryVBase
{
	public byte[] m_cryValue;

	public byte[] m_cryKey;

	public byte[] Value
	{
		get
		{
			return XORDecry();
		}
		set
		{
			m_cryKey = new byte[value.Length];
			int num = 0;
			Random random = new Random();
			for (int i = 0; i < m_cryKey.Length; i++)
			{
				num = random.Next() % 255;
				m_cryKey[i] = BitConverter.GetBytes(num)[0];
			}
			XOREncry(value);
		}
	}

	public CryVBase(byte[] value)
	{
		Value = value;
	}

	private void XOREncry(byte[] sourceData)
	{
		m_cryValue = new byte[sourceData.Length];
		int i = 0;
		int num = m_cryKey.Length;
		for (; i < sourceData.Length; i++)
		{
			byte b = sourceData[i];
			byte b2 = m_cryKey[i % num];
			b = (byte)(b ^ b2);
			m_cryValue[i] = b;
		}
	}

	private byte[] XORDecry()
	{
		byte[] array = new byte[m_cryValue.Length];
		int i = 0;
		int num = m_cryKey.Length;
		for (; i < array.Length; i++)
		{
			byte b = m_cryValue[i];
			byte b2 = m_cryKey[i % num];
			b = (array[i] = (byte)(b ^ b2));
		}
		return array;
	}

	public void Reset()
	{
		m_cryValue = null;
		m_cryKey = null;
	}
}
