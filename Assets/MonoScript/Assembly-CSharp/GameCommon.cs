using System.Collections.Generic;
using UnityEngine;

public static class GameCommon
{
	public static readonly string SPRITES_PATH = "Prefab/UI/Textures/";

	public static readonly string ICON_PATH = "Prefab/UI/Textures/Icons/";

	public static readonly int SYSGOODSCHANGE_MIN_NUM = 5;

	public static readonly int SYSLEVELCHANGE_MIN_NUM = 1;

	public static readonly int SYSPROPERTYCHANGE_MIN_NUM = 1;

	public static readonly int SYSMESSIONCHANGE_MIN_NUM = 1;

	public static readonly int SYSINSTRUMENTCHANGE_MIN_NUM = 1;

	public static readonly int INTERSTITIAL_AD_BASENUM = 15;

	public const int POWER_ID = 1;

	public const int REBIRTH_ID = 2;

	public const int GOLD_ID = 3;

	public const int SHIELD_ID = 4;

	public const int KEY_ID = 6;

	public const int BUYOUT_REBIRTH_ITEM_ID = 11;

	public const int POWER_TIME_30 = 9;

	public const int POWER_TIME_120 = 16;

	public static int START_FREE_SHIELD = 12;

	public static int EVERY_DAY_GIVE_POWER = 13;

	public static int ORIGIN_REBIRTH_FREE = 14;

	public static int ORIGIN_REBIRTH_UNLIMITED = 15;

	public static int START_FREE_SHIELD_TIME = 17;

	public static int ORIGIN_REBIRTH_FREE_TIME = 18;

	public static int REMOVE_AD = 19;

	public static int GuideLine = 20;

	public static int GuideLine120Min = 21;

	public static int SHIELD_ITEM = 30;

	public static int REBIRTH_FREE_ITEM = 31;

	public static int TUTORIAL_BUFF_GOODSTEAMID = 4905;

	public static int REPUTATION_ID = 32;

	public static int GUIDE_LINE_PIECE = 33;

	public static int SHIELD_ITEM_PIECE = 34;

	public static int REBIRTH_FREE_ITEM_PIECE = 35;

	public static int REMOVE_AD_TIME = 36;

	public static Color32 green_0 = new Color32(163, byte.MaxValue, 193, byte.MaxValue);

	public static int firstPowerNum = 15;

	public static int firstRebirthNum = 3;

	public static int firstShieldNum = 3;

	public static int powerCdTime = 120;

	public static int powerRecoverNum = 10;

	public static int powerRecoverNum_byAd = 10;

	public static List<int> rebirthConsume = new List<int>();

	public static int levelTargetMaxCanFinNum = 10;

	public static int levelTargetSelectedNum = 3;

	public static int rebirthMaxNum = 15;

	public static int rebirthMaxAdNum = 5;

	public static int levelTargetResetTime = 6;

	public static int firstFinishLevelGetKeysMum = 1;

	public static int levelTargetOpenFirstLevelPercent = 30;

	public static int useAssetOpenTotalFinishLevelNum = 15;

	public static int shopFreeAdResetTime = 6;

	public static int shopFreeAdTodayShowMaxCount = 5;

	public static int shopResetTime = 6;

	public static int getPowerByAdMaxNumEveryDay = 2;

	public static int timeOutFormGiveGoodsId = 8;

	public static int openBuffFunctionLevelTime = 300;

	public static int needHeartRebirthMaxNumber = 4;

	public static int tyrLevelNumPerAd = 1;

	public static int startPowerRecoverThreshold = 5;

	public static int motivateRewardCD = 86400;

	public static int motivateRewardNeedEnergy = 300;

	public static int progressMotivateEnergy = 1;

	public static int diamoundMotivateEnergy = 10;

	public static int crownMotivateEnergy = 35;

	public static int moneyGiftPackageShopID = 1;

	public static int powerGiftPackageShopID = 1;

	public static int powerRecoveryGiftPackageShopID = 1;

	public static int removeAdGiftPackageShopID = 1;

	public static int keyGiftPackageShopID = 1;

	public static int[] removeAdMoneyShopIDs;

	public const int powerBuffer_reward_goodsTeamId = 50002;

	public static float payUserThreshold = 1.99f;

	public static int screenPluginAdProtectedTime = 120;

	public static int screenPluginAdCDTime = 30;

	public static int showFiveStarThreshold = 5;

	public static int screenPlguinStartId = 0;

	public static int showRemoveAdForScreenCount = 3;

	public static int newVersionShowSeriesId = 4;

	public static int[] ignoreFiveStarLevelIds;

	public static int isOpenScreenPlugin = 1;

	public static int showFreeBuffThreshold = 4;

	public static int adFreeLevelEnterTime = 0;

	public static int AdAwardItemCount = 0;

	public static int AdAwardRestartTimes = 0;

	public static int firstGuideNum = 3;

	public static int maxMotivateAwardTimes = 10;

	public static int maxFreeGoodsTimes = 10;

	public static int maxFreeGoodsNoAdTimes = 1;

	public static int originRebirthContinueTime = 5;

	public static int originRebirthContinueShowTime = 5;

	public static float instrumentProductFrequency = 5f;

	public static float instrumentProductSpeeedUpFrequency = 5f;

	public static float instrumenCommonBroadFrequency = 5f;

	public static float instrumenSpeedUpBroadFrequency = 5f;

	public static int offlineProductReputationMaxTime = 3000;

	public static int offlineProductGoldMaxTime = 2000;

	public static int offlineProductMinTime = 60;

	public static int instrumentSpeedUpTime = 180;

	public static int instrumentSpeedUpMaxTime = 1800;

	public static int offlineProductCostDiamound = 300;

	public static int instrumentSpeedUpCostDiamound = 300;

	public static int upPlayerLevelByAdMaxTime = 48;

	public static int instrumentLimitLevel = 0;

	public static int instrumentAdProductFactor = 2;

	public static int networkVerifyLastTime = 5;

	public static int guideLineTime = 20;

	public static float instrumentLevelShowTime = 3f;

	public static float instrumentLevelClickShowTime = 1.5f;

	public static int coolPlayPackageContidtion_X = 1;

	public static int coolPlayPackageContidtion_Y = 3;

	public static int coolPlayPackageContidtion_Z = 8;

	public static int luckyTurnTableSpentGemsNum = 5;

	public static int showAdInInstrument = 600;

	public static int InstrumentUnlockLV = 1;

	public static int MAX_SLOT_NUM = 8;

	public static int PLAYER_MAX_LEVEL = 360;

	public const int TUTORIAL_LEVEL_TARGET = 1;

	public const int TUTORIAL_USING_ASSERTS = 2;

	public const int TUTORIAL_BUFF_SHOW = 3;

	public const int TUTORIAL_UNLIMITEDREBIRTH_SHOW = 4;

	public const int TUTORIAL_LEVEL = 10000;

	public static int FIRST_LEVEL = 131100;

	public static int SECOND_LEVEL = 121101;

	public static int THIRD_LEVEL = 131102;

	public static int FIRST_SERIES = 2;

	public static int SECOND_SERIES = 6;

	public static int THIRD_SERIES = 3;

	public const int TUTORIAL_LEFT_SLIDE = 1;

	public const int TUTORIAL_RIGHT_SLIDE = 2;

	public const int TUTORIAL_OPERATE_SUCCESS = 3;

	public const int TUTORIAL_END = 4;

	public const int TUTORIAL_HIDE_TIP = 5;

	public const int TUTORIAL_LEFT_JUMP = 6;

	public const int TUTORIAL_RIGHT_JUMP = 7;

	public const int TUTORIAL_SHOW_VIDEO = 8;

	public static int FIRST_PACKAGE_GIFT = 4100;

	public static int FULL_BUFF_GIFT = 4101;

	public static int SHIED_BUFF_GIFT_DISOCOUNT = 4104;

	public static int REBIRTH_BUFF_GIFT_DISCOUNT = 4102;

	public static int POWER_BUFF_GIFT_DISCOUNT = 4103;

	public static int SHIED_BUFF_GIFT_NO_DISOCOUNT = 4104;

	public static int REBIRTH_BUFF_GIFT_NO_DISOCOUNT = 4102;

	public static int POWER_BUFF_GIFT_NO_DISOCOUNT = 4103;

	public static int COOLPLAY_PACKAGE = 4501;

	public static Color COMMON_RED = new Color32(231, 68, 18, byte.MaxValue);

	public static Color COMMON_GREY = new Color32(178, 178, 178, byte.MaxValue);

	public static Color COMMON_GREEN = new Color32(180, byte.MaxValue, 201, byte.MaxValue);

	public static float COMMON_AD_REFRESHTIME = 0.5f;
}
