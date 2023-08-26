using System;
using System.Collections.Generic;
using Foundation;

namespace RS2
{
	public class ActiveGivePowerController : IActiveTimeController
	{
		private bool m_isFirstEnable;

		public string GetTimeKey()
		{
			return "ACTIVEGIVEPOWERCONTROLLER";
		}

		public void Init(DateTime initTime)
		{
			if (!(PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.EVERY_DAY_GIVE_POWER) > 0.0))
			{
				return;
			}
			string config = EncodeConfig.getConfig(GetTimeKey());
			if (string.IsNullOrEmpty(config) || config.Length == 0)
			{
				DateTime resetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, GameCommon.levelTargetResetTime, 0, 0);
				if (initTime.Hour >= GameCommon.levelTargetResetTime)
				{
					m_isFirstEnable = true;
					resetTime = resetTime.AddDays(1.0);
				}
				SetResetTime(resetTime);
			}
		}

		private void SetResetTime(DateTime time)
		{
			string values = TimeTools.DateTimeToString(time);
			EncodeConfig.setConfig(GetTimeKey(), values);
		}

		private DateTime GetResetTime()
		{
			return TimeTools.StringToDatetime(EncodeConfig.getConfig(GetTimeKey()));
		}

		public bool IsEnable(DateTime nowTime)
		{
			if (PlayerDataModule.Instance.GetPlayGoodsNum(GameCommon.EVERY_DAY_GIVE_POWER) <= 0.0)
			{
				return false;
			}
			DateTime dateTime = TimeTools.StringToDatetime(EncodeConfig.getConfig(GetTimeKey()));
			TimeSpan timeSpan = nowTime - dateTime;
			int num = (int)(nowTime - dateTime).TotalMilliseconds;
			bool flag = false;
			if (num >= 0)
			{
				flag = true;
			}
			if (!flag)
			{
				return m_isFirstEnable;
			}
			return true;
		}

		public void ResetTime()
		{
			DateTime dateTime = GetResetTime();
			if (!m_isFirstEnable)
			{
				int days = (DateTime.Now - dateTime).Days;
				if (days >= 0)
				{
					dateTime = dateTime.AddDays(1 + days);
				}
			}
			m_isFirstEnable = false;
			SetResetTime(dateTime);
		}

		public void Reward()
		{
			int functionNum = Mod.DataTable.Get<Goods_goodsTable>()[GameCommon.EVERY_DAY_GIVE_POWER].FunctionNum;
			Dictionary<int, int>.Enumerator enumerator = MonoSingleton<GameTools>.Instacne.DealGoodsTeamById(50002).GetEnumerator();
			while (enumerator.MoveNext())
			{
				PlayerDataModule.Instance.ChangePlayerGoodsNum(enumerator.Current.Key, enumerator.Current.Value * functionNum, AssertChangeType.BUFFER);
			}
			GetGoodsData getGoodsData = new GetGoodsData();
			getGoodsData.GoodsTeamId = 50002;
			getGoodsData.GoodsTeamNum = functionNum;
			getGoodsData.GoodsTeam = true;
			getGoodsData.ShowGetPath = true;
			Mod.UI.OpenUIForm(UIFormId.GetGoodsForm, getGoodsData);
		}
	}
}
