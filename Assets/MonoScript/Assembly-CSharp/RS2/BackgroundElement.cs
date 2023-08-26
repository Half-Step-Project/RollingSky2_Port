using System;
using System.Collections;
using Foundation;
using UnityEngine;

namespace RS2
{
	public class BackgroundElement : BaseBackgroundElement
	{
		public int CurrentIndex;

		public int AnimCount;

		public Animation animComponent;

		public string[] animNames;

		public override bool IsRecordOriginRebirth
		{
			get
			{
				return true;
			}
		}

		public override bool IfRebirthRecord
		{
			get
			{
				return false;
			}
		}

		private void OnEnable()
		{
			Mod.Event.Subscribe(EventArgs<BackgroundChangeEventArgs>.EventId, OnBackgroundChange);
		}

		private void OnDisable()
		{
			Mod.Event.Unsubscribe(EventArgs<BackgroundChangeEventArgs>.EventId, OnBackgroundChange);
		}

		public override void StartPlayAnim()
		{
			PlusIndex();
		}

		public override void Initialize(Transform parent)
		{
			animComponent = base.gameObject.GetComponent<Animation>();
			if ((bool)animComponent)
			{
				int num = (AnimCount = animComponent.GetClipCount());
				if (num > 0)
				{
					animNames = new string[num];
					int num2 = 0;
					IEnumerator enumerator = animComponent.GetEnumerator();
					while (enumerator.MoveNext())
					{
						AnimationState animationState = enumerator.Current as AnimationState;
						if (animationState != null)
						{
							animNames[num2] = animationState.name;
							num2++;
						}
					}
				}
			}
			CurrentIndex = 0;
			base.transform.parent = parent;
			base.transform.localPosition = Vector3.zero;
			base.transform.localEulerAngles = Vector3.zero;
		}

		public override void ResetElement()
		{
			ResetElementByIndex(0);
			CurrentIndex = 0;
		}

		private void ResetElementByIndex(int index)
		{
			if (animComponent != null && animNames != null)
			{
				animComponent.Play();
				animComponent[animNames[index]].normalizedTime = 0f;
				animComponent.Sample();
				animComponent.Stop();
			}
		}

		private void OnBackgroundChange(object sender, Foundation.EventArgs e)
		{
			if (e is BackgroundChangeEventArgs)
			{
				PlusIndex();
			}
		}

		private void PlusIndex()
		{
			if (AnimCount > 0)
			{
				animComponent[animNames[CurrentIndex]].normalizedTime = 0f;
				animComponent.Play(animNames[CurrentIndex]);
				CurrentIndex++;
				if (CurrentIndex >= AnimCount)
				{
					CurrentIndex = 0;
				}
			}
		}

		public override int GetBackgrondIndex()
		{
			return CurrentIndex;
		}

		public override void ResetBySavePointInfo(RebirthBoxData savePoint)
		{
			CurrentIndex = savePoint.m_backIndex;
			if (animComponent != null && animNames != null)
			{
				animComponent.Play(animNames[CurrentIndex]);
				animComponent[animNames[CurrentIndex]].normalizedTime = 0f;
				animComponent.Sample();
				animComponent.Stop();
			}
		}

		[Obsolete("this is Obsolete,please  please use SetOriginRebirthBsonData !")]
		public override void SetOriginRebirthData(object dataInfo)
		{
			RD_BackgroundElement_DATA rD_BackgroundElement_DATA = JsonUtility.FromJson<RD_BackgroundElement_DATA>(dataInfo as string);
			if (rD_BackgroundElement_DATA != null)
			{
				CurrentIndex = rD_BackgroundElement_DATA.CurrentIndex;
				ResetElementByIndex(CurrentIndex);
				animComponent.SetAnimData(rD_BackgroundElement_DATA.AnimData, ProcessState.Pause);
			}
		}

		[Obsolete("this is Obsolete,please  please use GetOriginRebirthBsonData !")]
		public override object GetOriginRebirthData(object obj = null)
		{
			return JsonUtility.ToJson(new RD_BackgroundElement_DATA
			{
				CurrentIndex = CurrentIndex,
				AnimData = animComponent.GetAnimData(animNames[CurrentIndex])
			});
		}

		[Obsolete("this is Obsolete,please  please use StartRunByOriginRebirthBsonData !")]
		public override void StartRunByOriginRebirthData(object dataInfo)
		{
			RD_BackgroundElement_DATA rD_BackgroundElement_DATA = JsonUtility.FromJson<RD_BackgroundElement_DATA>(dataInfo as string);
			if (rD_BackgroundElement_DATA != null)
			{
				animComponent.SetAnimData(rD_BackgroundElement_DATA.AnimData, ProcessState.Play);
			}
		}

		public override byte[] GetOriginRebirthBsonData(object obj = null)
		{
			return Bson.ToBson(new RD_BackgroundElement_DATA
			{
				CurrentIndex = CurrentIndex,
				AnimData = animComponent.GetAnimData(animNames[CurrentIndex])
			});
		}

		public override void SetOriginRebirthBsonData(byte[] dataInfo)
		{
			RD_BackgroundElement_DATA rD_BackgroundElement_DATA = Bson.ToObject<RD_BackgroundElement_DATA>(dataInfo);
			if (rD_BackgroundElement_DATA != null)
			{
				CurrentIndex = rD_BackgroundElement_DATA.CurrentIndex;
				ResetElementByIndex(CurrentIndex);
				animComponent.SetAnimData(rD_BackgroundElement_DATA.AnimData, ProcessState.Pause);
			}
		}

		public override void StartRunByOriginRebirthBsonData(byte[] dataInfo)
		{
			RD_BackgroundElement_DATA rD_BackgroundElement_DATA = Bson.ToObject<RD_BackgroundElement_DATA>(dataInfo);
			if (rD_BackgroundElement_DATA != null)
			{
				animComponent.SetAnimData(rD_BackgroundElement_DATA.AnimData, ProcessState.Play);
			}
		}
	}
}
