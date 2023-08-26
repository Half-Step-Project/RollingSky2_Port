using System.Collections.Generic;

public class PetManager
{
	public static Dictionary<int, PetData> m_petDatas = new Dictionary<int, PetData>
	{
		{
			1,
			new PetData(1, "PetStar", typeof(PetStar))
		},
		{
			2,
			new PetData(2, "PetEagle", typeof(PetEagle), 100)
		}
	};

	public static PetData GetPetDataById(int id)
	{
		PetData result = null;
		if (m_petDatas.ContainsKey(id))
		{
			result = m_petDatas[id];
		}
		return result;
	}
}
