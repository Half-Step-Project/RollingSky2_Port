using System.Collections.Generic;

public class ProgressManager : Singleton<ProgressManager>
{
	public class Progress
	{
		public readonly int levelId;

		private string playerPrefsProgressName;

		private string playerPrefsGemsName;

		private string playerPrefsCrownsName;

		public int ProgressPercentage { get; private set; }

		public float ProgressPercent { get; private set; }

		public float ProgressPercentLast { get; private set; }

		public int ProgressPercentageLast { get; private set; }

		public int ProgressPercentageUpToLast { get; private set; }

		public float ProgressPercentUpToLast { get; private set; }

		public int ProgressPercentageThisSession { get; private set; }

		public float ProgressPercentThisSession { get; private set; }

		public bool IsComplete { get; private set; }

		public bool RecordWasJustBroken { get; private set; }

		public bool WasJustCompleted { get; private set; }

		public bool WasJustCompletedForFirstTime { get; private set; }

		public bool WasJustReCompleted { get; private set; }

		public int Gems { get; private set; }

		public int GemsLast { get; private set; }

		public int Crowns { get; private set; }

		public int CrownsLast { get; private set; }

		public Progress(int levelNumber)
		{
			levelId = levelNumber;
			string text = levelId.ToString();
			playerPrefsProgressName = "P" + text;
			playerPrefsGemsName = "G" + text;
			playerPrefsCrownsName = "C" + text;
			ProgressPercentage = EncodeConfig.getInt(playerPrefsProgressName);
			ProgressPercent = MathUtils.FromPercentageInt(ProgressPercentage);
			ProgressPercentageLast = ProgressPercentage;
			ProgressPercentLast = ProgressPercent;
			ProgressPercentageUpToLast = ProgressPercentage;
			ProgressPercentUpToLast = ProgressPercent;
			ProgressPercentageThisSession = 0;
			ProgressPercentThisSession = 0f;
			Gems = EncodeConfig.getInt(playerPrefsGemsName);
			GemsLast = Gems;
			Crowns = EncodeConfig.getInt(playerPrefsCrownsName);
			CrownsLast = Crowns;
			IsComplete = ProgressPercentage >= 100;
			RecordWasJustBroken = false;
			WasJustCompleted = false;
			WasJustCompletedForFirstTime = false;
			WasJustReCompleted = false;
		}

		public bool Set(int newProgress, int gemsCollected, int crownsCollected)
		{
			ProgressPercentageUpToLast = ProgressPercentage;
			ProgressPercentUpToLast = ProgressPercent;
			ProgressPercentageLast = newProgress;
			ProgressPercentLast = MathUtils.FromPercentageInt(newProgress);
			bool flag = newProgress > ProgressPercentage;
			bool flag2 = false;
			if (flag)
			{
				ProgressPercentage = newProgress;
				ProgressPercent = MathUtils.FromPercentageInt(ProgressPercentage);
				EncodeConfig.setInt(playerPrefsProgressName, ProgressPercentage);
				if (!IsComplete && ProgressPercentage >= 100)
				{
					IsComplete = true;
					flag2 = true;
				}
			}
			RecordWasJustBroken = flag;
			WasJustCompletedForFirstTime = flag2;
			WasJustCompleted = newProgress >= 100;
			WasJustReCompleted = !flag2 && WasJustCompleted;
			if (newProgress > ProgressPercentageThisSession)
			{
				ProgressPercentageThisSession = newProgress;
				ProgressPercentThisSession = MathUtils.FromPercentageInt(newProgress);
			}
			GemsLast = gemsCollected;
			if (gemsCollected > Gems)
			{
				Gems = gemsCollected;
				EncodeConfig.setInt(playerPrefsGemsName, Gems);
			}
			CrownsLast = crownsCollected;
			if (crownsCollected > Crowns)
			{
				Crowns = crownsCollected;
				EncodeConfig.setInt(playerPrefsCrownsName, Crowns);
			}
			return flag;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}%", levelId, ProgressPercentage);
		}
	}

	private int currentLevelId;

	private const string progressPlayerPrefsSuffix = "P";

	private const string diamondsPlayerPrefsSuffix = "G";

	private const string crownsPlayerPrefsSuffix = "C";

	private Dictionary<int, Progress> progress;

	public int CurrentLevelId
	{
		get
		{
			return currentLevelId;
		}
		set
		{
			currentLevelId = value;
		}
	}

	public void SetProgressFor(int levelId, int scorePercentage, int diamondsCollected, int crownsCollected)
	{
		TryInitialize(levelId);
		progress[levelId].Set(scorePercentage, diamondsCollected, crownsCollected);
	}

	public Dictionary<int, Progress> GetProgress()
	{
		return progress;
	}

	public void SetProgress(Dictionary<int, Progress> progress)
	{
		this.progress = progress;
	}

	public int GetLastDiamondNum(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].GemsLast;
	}

	public int GetMaxDiamondNum(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].Gems;
	}

	public int GetLastCrownNum(int levelID)
	{
		TryInitialize(levelID);
		return progress[levelID].CrownsLast;
	}

	public int GetMaxCrowndNum(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].Crowns;
	}

	public int GetHighestProgress(int levelId)
	{
		TryInitialize(levelId);
		int num = progress[levelId].ProgressPercentage;
		if (num > 100)
		{
			num = 100;
		}
		return num;
	}

	public float GetHighestProgressFloat(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].ProgressPercent;
	}

	public int GetLastProgress(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].ProgressPercentageLast;
	}

	public float GetLastProgressFloat(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].ProgressPercentLast;
	}

	public int GetProgressUpToLast(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].ProgressPercentageUpToLast;
	}

	public float GetProgressFloatUpToLast(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].ProgressPercentUpToLast;
	}

	public int GetProgressThisSession(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].ProgressPercentageThisSession;
	}

	public float GetProgressFloatThisSession(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].ProgressPercentThisSession;
	}

	public int GetHighestProgress()
	{
		return GetHighestProgress(CurrentLevelId);
	}

	public float GetHighestProgressFloat()
	{
		return GetHighestProgressFloat(CurrentLevelId);
	}

	public int GetLastProgress()
	{
		return GetLastProgress(CurrentLevelId);
	}

	public float GetLastProgressFloat()
	{
		return GetLastProgressFloat(CurrentLevelId);
	}

	public int GetProgressUpToLast()
	{
		return GetProgressUpToLast(CurrentLevelId);
	}

	public float GetProgressFloatUpToLast()
	{
		return GetProgressFloatUpToLast(CurrentLevelId);
	}

	public int GetProgressThisSession()
	{
		return GetProgressThisSession(CurrentLevelId);
	}

	public float GetProgressFloatThisSession()
	{
		return GetProgressFloatThisSession(CurrentLevelId);
	}

	public bool IsComplete(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].IsComplete;
	}

	public bool WasJustCompleted(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].WasJustCompleted;
	}

	public bool WasJustCompletedForFirstTime(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].WasJustCompletedForFirstTime;
	}

	public bool WasJustReCompleted(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].WasJustReCompleted;
	}

	public bool IsComplete()
	{
		return IsComplete(CurrentLevelId);
	}

	public bool WasJustCompleted()
	{
		return WasJustCompleted(CurrentLevelId);
	}

	public bool WasJustCompletedForFirstTime()
	{
		return WasJustCompletedForFirstTime(CurrentLevelId);
	}

	public bool WasJustReCompleted()
	{
		return WasJustReCompleted(CurrentLevelId);
	}

	public bool HighscoreWasJustBeaten(int levelId)
	{
		TryInitialize(levelId);
		return progress[levelId].RecordWasJustBroken;
	}

	public bool HighscoreWasJustBeaten(int levelId, out bool wasJustCompleted)
	{
		TryInitialize(levelId);
		wasJustCompleted = progress[levelId].WasJustCompletedForFirstTime;
		return progress[levelId].RecordWasJustBroken;
	}

	public bool HighscoreWasJustBeaten(int levelId, out bool wasJustCompleted, out bool wasJustReCompleted)
	{
		TryInitialize(levelId);
		wasJustCompleted = progress[levelId].WasJustCompletedForFirstTime;
		wasJustReCompleted = progress[levelId].WasJustReCompleted;
		return progress[levelId].RecordWasJustBroken;
	}

	public bool HighscoreWasJustBeaten()
	{
		return HighscoreWasJustBeaten(CurrentLevelId);
	}

	public bool HighscoreWasJustBeaten(out bool wasJustCompleted)
	{
		return HighscoreWasJustBeaten(CurrentLevelId, out wasJustCompleted);
	}

	public bool HighscoreWasJustBeaten(out bool wasJustCompleted, out bool wasJustReCompleted)
	{
		return HighscoreWasJustBeaten(CurrentLevelId, out wasJustCompleted, out wasJustReCompleted);
	}

	private void TryInitialize(int leveleId)
	{
		if (progress == null)
		{
			progress = new Dictionary<int, Progress>();
		}
		if (!progress.ContainsKey(leveleId))
		{
			progress.Add(leveleId, new Progress(leveleId));
		}
	}
}
