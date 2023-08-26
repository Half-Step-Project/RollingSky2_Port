using System;
using System.IO;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class AlpacaEnemy : BaseEnemy
	{
		[Serializable]
		public class EnemyData : IReadWriteBytes
		{
			public bool IfAutoPlay;

			public float MoveSpeed = 5f;

			public Vector3[] PathPoints = new Vector3[0];

			[HideInInspector]
			public Vector3[] BezierPoints = new Vector3[0];

			public void ReadBytes(byte[] bytes)
			{
				int startIndex = 0;
				IfAutoPlay = bytes.GetBoolean(ref startIndex);
				MoveSpeed = bytes.GetSingle(ref startIndex);
				PathPoints = bytes.GetVector3Array(ref startIndex);
				BezierPoints = bytes.GetVector3Array(ref startIndex);
			}

			public byte[] WriteBytes()
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					int offset = 0;
					memoryStream.WriteByteArray(IfAutoPlay.GetBytes(), ref offset);
					memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
					memoryStream.WriteByteArray(PathPoints.GetBytes(), ref offset);
					memoryStream.WriteByteArray(BezierPoints.GetBytes(), ref offset);
					memoryStream.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					return memoryStream.ToArray();
				}
			}
		}

		public readonly int WaitAnimHash = Animator.StringToHash("Wait_0");

		public readonly int RunAnimHash = Animator.StringToHash("Run");

		public readonly int WaitToRunTrigger = Animator.StringToHash("StartRun");

		private Animator animController;

		public EnemyData data = new EnemyData();

		private BezierMover bezierMover;

		[Range(0f, 1f)]
		public float DebugPercent;

		private float currentPercent = -1f;

		private Vector3 debugPosition = Vector3.zero;

		private Vector3 debugPositionZ = Vector3.zero;

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<GameStartEventArgs>.EventId, OnGameStart);
		}

		public override void Initialize()
		{
			base.Initialize();
			commonState = CommonState.None;
			animController = base.gameObject.GetComponentInChildren<Animator>();
			if (data.IfAutoPlay)
			{
				animController.Play(WaitAnimHash);
				commonState = CommonState.Begin;
			}
			bezierMover = new BezierMover(data.BezierPoints);
		}

		public override void ResetElement()
		{
			base.ResetElement();
			animController.ResetTrigger(WaitToRunTrigger);
			bezierMover = null;
		}

		public override void UpdateElement()
		{
			if (commonState == CommonState.Active)
			{
				Vector3 targetPos = base.transform.position;
				Vector3 moveDir = Vector3.forward;
				if (bezierMover.MoveForwardByDis(data.MoveSpeed * Time.deltaTime, base.transform.position, ref targetPos, ref moveDir))
				{
					commonState = CommonState.End;
				}
				base.transform.position = targetPos;
				base.transform.forward = moveDir;
			}
			else
			{
				CommonState commonState2 = commonState;
				int num = 5;
			}
		}

		public override void Read(string info)
		{
			data = JsonUtility.FromJson<EnemyData>(info);
		}

		public override string Write()
		{
			Vector3[] worldPointsByLocal = BaseElement.GetWorldPointsByLocal(data.PathPoints, base.transform);
			data.BezierPoints = Bezier.GetPathByPositions(worldPointsByLocal, 15);
			return JsonUtility.ToJson(data);
		}

		public override void ReadBytes(byte[] bytes)
		{
			data = StructTranslatorUtility.ToClass<EnemyData>(bytes);
		}

		public override byte[] WriteBytes()
		{
			Vector3[] worldPointsByLocal = BaseElement.GetWorldPointsByLocal(data.PathPoints, base.transform);
			data.BezierPoints = Bezier.GetPathByPositions(worldPointsByLocal, 15);
			return StructTranslatorUtility.ToByteArray(data);
		}

		public override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (data.PathPoints != null && data.PathPoints.Length > 2)
			{
				BaseElement.DrawWorldPath(Bezier.GetPathByPositions(BaseElement.GetWorldPointsByLocal(data.PathPoints, base.transform)), Color.red);
				DrawDebug();
			}
		}

		private void DrawDebug()
		{
			if (DebugPercent != currentPercent)
			{
				currentPercent = DebugPercent;
				Vector3[] pathByPositions = Bezier.GetPathByPositions(BaseElement.GetWorldPointsByLocal(data.PathPoints, base.transform), 15);
				BezierMover obj = new BezierMover(pathByPositions);
				float totalDistance = obj.GetTotalDistance();
				float num = currentPercent * totalDistance;
				Vector3 moveDir = Vector3.forward;
				obj.MoveForwardByDis(num, pathByPositions[0], ref debugPosition, ref moveDir);
				float totalDistanceZ = obj.GetTotalDistanceZ();
				if (totalDistanceZ > 0f)
				{
					debugPositionZ = Vector3.Lerp(pathByPositions[0], pathByPositions[pathByPositions.Length - 1], num / totalDistanceZ);
				}
			}
			Gizmos.color = Color.green;
			Gizmos.DrawCube(debugPosition, new Vector3(0.2f, 0.2f, 0.5f));
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(debugPositionZ, 0.25f);
		}

		private void OnGameStart(object sender, Foundation.EventArgs e)
		{
			GameStartEventArgs gameStartEventArgs = e as GameStartEventArgs;
			if (gameStartEventArgs != null && gameStartEventArgs.StartType == GameStartEventArgs.GameStartType.ForceRun && (commonState == CommonState.None || commonState == CommonState.Begin))
			{
				animController.Play(RunAnimHash);
				animController.SetTrigger(WaitToRunTrigger);
				commonState = CommonState.Active;
			}
		}
	}
}
