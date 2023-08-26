using System.Collections.Generic;
using Foundation;
using UnityEngine;

public static class CutManager
{
	private static Dictionary<int, IAttack> mAttacks = new Dictionary<int, IAttack>();

	private static Dictionary<int, List<IAttackable>> mAttackables = new Dictionary<int, List<IAttackable>>();

	public static void SubAttacks(int groupID, IAttack attack)
	{
		IAttack value = null;
		mAttacks.TryGetValue(groupID, out value);
		if (value != null)
		{
			Log.Warning("重复SubAttacks{0}", groupID);
		}
		mAttacks[groupID] = attack;
	}

	public static void UnSubAttacks(int groupID)
	{
		mAttacks.Remove(groupID);
	}

	public static IAttack GetAttack(int groupID)
	{
		IAttack value = null;
		mAttacks.TryGetValue(groupID, out value);
		return value;
	}

	public static void SubAttackables(int groupID, IAttackable attackable)
	{
		List<IAttackable> value = null;
		if (!mAttackables.TryGetValue(groupID, out value) || value == null)
		{
			value = new List<IAttackable>();
		}
		if (!value.Contains(attackable))
		{
			value.Add(attackable);
		}
		else
		{
			Log.Warning("重复SubAttackables {0}", groupID);
		}
		mAttackables[groupID] = value;
	}

	public static void UnSubAttackables(int groupID, IAttackable attackable)
	{
		List<IAttackable> value = null;
		if ((mAttackables.TryGetValue(groupID, out value) || value != null) && value.Contains(attackable))
		{
			value.Remove(attackable);
		}
	}

	public static List<IAttackable> GetAttackables(int groupID)
	{
		List<IAttackable> value = null;
		mAttackables.TryGetValue(groupID, out value);
		return value;
	}

	public static IAttackable GetAttackableByDistanceZ(int groupID, Vector3 vector3)
	{
		List<IAttackable> attackables = GetAttackables(groupID);
		if (attackables == null)
		{
			return null;
		}
		float num = 10000f;
		for (int i = 0; i < attackables.Count; i++)
		{
			IAttackable attackable = attackables[i];
			if (attackable != null)
			{
				float z = attackable.GetPosition().z;
				if (z < num)
				{
					num = z;
				}
			}
		}
		return attackables[0];
	}

	public static IAttackable GetAttackableByDistanceZ(int groupID)
	{
		IAttack attack = GetAttack(groupID);
		if (attack == null)
		{
			return null;
		}
		return GetAttackableByDistanceZ(groupID, attack.GetPosition());
	}
}
