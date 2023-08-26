using Foundation;
using UnityEngine;

namespace RS2
{
	public sealed class RoleLineElment : IElementRebirth
	{
		private static readonly string DefaultColorParam = "_TintColor";

		private Material lineMat;

		private float beginPosZ;

		private float deltaDis;

		private float maxAlpha;

		private bool ifNeedChange;

		public bool IfHaveLine { get; private set; }

		public bool IfRebirthRecord
		{
			get
			{
				return false;
			}
		}

		public void InitLine(Transform line)
		{
			IfHaveLine = line != null;
			if (IfHaveLine)
			{
				MeshRenderer component = line.GetComponent<MeshRenderer>();
				lineMat = component.sharedMaterial;
				SetMatAlpha(0f);
				Mod.Event.Subscribe(EventArgs<TempLineAlphaEventArgs>.EventId, OnAlphaChange);
			}
		}

		public void ResetLine()
		{
			if (IfHaveLine)
			{
				SetMatAlpha(0f);
				ifNeedChange = false;
			}
		}

		public void DestroyLine()
		{
			if (IfHaveLine)
			{
				Mod.Event.Unsubscribe(EventArgs<TempLineAlphaEventArgs>.EventId, OnAlphaChange);
				ifNeedChange = false;
				IfHaveLine = false;
			}
		}

		public void UpdateLine()
		{
			if (IfHaveLine && ifNeedChange)
			{
				float percentByPosZ = GetPercentByPosZ(BaseRole.BallPosition.z);
				float num = 1f - Mathf.Abs(percentByPosZ - 0.5f) * 2f;
				SetMatAlpha(maxAlpha * num);
				if (percentByPosZ >= 1f)
				{
					ifNeedChange = false;
					SetMatAlpha(0f);
				}
			}
		}

		private void OnAlphaChange(object sender, EventArgs e)
		{
			TempLineAlphaEventArgs tempLineAlphaEventArgs = e as TempLineAlphaEventArgs;
			if (tempLineAlphaEventArgs != null)
			{
				deltaDis = tempLineAlphaEventArgs.dis;
				maxAlpha = tempLineAlphaEventArgs.maxAlpha;
				beginPosZ = BaseRole.BallPosition.z;
				ifNeedChange = true;
			}
		}

		public float GetPercentByPosZ(float posZ)
		{
			return (posZ - beginPosZ) / deltaDis;
		}

		private void SetMatAlpha(float a)
		{
			if (IfHaveLine)
			{
				a /= 255f;
				Color color = lineMat.GetColor(DefaultColorParam);
				color.a = a;
				lineMat.SetColor(DefaultColorParam, color);
			}
		}

		public void RebirthReadData(object rd_data)
		{
		}

		public object RebirthWriteData()
		{
			return JsonUtility.ToJson(new RD_RoleLineElment_DATA());
		}

		public void RebirthStartGame(object rd_data)
		{
		}

		public void RebirthReadByteData(byte[] rd_data)
		{
		}

		public byte[] RebirthWriteByteData()
		{
			return Bson.ToBson(new RD_RoleLineElment_DATA());
		}

		public void RebirthStartByteGame(byte[] rd_data)
		{
		}
	}
}
