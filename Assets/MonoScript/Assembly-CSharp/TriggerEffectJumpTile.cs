using System;
using System.IO;
using Foundation;
using UnityEngine;

public class TriggerEffectJumpTile : BaseTile
{
	[Serializable]
	public struct TileData : IReadWriteBytes
	{
		public float BeginDistance;

		public float ResetDistance;

		public float BaseBallSpeed;

		public float JumpDistance;

		public float JumpHeight;

		public float MoveSpeed;

		public Vector3 BeginPos;

		public float DefaultEmission;

		public float TargetEmission;

		public float EmissionScaler;

		public void ReadBytes(byte[] bytes)
		{
			int startIndex = 0;
			BeginDistance = bytes.GetSingle(ref startIndex);
			ResetDistance = bytes.GetSingle(ref startIndex);
			BaseBallSpeed = bytes.GetSingle(ref startIndex);
			JumpDistance = bytes.GetSingle(ref startIndex);
			JumpHeight = bytes.GetSingle(ref startIndex);
			MoveSpeed = bytes.GetSingle(ref startIndex);
			BeginPos = bytes.GetVector3(ref startIndex);
			DefaultEmission = bytes.GetSingle(ref startIndex);
			TargetEmission = bytes.GetSingle(ref startIndex);
			EmissionScaler = bytes.GetSingle(ref startIndex);
		}

		public byte[] WriteBytes()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				int offset = 0;
				memoryStream.WriteByteArray(BeginDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(ResetDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BaseBallSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpDistance.GetBytes(), ref offset);
				memoryStream.WriteByteArray(JumpHeight.GetBytes(), ref offset);
				memoryStream.WriteByteArray(MoveSpeed.GetBytes(), ref offset);
				memoryStream.WriteByteArray(BeginPos.GetBytes(), ref offset);
				memoryStream.WriteByteArray(DefaultEmission.GetBytes(), ref offset);
				memoryStream.WriteByteArray(TargetEmission.GetBytes(), ref offset);
				memoryStream.WriteByteArray(EmissionScaler.GetBytes(), ref offset);
				memoryStream.Flush();
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return memoryStream.ToArray();
			}
		}
	}

	public TileData data;

	[Range(0f, 2f)]
	public float TestPercent;

	public float TestBaseSpeed = 7.25f;

	public float TestTime;

	private Vector3 beginPos;

	private Vector3 endPos;

	private float collidePos;

	private Transform state1;

	private Transform state2;

	private Animation anim;

	private Material jumpMat;

	private bool ifTriggerActive;

	private RD_TriggerEffectJumpTile_DATA m_rebirthData;

	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	public override void SetDefaultValue(object[] objs)
	{
		data.BeginDistance = (float)objs[0];
		data.ResetDistance = (float)objs[1];
		data.BaseBallSpeed = (float)objs[2];
		data.JumpDistance = (float)objs[3];
		data.JumpHeight = (float)objs[4];
		data.MoveSpeed = (float)objs[5];
		data.DefaultEmission = (float)objs[6];
		data.TargetEmission = (float)objs[7];
		data.EmissionScaler = (float)objs[8];
	}

	public override void Initialize()
	{
		base.Initialize();
		commonState = CommonState.None;
		if (state1 == null || state2 == null)
		{
			state1 = base.transform.Find("model/state1");
			state2 = base.transform.Find("model/state2");
		}
		if ((bool)state1)
		{
			jumpMat = state1.GetComponentInChildren<MeshRenderer>().material;
			jumpMat.SetFloat("_Emmission", data.DefaultEmission);
		}
		anim = GetComponentInChildren<Animation>();
		if ((bool)anim && data.BaseBallSpeed > 0f)
		{
			anim.wrapMode = WrapMode.ClampForever;
			float speed = Railway.theRailway.SpeedForward / data.BaseBallSpeed;
			anim["anim01"].speed = speed;
		}
		beginPos = (endPos = StartLocalPos);
		collidePos = 10000f;
		ifTriggerActive = false;
		ChangeStateTo1();
	}

	public override void ResetElement()
	{
		base.ResetElement();
		StopAnim();
		collidePos = 10000f;
		ifTriggerActive = false;
	}

	public override void UpdateElement()
	{
		float num = 0f;
		num = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - base.groupTransform.InverseTransformPoint(base.transform.position).z;
		if (commonState == CommonState.None)
		{
			if (num >= data.BeginDistance)
			{
				OnTriggerPlay();
				commonState = CommonState.Active;
			}
		}
		else if (commonState == CommonState.Active)
		{
			if (ifTriggerActive)
			{
				float distance = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z - collidePos;
				PlayByPercent(GetPercent(distance));
				ChangeEmissionByPercent(GetEmissionPercent(distance));
			}
			if (num >= data.ResetDistance)
			{
				ResetElement();
				commonState = CommonState.End;
			}
		}
		else if (num < data.BeginDistance)
		{
			commonState = CommonState.None;
		}
	}

	public override void OnTriggerPlay()
	{
		if ((bool)anim)
		{
			anim["anim01"].normalizedTime = 0f;
			anim.Play();
		}
	}

	public override float GetPercent(float distance)
	{
		return Mathf.Min(2f, Mathf.Max(0f, distance));
	}

	public override void PlayByPercent(float percent)
	{
		if (percent > 0f && percent <= 1f)
		{
			float num = data.MoveSpeed - data.MoveSpeed * (percent - 1f) * (percent - 1f);
			base.transform.localPosition = new Vector3(endPos.x, endPos.y + num, endPos.z);
		}
		else if (percent > 1f && percent < 2f)
		{
			float num2 = data.MoveSpeed - data.MoveSpeed * (percent - 1f) * (percent - 1f);
			base.transform.localPosition = new Vector3(endPos.x, endPos.y + num2, endPos.z);
		}
		else if (percent <= 0f || percent >= 2f)
		{
			base.transform.localPosition = endPos;
		}
	}

	private float GetEmissionPercent(float distance)
	{
		return Mathf.Min(1f, Mathf.Max(0f, distance * data.EmissionScaler));
	}

	private void ChangeEmissionByPercent(float percent)
	{
		if (percent > 0f && percent < 1f && (bool)jumpMat && data.DefaultEmission >= -1f)
		{
			jumpMat.SetFloat("_Emmission", Mathf.Lerp(data.DefaultEmission, data.TargetEmission, percent));
		}
	}

	public override void TriggerEnter(BaseRole ball)
	{
		if (commonState == CommonState.Active && !ball.IfDie)
		{
			ball.OnTileEnter(this);
			collidePos = base.groupTransform.InverseTransformPoint(BaseRole.BallPosition).z;
			if (ball.IfJumpingDown)
			{
				ball.CallEndJump(base.transform.position.y, true);
				ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.Super);
				ifTriggerActive = true;
				PlaySoundEffect();
			}
			else if (!ball.IfJumping)
			{
				ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.Super);
				ifTriggerActive = true;
				PlaySoundEffect();
			}
			else if (ball.IfDropping)
			{
				ball.CallEndDrop(base.transform.position.y);
				ball.CallBeginJump(base.transform.position, base.transform.position + data.JumpDistance * ball.transform.forward, ball.transform.forward, data.JumpHeight, BaseRole.JumpType.Super);
				ifTriggerActive = true;
				PlaySoundEffect();
			}
		}
	}

	public override void Read(string info)
	{
		data = JsonUtility.FromJson<TileData>(info);
	}

	public override string Write()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return JsonUtility.ToJson(data);
	}

	public override void ReadBytes(byte[] bytes)
	{
		data = StructTranslatorUtility.ToStructure<TileData>(bytes);
	}

	public override byte[] WriteBytes()
	{
		Transform transform = base.transform.Find("triggerPoint");
		data.BeginPos = transform.position;
		data.BeginDistance = base.transform.parent.InverseTransformPoint(transform.position).z - base.transform.parent.InverseTransformPoint(base.transform.position).z;
		return StructTranslatorUtility.ToByteArray(data);
	}

	public void OnDrawGizmos()
	{
		Transform transform = base.transform.Find("triggerPoint");
		Color color = Gizmos.color;
		if ((bool)transform)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(transform.position, new Vector3(1f, 0.1f, 0.1f));
			Gizmos.color = color;
		}
		Vector3 position = base.transform.position;
		Vector3 forward = base.transform.forward;
		float jumpHeight = data.JumpHeight;
		Vector3 vector = base.transform.position;
		int num = Mathf.Max(3, Mathf.CeilToInt(data.JumpDistance * 2.5f));
		float num2 = 0.5f;
		Vector3[] array = new Vector3[num + 1];
		array[0] = vector;
		color = Gizmos.color;
		Gizmos.color = Color.yellow;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector2 = vector + forward * num2;
			float num3 = 0f - Vector3.Dot(position - vector2, forward);
			float y = jumpHeight - 4f * jumpHeight / Mathf.Pow(data.JumpDistance, 2f) * Mathf.Pow(num3 - data.JumpDistance / 2f, 2f);
			array[i + 1] = vector2 + new Vector3(0f, y, 0f);
			Gizmos.DrawLine(array[i], array[i + 1]);
			vector = vector2;
		}
		Gizmos.color = color;
		float num4 = data.JumpDistance * TestPercent;
		float num5 = jumpHeight - 4f * jumpHeight / Mathf.Pow(data.JumpDistance, 2f) * Mathf.Pow(num4 - data.JumpDistance / 2f, 2f);
		Vector3 center = position;
		center.z += num4;
		center.y += num5;
		Gizmos.DrawCube(center, Vector3.one);
		TestTime = num4 / TestBaseSpeed;
		int num6 = Mathf.FloorToInt(TestTime);
		TestTime = (float)num6 + (TestTime - (float)num6) * 0.6f;
	}

	private void StopAnim()
	{
		if ((bool)anim)
		{
			anim.Play();
			anim["anim01"].normalizedTime = 0f;
			anim.Sample();
			anim.Stop();
		}
	}

	private void ChangeStateTo1()
	{
		if ((bool)state1)
		{
			state1.gameObject.SetActive(true);
		}
		if ((bool)state2)
		{
			state2.gameObject.SetActive(false);
		}
	}

	private void ChangeStateTo2()
	{
		if ((bool)state1)
		{
			state1.gameObject.SetActive(false);
		}
		if ((bool)state2)
		{
			state2.gameObject.SetActive(true);
		}
	}

	[Obsolete("this is Obsolete,please  please use RebirthReadBsonData !")]
	public override void RebirthReadData(object rd_data)
	{
		m_rebirthData = JsonUtility.FromJson<RD_TriggerEffectJumpTile_DATA>(rd_data as string);
		collidePos = m_rebirthData.collidePos;
		commonState = m_rebirthData.commonState;
		base.transform.SetTransData(m_rebirthData.trans);
		state1.SetTransData(m_rebirthData.state1);
		state2.SetTransData(m_rebirthData.state2);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		jumpMat.SetFloat("_Emmission", m_rebirthData.jumpMat);
		ifTriggerActive = m_rebirthData.ifTriggerActive;
	}

	[Obsolete("this is Obsolete,please  please use RebirthWriteBsonData !")]
	public override object RebirthWriteData()
	{
		RD_TriggerEffectJumpTile_DATA rD_TriggerEffectJumpTile_DATA = new RD_TriggerEffectJumpTile_DATA();
		rD_TriggerEffectJumpTile_DATA.collidePos = collidePos;
		rD_TriggerEffectJumpTile_DATA.commonState = commonState;
		rD_TriggerEffectJumpTile_DATA.trans = base.transform.GetTransData();
		rD_TriggerEffectJumpTile_DATA.state1 = state1.GetTransData();
		rD_TriggerEffectJumpTile_DATA.state2 = state2.GetTransData();
		rD_TriggerEffectJumpTile_DATA.anim = anim.GetAnimData();
		if (jumpMat != null)
		{
			rD_TriggerEffectJumpTile_DATA.jumpMat = jumpMat.GetFloat("_Emmission");
		}
		rD_TriggerEffectJumpTile_DATA.ifTriggerActive = ifTriggerActive;
		return JsonUtility.ToJson(rD_TriggerEffectJumpTile_DATA);
	}

	[Obsolete("this is Obsolete,please  please use RebirthStartGameBsonData !")]
	public override void RebirthStartGame(object rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		m_rebirthData = null;
	}

	public override void RebirthReadByteData(byte[] rd_data)
	{
		m_rebirthData = Bson.ToObject<RD_TriggerEffectJumpTile_DATA>(rd_data);
		collidePos = m_rebirthData.collidePos;
		commonState = m_rebirthData.commonState;
		base.transform.SetTransData(m_rebirthData.trans);
		state1.SetTransData(m_rebirthData.state1);
		state2.SetTransData(m_rebirthData.state2);
		anim.SetAnimData(m_rebirthData.anim, ProcessState.Pause);
		jumpMat.SetFloat("_Emmission", m_rebirthData.jumpMat);
		ifTriggerActive = m_rebirthData.ifTriggerActive;
	}

	public override byte[] RebirthWriteByteData()
	{
		RD_TriggerEffectJumpTile_DATA rD_TriggerEffectJumpTile_DATA = new RD_TriggerEffectJumpTile_DATA();
		rD_TriggerEffectJumpTile_DATA.collidePos = collidePos;
		rD_TriggerEffectJumpTile_DATA.commonState = commonState;
		rD_TriggerEffectJumpTile_DATA.trans = base.transform.GetTransData();
		rD_TriggerEffectJumpTile_DATA.state1 = state1.GetTransData();
		rD_TriggerEffectJumpTile_DATA.state2 = state2.GetTransData();
		rD_TriggerEffectJumpTile_DATA.anim = anim.GetAnimData();
		if (jumpMat != null)
		{
			rD_TriggerEffectJumpTile_DATA.jumpMat = jumpMat.GetFloat("_Emmission");
		}
		rD_TriggerEffectJumpTile_DATA.ifTriggerActive = ifTriggerActive;
		return Bson.ToBson(rD_TriggerEffectJumpTile_DATA);
	}

	public override void RebirthStartByteGame(byte[] rd_data)
	{
		anim.SetAnimData(m_rebirthData.anim, ProcessState.UnPause);
		m_rebirthData = null;
	}
}
