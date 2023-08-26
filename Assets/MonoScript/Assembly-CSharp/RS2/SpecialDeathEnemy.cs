using System;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class SpecialDeathEnemy : BaseEnemy
	{
		[Serializable]
		public struct EnemyData : IReadWriteBytes
		{
			public float DeltaY;

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				DeltaY = bytes.GetSingle(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				return DeltaY.GetBytes();
			}
		}

		public EnemyData data;

		public override bool IfRebirthRecord
		{
			get
			{
				return false;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void TriggerEnter(BaseRole ball)
		{
			if ((bool)ball && !GameController.IfNotDeath)
			{
				if (ball.IfJumpingDown)
				{
					ball.CallEndJump(base.transform.position.y + data.DeltaY);
				}
				if (ball.IfDropping)
				{
					ball.CallEndDrop(base.transform.position.y + data.DeltaY);
				}
				ball.SpecialCrashBall(ball.transform.position);
			}
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			Color color = Gizmos.color;
			Gizmos.color = Color.white;
			Gizmos.DrawSphere(base.transform.position + new Vector3(0f, 0.5f + data.DeltaY, 0f), 0.5f);
			Gizmos.color = color;
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<EnemyData>(info);
		}

		public override string Write()
		{
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToStructure<EnemyData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			return StructTranslatorUtility.ToByteArray(data);
		}
	}
}
