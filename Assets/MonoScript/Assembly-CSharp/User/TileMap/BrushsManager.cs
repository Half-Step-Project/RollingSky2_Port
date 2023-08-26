using System.Collections.Generic;
using RS2;

namespace User.TileMap
{
	public class BrushsManager
	{
		private static Dictionary<int, BrushData> m_groupBrushs = new Dictionary<int, BrushData>
		{
			{
				1,
				new BrushData(1, "Base/Levels/Home_Rainbow/Brushes/Group/FollowRotatePlatform", typeof(FollowRotatePlatform))
			},
			{
				2,
				new BrushData(2, "Base/Levels/Home_Rainbow/Brushes/Group/YiJi_01_FollowRotatePlatform", typeof(FollowRotatePlatform))
			},
			{
				3,
				new BrushData(3, "Base/Levels/Home_Rainbow/Brushes/Group/YiJi_02_FollowRotatePlatform", typeof(FollowRotatePlatform))
			},
			{
				4,
				new BrushData(4, "Base/Levels/Home_Rainbow/Brushes/Group/YiJi_03_FollowRotatePlatform", typeof(FollowRotatePlatform))
			},
			{
				5,
				new BrushData(5, "Base/Levels/Home_Rainbow/Brushes/Group/YiJi_04_FollowRotatePlatform", typeof(FollowRotatePlatform))
			}
		};

		private static Dictionary<int, BrushData> m_tileBrushs = new Dictionary<int, BrushData>
		{
			{
				1,
				new BrushData(1, "Base/Levels/Level1/Brushes/Tile/Tile_LV1_End01_True", typeof(HangingWinTile))
			},
			{
				2,
				new BrushData(2, "Brush/Templet/Tile_DepartFromShip", typeof(DepartVehicleTile))
			},
			{
				3,
				new BrushData(3, "Brush/Templet/Tile_Templete_JumpShip", typeof(JumpShipTile))
			},
			{
				4,
				new BrushData(4, "Base/Levels/Level1/Brushes/Tile/Tile_P1_Jump_small", typeof(NormalJumpTile))
			},
			{
				5,
				new BrushData(5, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Move_B", typeof(TrapRootTile))
			},
			{
				6,
				new BrushData(6, "Base/Levels/Level1/Brushes/Tile/Tile_P1_Move_F", typeof(TrapRootTile))
			},
			{
				7,
				new BrushData(7, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Move_L", typeof(TrapRootTile))
			},
			{
				8,
				new BrushData(8, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Move_R", typeof(TrapRootTile))
			},
			{
				9,
				new BrushData(9, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Road01", typeof(NormalTile))
			},
			{
				10,
				new BrushData(10, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Road02", typeof(NormalTile))
			},
			{
				11,
				new BrushData(11, "Base/Levels/Level1/Brushes/Tile/Tile_P1_Road03", typeof(NormalTile))
			},
			{
				12,
				new BrushData(12, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Road04", typeof(NormalTile))
			},
			{
				13,
				new BrushData(13, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Road05", typeof(NormalTile))
			},
			{
				14,
				new BrushData(14, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Jump_small", typeof(NormalJumpTile))
			},
			{
				15,
				new BrushData(15, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Move_B", typeof(TrapRootTile))
			},
			{
				16,
				new BrushData(16, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Move_F", typeof(TrapRootTile))
			},
			{
				17,
				new BrushData(17, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Move_L", typeof(TrapRootTile))
			},
			{
				18,
				new BrushData(18, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Move_R", typeof(TrapRootTile))
			},
			{
				19,
				new BrushData(19, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Road01", typeof(NormalTile))
			},
			{
				20,
				new BrushData(20, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Road02", typeof(NormalTile))
			},
			{
				21,
				new BrushData(21, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Road03", typeof(NormalTile))
			},
			{
				22,
				new BrushData(22, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Road04", typeof(NormalTile))
			},
			{
				23,
				new BrushData(23, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Road05", typeof(NormalTile))
			},
			{
				24,
				new BrushData(24, "Base/Levels/Level1/Brushes/Tile/Tile_P1_MoveFollow", typeof(TrapChildTile))
			},
			{
				25,
				new BrushData(25, "Base/Levels/Level2/Brushes/Tile/Tile_P2_MoveFollow", typeof(TrapChildTile))
			},
			{
				26,
				new BrushData(26, "Base/Levels/Level2/Brushes/Tile/Tile_P1_MoveAuTo_B", typeof(TrapTriggerTile))
			},
			{
				27,
				new BrushData(27, "Base/Levels/Level2/Brushes/Tile/Tile_P1_MoveAuTo_F", typeof(TrapTriggerTile))
			},
			{
				28,
				new BrushData(28, "Base/Levels/Level2/Brushes/Tile/Tile_P1_MoveAuTo_L", typeof(TrapTriggerTile))
			},
			{
				29,
				new BrushData(29, "Base/Levels/Level2/Brushes/Tile/Tile_P1_MoveAuTo_R", typeof(TrapTriggerTile))
			},
			{
				30,
				new BrushData(30, "Base/Levels/Level2/Brushes/Tile/Tile_P2_MoveAuTo_B", typeof(TrapTriggerTile))
			},
			{
				31,
				new BrushData(31, "Base/Levels/Level2/Brushes/Tile/Tile_P2_MoveAuTo_F", typeof(TrapTriggerTile))
			},
			{
				32,
				new BrushData(32, "Base/Levels/Level2/Brushes/Tile/Tile_P2_MoveAuTo_L", typeof(TrapTriggerTile))
			},
			{
				33,
				new BrushData(33, "Base/Levels/Level2/Brushes/Tile/Tile_P2_MoveAuTo_R", typeof(TrapTriggerTile))
			},
			{
				34,
				new BrushData(34, "Base/Levels/Level1/Brushes/Tile/Tile_P0_Road01_Water", typeof(NormalTile))
			},
			{
				35,
				new BrushData(35, "Base/Levels/Tutorial/Brushes/Tile/Tutorial_Tile_road01", typeof(MultiSegmentAnimationTile))
			},
			{
				37,
				new BrushData(37, "Brush/AiJi/Tile/Tile_P0_QTE_End", typeof(EndQTEJumpTile))
			},
			{
				38,
				new BrushData(38, "Base/Levels/Level2/Brushes/Tile/Tile_P0_Road01_WaterS", typeof(NormalWideTile))
			},
			{
				39,
				new BrushData(39, "Brush/AiJi/Tile/Tile_P0_JumpShip", typeof(JumpShipTile))
			},
			{
				41,
				new BrushData(41, "Base/Levels/Tutorial/Brushes/Tile/Tutorial_Tile_Piano_L", typeof(FreeCollideAnimTile))
			},
			{
				42,
				new BrushData(42, "Base/Levels/Tutorial/Brushes/Tile/Tutorial_Tile_Piano_R", typeof(FreeCollideAnimTile))
			},
			{
				43,
				new BrushData(43, "Brush/AiJi/Tile/Tile_P0_SuperJump", typeof(JumpDistanceTile))
			},
			{
				45,
				new BrushData(45, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Road02_UP", typeof(MoveAllDirTile))
			},
			{
				46,
				new BrushData(46, "Base/Levels/Level1/Brushes/Tile/Tile_P0_SuperJumpQTE", typeof(JumpDistanceQTETile))
			},
			{
				47,
				new BrushData(47, "Base/Levels/Level2/Brushes/Tile/Tile_P0_MagicCube_3x3", typeof(MagicCubeTile3x3))
			},
			{
				48,
				new BrushData(48, "Base/Levels/Level2/Brushes/Tile/Tile_P0_MagicCube_5x5", typeof(MagicCubeTile5x5))
			},
			{
				49,
				new BrushData(49, "Brush/AiJi/Tile/Tile_P1_HorizonMove_L", typeof(HorizonMoveTile))
			},
			{
				50,
				new BrushData(50, "Brush/AiJi/Tile/Tile_P1_HorizonMove_R", typeof(HorizonMoveTile))
			},
			{
				51,
				new BrushData(51, "Base/Levels/Level1/Brushes/Tile/Tile_P0_TwoStepsJump_Start", typeof(JumpDistanceQTETile))
			},
			{
				52,
				new BrushData(52, "Base/Levels/Level2/Brushes/Tile/Tile_P0_TwoStepsJump_Floating", typeof(JumpDistanceQTETile))
			},
			{
				53,
				new BrushData(53, "Base/Levels/Level1/Brushes/Tile/Tile_Eagle_SuperJump", typeof(JumpDistanceQTETile))
			},
			{
				54,
				new BrushData(54, "Brush/AiJi/Tile/AutoMoveJumpTile", typeof(AutoMoveJumpTile))
			},
			{
				55,
				new BrushData(55, "Base/Levels/Level1/Brushes/Tile/FreeMoveTile", typeof(FreeMoveTile))
			},
			{
				56,
				new BrushData(56, "Brush/AiJi/Tile/FreeCollideTile3x4", typeof(FreeCollideTile))
			},
			{
				57,
				new BrushData(57, "Brush/AiJi/Tile/StarwayTile", typeof(AnimFreeCollideTile))
			},
			{
				58,
				new BrushData(58, "Base/Levels/Level2/Brushes/Tile/Tile_P2_Road02_UP", typeof(MoveAllDirTile))
			},
			{
				59,
				new BrushData(59, "Brush/XiaoWangZi/1_Tile/Tile_XiaoZhen_ShiBanJump01", typeof(RockJumpTile))
			},
			{
				60,
				new BrushData(60, "Base/Levels/Level1/Brushes/Tile/Tile_Elevator01", typeof(ElevatorTile))
			},
			{
				61,
				new BrushData(61, "Base/Levels/Level1/Brushes/Tile/Tile_XiongKong_JumpStar", typeof(SuperStarJumpTile))
			},
			{
				62,
				new BrushData(62, "Base/Levels/Level1/Brushes/Tile/Tile_XiaoZhen_DiKuai", typeof(NormalTile))
			},
			{
				63,
				new BrushData(63, "Base/Levels/Level1/Brushes/Tile/Tile_WuDing_DiKuai", typeof(NormalTile))
			},
			{
				64,
				new BrushData(64, "Base/Levels/Level1/Brushes/Tile/Tile_Road01_WaterS", typeof(NormalWideTile))
			},
			{
				65,
				new BrushData(65, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Jump_small_lv2", typeof(NormalJumpTile))
			},
			{
				66,
				new BrushData(66, "Base/Levels/Level2/Brushes/Tile/Tile_P0_TwoStepsJump_Start_lv2", typeof(JumpDistanceQTETile))
			},
			{
				67,
				new BrushData(67, "Base/Levels/Level2/Brushes/Tile/Tile_P0_SuperJumpQTE_lv2", typeof(JumpDistanceQTETile))
			},
			{
				68,
				new BrushData(68, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Move_F_lv2", typeof(TrapRootTile))
			},
			{
				69,
				new BrushData(69, "Base/Levels/Level2/Brushes/Tile/Tile_P1_Road03_lv2", typeof(NormalTile))
			},
			{
				70,
				new BrushData(70, "Base/Levels/Level2/Brushes/Tile/Tile_P1_MoveFollow_lv2", typeof(TrapChildTile))
			},
			{
				71,
				new BrushData(71, "Base/Levels/Level2/Brushes/Tile/Tile_Eagle_SuperJump_lv2", typeof(JumpDistanceQTETile))
			},
			{
				72,
				new BrushData(72, "Base/Levels/Level2/Brushes/Tile/Tile_LV1_End01_True_lv2", typeof(HangingWinTile))
			},
			{
				73,
				new BrushData(73, "Base/Levels/Level2/Brushes/Tile/FreeMoveTile_lv2", typeof(FreeMoveTile))
			},
			{
				300,
				new BrushData(300, "Base/Levels/Level1/Brushes/Tile/Tile_P1_Jump_small_Water", typeof(NormalJumpTile))
			},
			{
				301,
				new BrushData(301, "Base/Levels/Level1/Brushes/Tile/Tile_XiaoZhen_Move", typeof(MoveAllDirTile))
			},
			{
				302,
				new BrushData(302, "Base/Levels/Level1/Brushes/Tile/Tile_Road_Shining", typeof(EmissionTile))
			},
			{
				303,
				new BrushData(303, "Base/Levels/Level1/Brushes/Tile/Tile_TirggerEffectJump", typeof(TriggerEffectJumpTile))
			},
			{
				304,
				new BrushData(304, "Base/Levels/Level1/Brushes/Tile/Midground_denghai_UP", typeof(FreeMoveTile))
			},
			{
				305,
				new BrushData(305, "Base/Levels/Level1/Brushes/Tile/Tile_XiaoZhen_Move_Water", typeof(MoveAllDirTile))
			},
			{
				306,
				new BrushData(306, "Base/Levels/Level1/Brushes/Tile/Tile_TirggerEffectJump01", typeof(TriggerEffectJumpTile))
			},
			{
				307,
				new BrushData(307, "Base/Levels/Level3/Brushes/Tile/Tile_GlassRootTile", typeof(GlassRootTile))
			},
			{
				308,
				new BrushData(308, "Base/Levels/Level3/Brushes/Tile/Tile_GlassChildTile", typeof(GlassChildTile))
			},
			{
				309,
				new BrushData(309, "Base/Levels/Level3/Brushes/Tile/Tile_Grassland_3", typeof(AnimTile))
			},
			{
				310,
				new BrushData(310, "Base/Levels/Level3/Brushes/Tile/Tile_Grassland_4", typeof(AnimTile))
			},
			{
				311,
				new BrushData(311, "Base/Levels/Level3/Brushes/Tile/Tile_Grassland_5", typeof(AnimTile))
			},
			{
				312,
				new BrushData(312, "Base/Levels/Level3/Brushes/Tile/BaiRiMeng_Road01", typeof(NormalTile))
			},
			{
				313,
				new BrushData(313, "Base/Levels/Level3/Brushes/Tile/BaiRiMeng_Road01_Transparent", typeof(NormalTile))
			},
			{
				314,
				new BrushData(314, "Base/Levels/Level3/Brushes/Tile/Tile_GlassRootTile_None", typeof(GlassRootTile))
			},
			{
				315,
				new BrushData(315, "Base/Levels/Level3/Brushes/Tile/BaiRiMeng_Road01_AN_Transparent", typeof(AnimTile))
			},
			{
				316,
				new BrushData(316, "Base/Levels/Level3/Brushes/Tile/Tile_YunKuai_Scale", typeof(AnimTile))
			},
			{
				318,
				new BrushData(318, "Base/Levels/Level3/Brushes/Tile/Home_Jump_LRMove", typeof(JumpDistanceCycleLRTile))
			},
			{
				319,
				new BrushData(319, "Base/Levels/Level3/Brushes/Tile/Tile_NormalTileAsFragement", typeof(NormalTileAsFragement))
			},
			{
				320,
				new BrushData(320, "Base/Levels/Level3/Brushes/Tile/Home_Road01_Up", typeof(AnimTile))
			},
			{
				321,
				new BrushData(321, "Base/Levels/Level3/Brushes/Tile/Home_Road02_Up", typeof(AnimTile))
			},
			{
				322,
				new BrushData(322, "Base/Levels/Level3/Brushes/Tile/Home_Road03_Up", typeof(AnimTile))
			},
			{
				323,
				new BrushData(323, "Base/Levels/Level3/Brushes/Tile/Home_Road01_Down", typeof(AnimTile))
			},
			{
				324,
				new BrushData(324, "Base/Levels/Level3/Brushes/Tile/Home_Road02_Down", typeof(AnimTile))
			},
			{
				325,
				new BrushData(325, "Base/Levels/Level3/Brushes/Tile/Home_Road03_Down", typeof(AnimTile))
			},
			{
				326,
				new BrushData(326, "Base/Levels/Level3/Brushes/Tile/Home_Road01", typeof(NormalTile))
			},
			{
				327,
				new BrushData(327, "Base/Levels/Level3/Brushes/Tile/Home_Road02", typeof(NormalTile))
			},
			{
				328,
				new BrushData(328, "Base/Levels/Level3/Brushes/Tile/Home_Road03", typeof(NormalTile))
			},
			{
				329,
				new BrushData(329, "Base/Levels/Level3/Brushes/Tile/Home_Road01", typeof(HorizonMoveTile))
			},
			{
				330,
				new BrushData(330, "Base/Levels/Level3/Brushes/Tile/Home_Road02", typeof(HorizonMoveTile))
			},
			{
				331,
				new BrushData(331, "Base/Levels/Level3/Brushes/Tile/Home_Road03", typeof(HorizonMoveTile))
			},
			{
				332,
				new BrushData(332, "Base/Levels/Level3/Brushes/Tile/Home_pingtai_02", typeof(HorizonMoveTile))
			},
			{
				333,
				new BrushData(333, "Base/Levels/Level3/Brushes/Tile/FreeMoveTile_Home01", typeof(FreeMoveTile))
			},
			{
				334,
				new BrushData(334, "Base/Levels/Level3/Brushes/Tile/FreeMoveTile_Home02", typeof(FreeMoveTile))
			},
			{
				335,
				new BrushData(335, "Base/Levels/Level3/Brushes/Tile/FreeMoveTile_Home03", typeof(FreeMoveTile))
			},
			{
				336,
				new BrushData(336, "Base/Levels/Level3/Brushes/Tile/Home_Road01_Move", typeof(MoveAllDirTile))
			},
			{
				337,
				new BrushData(337, "Base/Levels/Level3/Brushes/Tile/Home_Road02_Move", typeof(MoveAllDirTile))
			},
			{
				338,
				new BrushData(338, "Base/Levels/Level3/Brushes/Tile/Home_Road03_Move", typeof(MoveAllDirTile))
			},
			{
				339,
				new BrushData(339, "Base/Levels/Level3/Brushes/Tile/AutoSpeedHorizonMoveTile", typeof(AutoSpeedHorizonMoveTile))
			},
			{
				340,
				new BrushData(340, "Base/Levels/Level3/Brushes/Tile/Home_AutoSpeedHorizonMoveTile", typeof(AutoSpeedHorizonMoveTile))
			},
			{
				341,
				new BrushData(341, "Base/Levels/Level1/Brushes/Tile/Tile_HorizonMove", typeof(HorizonMoveTile))
			},
			{
				342,
				new BrushData(342, "Base/Levels/Level3/Brushes/Tile/Home_MultiSegmentAnimationTile_Road01_Up", typeof(MultiSegmentAnimationTile))
			},
			{
				343,
				new BrushData(343, "Base/Levels/Level3/Brushes/Tile/Home_MultiSegmentAnimationTile_Road02_Up", typeof(MultiSegmentAnimationTile))
			},
			{
				344,
				new BrushData(344, "Base/Levels/Level3/Brushes/Tile/Home_MultiSegmentAnimationTile_Road03_Up", typeof(MultiSegmentAnimationTile))
			},
			{
				401,
				new BrushData(401, "Base/Levels/Level5/Brushes/Tile/Fate_Piano_white", typeof(FreeCollideAnimTile))
			},
			{
				402,
				new BrushData(402, "Base/Levels/Level5/Brushes/Tile/Tile_LV5_End01_True", typeof(HangingWinTile))
			},
			{
				403,
				new BrushData(403, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_road_yes", typeof(FreeCollideAnimTile))
			},
			{
				404,
				new BrushData(404, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_road_no", typeof(FreeCollideAnimTile))
			},
			{
				405,
				new BrushData(405, "Base/Levels/Level5/Brushes/Tile/Tile_SuperJump", typeof(JumpDistanceQTETile))
			},
			{
				406,
				new BrushData(406, "Base/Levels/Level5/Brushes/Tile/Tile_Fate_SuperJump", typeof(JumpDistanceQTETile))
			},
			{
				407,
				new BrushData(407, "Base/Levels/Level5/Brushes/Tile/Fate_Piano_white_s", typeof(FreeCollideAnimTile))
			},
			{
				408,
				new BrushData(408, "Base/Levels/Level5/Brushes/Tile/Tile_SuperJump_big", typeof(JumpDistanceQTETile))
			},
			{
				411,
				new BrushData(411, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_white_L", typeof(FreeCollideAnimTile))
			},
			{
				412,
				new BrushData(412, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_white_R", typeof(FreeCollideAnimTile))
			},
			{
				413,
				new BrushData(413, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_road_jump", typeof(JumpDistanceQTETile))
			},
			{
				421,
				new BrushData(421, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_white_move_L", typeof(MoveAllDirTile))
			},
			{
				422,
				new BrushData(422, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_white_move_R", typeof(MoveAllDirTile))
			},
			{
				424,
				new BrushData(424, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_FreeMoveTile_L", typeof(FreeMoveTile))
			},
			{
				425,
				new BrushData(425, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_FreeMoveTile_R", typeof(FreeMoveTile))
			},
			{
				426,
				new BrushData(426, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_FreeMoveTile_white", typeof(FreeMoveTile))
			},
			{
				427,
				new BrushData(427, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_qipan_white_move", typeof(MoveAllDirTile))
			},
			{
				428,
				new BrushData(428, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_qipan_white", typeof(EmissionTile))
			},
			{
				437,
				new BrushData(437, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_qipan_piao", typeof(HorizonMoveTile))
			},
			{
				438,
				new BrushData(438, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_Piano_piao1", typeof(HorizonMoveTile))
			},
			{
				439,
				new BrushData(439, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_Piano_piao", typeof(HorizonMoveTile))
			},
			{
				440,
				new BrushData(440, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_360_bai", typeof(FreeCollideAnimTile))
			},
			{
				441,
				new BrushData(441, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_360_hei", typeof(FreeCollideAnimTile))
			},
			{
				442,
				new BrushData(442, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_360_hei_2", typeof(FreeCollideAnimTile))
			},
			{
				443,
				new BrushData(443, "Base/Levels/Level5/Brushes/Tile/Fate_Tile_360_hei_3", typeof(FreeCollideAnimTile))
			},
			{
				449,
				new BrushData(449, "Base/Levels/Level5/Brushes/Tile/WideTilePro", typeof(WideTilePro))
			},
			{
				450,
				new BrushData(450, "Base/Levels/Level5/Brushes/Tile/WideTilePro5", typeof(WideTilePro))
			},
			{
				451,
				new BrushData(451, "Base/Levels/Level5/Brushes/Tile/Fate_tile_Normal", typeof(NormalTile))
			},
			{
				452,
				new BrushData(452, "Base/Levels/Level3/Brushes/Tile/Tile_Road_Water", typeof(NormalTile))
			},
			{
				499,
				new BrushData(499, "Base/Levels/Level5/Brushes/Tile/Tile_SuperJump", typeof(JumpDistanceTrigger))
			},
			{
				501,
				new BrushData(501, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road01", typeof(NormalTile))
			},
			{
				502,
				new BrushData(502, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road02", typeof(NormalTile))
			},
			{
				503,
				new BrushData(503, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road03", typeof(NormalTile))
			},
			{
				504,
				new BrushData(504, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road04", typeof(NormalTile))
			},
			{
				505,
				new BrushData(505, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road05", typeof(NormalTile))
			},
			{
				506,
				new BrushData(506, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Jump_small", typeof(NormalJumpTile))
			},
			{
				507,
				new BrushData(507, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Road01", typeof(NormalTile))
			},
			{
				508,
				new BrushData(508, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Road02", typeof(NormalTile))
			},
			{
				509,
				new BrushData(509, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Road03", typeof(NormalTile))
			},
			{
				510,
				new BrushData(510, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Road04", typeof(NormalTile))
			},
			{
				511,
				new BrushData(511, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Road05", typeof(NormalTile))
			},
			{
				512,
				new BrushData(512, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Move_B", typeof(TrapRootTile))
			},
			{
				513,
				new BrushData(513, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Move_F", typeof(TrapRootTile))
			},
			{
				514,
				new BrushData(514, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Move_L", typeof(TrapRootTile))
			},
			{
				515,
				new BrushData(515, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Move_R", typeof(TrapRootTile))
			},
			{
				516,
				new BrushData(516, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_MoveAuto_B", typeof(TrapTriggerTile))
			},
			{
				517,
				new BrushData(517, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_MoveAuto_F", typeof(TrapTriggerTile))
			},
			{
				518,
				new BrushData(518, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_MoveAuto_L", typeof(TrapTriggerTile))
			},
			{
				519,
				new BrushData(519, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_MoveAuto_R", typeof(TrapTriggerTile))
			},
			{
				520,
				new BrushData(520, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_MoveFollow", typeof(TrapChildTile))
			},
			{
				521,
				new BrushData(521, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_TwoStepsJump_Start", typeof(JumpDistanceQTETile))
			},
			{
				522,
				new BrushData(522, "Base/Levels/LightMap/Brush/Tile/Tile_TwoStepsJump_Floating", typeof(JumpDistanceQTETile))
			},
			{
				523,
				new BrushData(523, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road_UP", typeof(MoveAllDirTile))
			},
			{
				524,
				new BrushData(524, "Base/Levels/LightMap/Brush/Tile/Tile_Road_Empty", typeof(FreeCollideTile))
			},
			{
				525,
				new BrushData(525, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road_UP_Tile", typeof(MoveAllDirTile))
			},
			{
				526,
				new BrushData(526, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_MagicCube_3x3", typeof(MagicCubeTile3x3))
			},
			{
				527,
				new BrushData(527, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_MagicCube_5x5", typeof(MagicCubeTile5x5))
			},
			{
				528,
				new BrushData(528, "Base/Levels/LightMap/Brush/Tile/Tile_MubanAni_2", typeof(FreeCollideAnimTile))
			},
			{
				529,
				new BrushData(529, "Base/Levels/LightMap/Brush/Tile/Tile_MubanAni_3", typeof(FreeCollideAnimTile))
			},
			{
				530,
				new BrushData(530, "Base/Levels/LightMap/Brush/Tile/Tile_Muban_5_1", typeof(FreeCollideTile))
			},
			{
				531,
				new BrushData(531, "Base/Levels/LightMap/Brush/Tile/Tile_Muban_5_2", typeof(FreeCollideTile))
			},
			{
				532,
				new BrushData(532, "Base/Levels/LightMap/Brush/Tile/Tile_Muban_5_3", typeof(FreeCollideTile))
			},
			{
				533,
				new BrushData(533, "Base/Levels/LightMap/Brush/Tile/Tile_Muban_2", typeof(FreeCollideTile))
			},
			{
				534,
				new BrushData(534, "Base/Levels/LightMap/Brush/Tile/Tile_Muban_3", typeof(FreeCollideTile))
			},
			{
				535,
				new BrushData(535, "Base/Levels/LightMap/Brush/Tile/FreeMoveTile", typeof(FreeMoveTile))
			},
			{
				536,
				new BrushData(536, "Base/Levels/LightMap/Brush/Tile/Tile_SuperJump", typeof(JumpDistanceQTETile))
			},
			{
				537,
				new BrushData(537, "Base/Levels/LightMap/Brush/Tile/Tile_Shendian_Jump_small", typeof(NormalJumpTile))
			},
			{
				538,
				new BrushData(538, "Base/Levels/LightMap/Brush/Tile/Tile_Shendian_TwoStepsJump_Start", typeof(JumpDistanceQTETile))
			},
			{
				539,
				new BrushData(539, "Base/Levels/LightMap/Brush/Tile/Tile_VerticalMove", typeof(CycleFBTile))
			},
			{
				540,
				new BrushData(540, "Base/Levels/LightMap/Brush/Tile/Tile_Shendian_Road_UP", typeof(MoveAllDirTile))
			},
			{
				541,
				new BrushData(541, "Base/Levels/LightMap/Brush/Tile/Tile_GlassRootTile", typeof(GlassRootTile))
			},
			{
				542,
				new BrushData(542, "Base/Levels/LightMap/Brush/Tile/Tile_GlassChildTile", typeof(GlassChildTile))
			},
			{
				543,
				new BrushData(543, "Base/Levels/LightMap/Brush/Tile/Tile_GlassRootTile_None", typeof(GlassRootTile))
			},
			{
				544,
				new BrushData(544, "Base/Levels/LightMap/Brush/Tile/Tile_End01_True_lv4", typeof(HangingWinTile))
			},
			{
				545,
				new BrushData(545, "Base/Levels/LightMap/Brush/Tile/Tile_Road_WaterS", typeof(NormalWideTile))
			},
			{
				546,
				new BrushData(546, "Base/Levels/LightMap/Brush/Tile/AiJi_WideTilePro", typeof(WideTilePro))
			},
			{
				547,
				new BrushData(547, "Base/Levels/LightMap/Brush/Tile/AiJi_AutoSpeedHorizonMoveTile", typeof(AutoSpeedHorizonMoveTile))
			},
			{
				548,
				new BrushData(548, "Base/Levels/LightMap/Brush/Tile/AiJi_AutoSpeedHorizonMoveTile_bake", typeof(AutoSpeedHorizonMoveTile))
			},
			{
				551,
				new BrushData(551, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road01_Bake", typeof(NormalTile))
			},
			{
				552,
				new BrushData(552, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road02_Bake", typeof(NormalTile))
			},
			{
				553,
				new BrushData(553, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road03_Bake", typeof(NormalTile))
			},
			{
				554,
				new BrushData(554, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road04_Bake", typeof(NormalTile))
			},
			{
				555,
				new BrushData(555, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road05_Bake", typeof(NormalTile))
			},
			{
				556,
				new BrushData(556, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Jump_small_Bake", typeof(NormalJumpTile))
			},
			{
				557,
				new BrushData(557, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_TwoStepsJump_Start_Bake", typeof(JumpDistanceQTETile))
			},
			{
				558,
				new BrushData(558, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road_UP_Bake", typeof(MoveAllDirTile))
			},
			{
				559,
				new BrushData(559, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Jump_UP", typeof(MoveAllDirTile))
			},
			{
				560,
				new BrushData(560, "Base/Levels/LightMap/Brush/Tile/Tile_VerticalMove_normal", typeof(CycleFBTile))
			},
			{
				561,
				new BrushData(561, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Jump_small_Blank", typeof(NormalJumpTile))
			},
			{
				562,
				new BrushData(562, "Base/Levels/LightMap/Brush/Tile/FreeMoveJumpTile_Pharaohs", typeof(FreeMoveTile))
			},
			{
				601,
				new BrushData(601, "Base/Levels/Level3/Brushes/Tile/Tile_VerticalMove", typeof(CycleFBTile))
			},
			{
				602,
				new BrushData(602, "Base/Levels/Level3/Brushes/Tile/Tile_InputType", typeof(InputTypeTile))
			},
			{
				701,
				new BrushData(701, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Empty", typeof(WideTilePro))
			},
			{
				702,
				new BrushData(702, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Piano_Normal", typeof(CombinationPianoKeyTile))
			},
			{
				703,
				new BrushData(703, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Piano_SuperJump", typeof(CombinationPianoKeyJumpTile))
			},
			{
				704,
				new BrushData(704, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Piano_SuperJump_white", typeof(CombinationPianoKeyJumpTile))
			},
			{
				705,
				new BrushData(705, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Piano_Normal_white", typeof(CombinationPianoKeyTile))
			},
			{
				706,
				new BrushData(706, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Piano_SuperJump_empty", typeof(JumpDistanceQTETile))
			},
			{
				707,
				new BrushData(707, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Empty1x4", typeof(WideTilePro))
			},
			{
				708,
				new BrushData(708, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Empty1x1", typeof(WideTilePro))
			},
			{
				709,
				new BrushData(709, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Piano_SuperJump_Laba", typeof(CombinationPianoKeyJumpTile))
			},
			{
				710,
				new BrushData(710, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_paper_jump", typeof(JumpDistanceQTETile))
			},
			{
				711,
				new BrushData(711, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Normal_yes", typeof(FreeCollideAnimTile))
			},
			{
				712,
				new BrushData(712, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Normal_no", typeof(FreeCollideAnimTile))
			},
			{
				713,
				new BrushData(713, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_normal_jump", typeof(JumpDistanceQTETile))
			},
			{
				714,
				new BrushData(714, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_paper_Normal", typeof(NormalTile))
			},
			{
				715,
				new BrushData(715, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_paper_empty", typeof(NormalTile))
			},
			{
				716,
				new BrushData(716, "Base/Levels/Waltz/Brushes/Tile/Waltz_MultiSegmentAnimationTile_Paper", typeof(MultiSegmentAnimationTile))
			},
			{
				717,
				new BrushData(717, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_paper_jump_empty", typeof(JumpDistanceQTETile))
			},
			{
				718,
				new BrushData(718, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_paper_jump_animator1x1", typeof(JumpEmptyTile))
			},
			{
				719,
				new BrushData(719, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Qinbing_01", typeof(FreeCollideTile))
			},
			{
				720,
				new BrushData(720, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Qinbing_02", typeof(FreeCollideTile))
			},
			{
				721,
				new BrushData(721, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_paper_jump_animator3x1", typeof(JumpEmptyTile))
			},
			{
				722,
				new BrushData(722, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Yuepu_jump", typeof(JumpDistanceQTETile))
			},
			{
				723,
				new BrushData(723, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Piano_SuperJump_Laba_right", typeof(CombinationPianoKeyJumpTile))
			},
			{
				724,
				new BrushData(724, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Piano_SuperJump_Laba_easy", typeof(CombinationPianoKeyJumpTile))
			},
			{
				793,
				new BrushData(793, "Base/Levels/Waltz/Brushes/Tile/Waltz_Tile_Piano_Empty", typeof(JumpDistanceQTETile))
			},
			{
				801,
				new BrushData(801, "Base/Levels/Theif/Brushes/Tile/Theif_Road", typeof(EmissionTile))
			},
			{
				802,
				new BrushData(802, "Base/Levels/Theif/Brushes/Tile/Theif_Road_Water", typeof(FreeCollideTile))
			},
			{
				803,
				new BrushData(803, "Base/Levels/Theif/Brushes/Tile/Theif_Road_Water_5", typeof(FreeCollideTile))
			},
			{
				804,
				new BrushData(804, "Base/Levels/Theif/Brushes/Tile/Theif_Road_ForcePosTile", typeof(ForcePosTIle))
			},
			{
				811,
				new BrushData(811, "Base/Levels/Theif/Brushes/Tile/Theif_CraneTile", typeof(CraneTile))
			},
			{
				812,
				new BrushData(812, "Base/Levels/Theif/Brushes/Tile/Theif_CraneJumpTile", typeof(CraneJumpTile))
			},
			{
				813,
				new BrushData(813, "Base/Levels/Theif/Brushes/Tile/Theif_CraneTile_Enemy_R", typeof(CraneTile))
			},
			{
				814,
				new BrushData(814, "Base/Levels/Theif/Brushes/Tile/Theif_CraneJumpTile_Diamond", typeof(CraneJumpTile))
			},
			{
				815,
				new BrushData(815, "Base/Levels/Theif/Brushes/Tile/Theif_CraneTile_Enemy_L", typeof(CraneTile))
			},
			{
				816,
				new BrushData(816, "Base/Levels/Theif/Brushes/Tile/Thief_Tile_SuperJumpQTE", typeof(JumpDistanceQTETile))
			},
			{
				817,
				new BrushData(817, "Base/Levels/Theif/Brushes/Tile/Theif_CraneTile_Enemy_00", typeof(CraneTile))
			},
			{
				818,
				new BrushData(818, "Base/Levels/Theif/Brushes/Tile/Thief_ChuanSong", typeof(FreeCollideAnimTile))
			},
			{
				819,
				new BrushData(819, "Base/Levels/Theif/Brushes/Tile/Thief_ChuanSong01", typeof(FreeCollideAnimTile))
			},
			{
				820,
				new BrushData(820, "Base/Levels/Theif/Brushes/Tile/Thief_ChuanSong02", typeof(FreeCollideAnimTile))
			},
			{
				821,
				new BrushData(821, "Base/Levels/Theif/Brushes/Tile/Thief_ChuanSong00", typeof(FreeCollideAnimTile))
			},
			{
				822,
				new BrushData(822, "Base/Levels/Theif/Brushes/Tile/Theif_SuperJump_Kong", typeof(JumpDistanceQTETile))
			},
			{
				823,
				new BrushData(823, "Base/Levels/Theif/Brushes/Tile/Theif_CraneTile_Enemy_R_H", typeof(CraneTile))
			},
			{
				824,
				new BrushData(824, "Base/Levels/Theif/Brushes/Tile/Theif_CraneJumpTile_2", typeof(CraneJumpTile))
			},
			{
				825,
				new BrushData(825, "Base/Levels/Theif/Brushes/Tile/Theif_CraneTile_Enemy_R_A", typeof(CraneTile))
			},
			{
				826,
				new BrushData(826, "Base/Levels/Theif/Brushes/Tile/Theif_CraneTile_Enemy_R_B", typeof(CraneTile))
			},
			{
				1100,
				new BrushData(1100, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Waltz_Tile_Piano_Normal", typeof(CombinationPianoKeyTile))
			},
			{
				1101,
				new BrushData(1101, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Waltz_Tile_Piano_SuperJump", typeof(CombinationPianoKeyJumpTile))
			},
			{
				1102,
				new BrushData(1102, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Waltz_Tile_Piano_SuperJump_white", typeof(CombinationPianoKeyJumpTile))
			},
			{
				1103,
				new BrushData(1103, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Waltz_Tile_Piano_Normal_white", typeof(CombinationPianoKeyTile))
			},
			{
				1104,
				new BrushData(1104, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_Danhuangguan_normal", typeof(NormalTile))
			},
			{
				1105,
				new BrushData(1105, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_Danhuangguan_zhijia", typeof(NormalTile))
			},
			{
				1106,
				new BrushData(1106, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_Danhuangguan_Jump", typeof(JumpDistanceQTETile))
			},
			{
				1107,
				new BrushData(1107, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_Jump_Empty1x1", typeof(JumpDistanceQTETile))
			},
			{
				1108,
				new BrushData(1108, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_Empty1x1", typeof(NormalTile))
			},
			{
				1109,
				new BrushData(1109, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_Empty3x1", typeof(WideTilePro))
			},
			{
				1110,
				new BrushData(1110, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_Rotation01", typeof(FreeCollideAnimTile))
			},
			{
				1111,
				new BrushData(1111, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_AutoSpeedHorizonMoveTile", typeof(AutoSpeedHorizonMoveTile))
			},
			{
				1138,
				new BrushData(1138, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_xiaohao_jump_offon", typeof(JumpDistanceQTETile))
			},
			{
				1139,
				new BrushData(1139, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_xiaohao_jump", typeof(JumpDistanceQTETile))
			},
			{
				1140,
				new BrushData(1140, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_xiaohao_jump_only", typeof(JumpDistanceQTETile))
			},
			{
				1141,
				new BrushData(1141, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_xiaohao_normal", typeof(MultiSegmentAnimationTile))
			},
			{
				1142,
				new BrushData(1142, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_xiaohao_normal_only", typeof(NormalTile))
			},
			{
				1143,
				new BrushData(1143, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_xiaohao_move", typeof(MoveAllDirTile))
			},
			{
				1144,
				new BrushData(1144, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_xiaohao_move_only", typeof(MoveAllDirTile))
			},
			{
				1145,
				new BrushData(1145, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_xiaohao_piao", typeof(HorizonMoveTile))
			},
			{
				1146,
				new BrushData(1146, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_Tile_xiaohao_piao_only", typeof(HorizonMoveTile))
			},
			{
				1149,
				new BrushData(1149, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_danhuangguan_jump", typeof(MultiSegmentAnimationTile))
			},
			{
				1150,
				new BrushData(1150, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_danhuangguan", typeof(MultiSegmentAnimationTile))
			},
			{
				1151,
				new BrushData(1151, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_danhuangguan_zhijia", typeof(MultiSegmentAnimationTile))
			},
			{
				1152,
				new BrushData(1152, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_Piano_Fly01", typeof(MultiSegmentAnimationTile))
			},
			{
				1153,
				new BrushData(1153, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_Piano_Fly02", typeof(MultiSegmentAnimationTile))
			},
			{
				1154,
				new BrushData(1154, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_Piano_Fly03", typeof(MultiSegmentAnimationTile))
			},
			{
				1155,
				new BrushData(1155, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_danhuangguan_Rotation01", typeof(MultiSegmentAnimationTile))
			},
			{
				1156,
				new BrushData(1156, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_Piano_Fly01_S", typeof(MultiSegmentAnimationTile))
			},
			{
				1157,
				new BrushData(1157, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_Piano_Fly02_S", typeof(MultiSegmentAnimationTile))
			},
			{
				1158,
				new BrushData(1158, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_Piano_Fly03_S", typeof(MultiSegmentAnimationTile))
			},
			{
				1159,
				new BrushData(1159, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_Piano_jumpheng", typeof(MultiSegmentAnimationTile))
			},
			{
				1160,
				new BrushData(1160, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_yuepu_jump", typeof(MultiSegmentAnimationTile))
			},
			{
				1161,
				new BrushData(1161, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_yuepu_01", typeof(MultiSegmentAnimationTile))
			},
			{
				1162,
				new BrushData(1162, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_yuepu_02", typeof(MultiSegmentAnimationTile))
			},
			{
				1163,
				new BrushData(1163, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_yuepu_03", typeof(MultiSegmentAnimationTile))
			},
			{
				1164,
				new BrushData(1164, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_yuepu_04", typeof(MultiSegmentAnimationTile))
			},
			{
				1165,
				new BrushData(1165, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_yuepu_jump01", typeof(MultiSegmentAnimationTile))
			},
			{
				1167,
				new BrushData(1167, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_Piano_Fly04", typeof(MultiSegmentAnimationTile))
			},
			{
				1168,
				new BrushData(1168, "Base/Levels/Fantasia_Jazz/Brushes/Tile/Jazz_MultiSegmentAnimationTile_Piano_Fly04_S", typeof(MultiSegmentAnimationTile))
			},
			{
				1201,
				new BrushData(1201, "Base/Levels/Home_Rainbow/Brushes/Tile/Rainbow_Road_Water_3", typeof(FreeCollideTile))
			},
			{
				1202,
				new BrushData(1202, "Base/Levels/Home_Rainbow/Brushes/Tile/Rainbow_Tile_ShanDong", typeof(NormalTile))
			},
			{
				1203,
				new BrushData(1203, "Base/Levels/Home_Rainbow/Brushes/Tile/Rainbow_Tile_ShanDong01", typeof(NormalTile))
			},
			{
				1205,
				new BrushData(1205, "Base/Levels/Home_Rainbow/Brushes/Tile/RandomAnimTile", typeof(RandomAnimTile))
			},
			{
				1300,
				new BrushData(1300, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_P1_Road03", typeof(NormalTile))
			},
			{
				1301,
				new BrushData(1301, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_Road_Shining", typeof(EmissionTile))
			},
			{
				1302,
				new BrushData(1302, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_XiaoZhen_Move", typeof(MoveAllDirTile))
			},
			{
				1303,
				new BrushData(1303, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_P1_Move_F", typeof(TrapRootTile))
			},
			{
				1304,
				new BrushData(1304, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_TirggerEffectJump", typeof(TriggerEffectJumpTile))
			},
			{
				1305,
				new BrushData(1305, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_TirggerEffectJump01", typeof(TriggerEffectJumpTile))
			},
			{
				1306,
				new BrushData(1306, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_Eagle_SuperJump", typeof(JumpDistanceQTETile))
			},
			{
				1307,
				new BrushData(1307, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_Elevator01", typeof(ElevatorTile))
			},
			{
				1308,
				new BrushData(1308, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_XiongKong_JumpStar", typeof(SuperStarJumpTile))
			},
			{
				1309,
				new BrushData(1309, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_Star", typeof(MultiSegmentAnimationTile))
			},
			{
				1310,
				new BrushData(1310, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_Star_Jump", typeof(MultiSegmentAnimationTile))
			},
			{
				1311,
				new BrushData(1311, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_Tile_SuperJump", typeof(JumpDistanceQTETile))
			},
			{
				1312,
				new BrushData(1312, "Base/Levels/Level1/Brushes/Tile/StarryDream_Tile_Star", typeof(MultiSegmentAnimationTile))
			},
			{
				1313,
				new BrushData(1313, "Base/Levels/Level1/Brushes/Tile/Tile_P1_Move_Child", typeof(TrapChildTile))
			},
			{
				1314,
				new BrushData(1314, "Base/Levels/Level1/Brushes/Tile/Tile_Midground_Star", typeof(FreeMoveTile))
			},
			{
				1315,
				new BrushData(1315, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_Tile_Midground_Star", typeof(FreeMoveTile))
			},
			{
				1400,
				new BrushData(1400, "Base/Levels/LightMap/Brush/Tile/AiJi_Move_Water", typeof(MoveAllDirTile))
			},
			{
				1401,
				new BrushData(1401, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Road02_HN", typeof(NormalTile))
			},
			{
				1402,
				new BrushData(1402, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Road03_HN", typeof(NormalTile))
			},
			{
				1403,
				new BrushData(1403, "Base/Levels/LightMap/Brush/Tile/Tile_ShenDian_Road04_HN", typeof(NormalTile))
			},
			{
				1404,
				new BrushData(1404, "Base/Levels/LightMap/Brush/Tile/Tile_Shendian_Jump_small_HN", typeof(NormalJumpTile))
			},
			{
				1405,
				new BrushData(1405, "Base/Levels/LightMap/Brush/Tile/Tile_Shendian_TwoStepsJump_Start_HN", typeof(JumpDistanceQTETile))
			},
			{
				1406,
				new BrushData(1406, "Base/Levels/LightMap/Brush/Tile/Tile_Shendian_Road_UP_HN", typeof(MoveAllDirTile))
			},
			{
				1407,
				new BrushData(1407, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road_UP_Bake_HN", typeof(MoveAllDirTile))
			},
			{
				1408,
				new BrushData(1408, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road_UP_Tile_HN", typeof(MoveAllDirTile))
			},
			{
				1409,
				new BrushData(1409, "Base/Levels/LightMap/Brush/Tile/Tile_ShaMo_Road_Move_Bake_HN", typeof(MultiSegmentAnimationTile))
			},
			{
				1500,
				new BrushData(1500, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Empty", typeof(NormalTile))
			},
			{
				1501,
				new BrushData(1501, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_SuperJump", typeof(JumpDistanceQTETile))
			},
			{
				1502,
				new BrushData(1502, "Base/Levels/Samurai/Brushes/Tile/Samurai_MultiSegmentAnimationTile_Chuan_L", typeof(MultiSegmentAnimationTile))
			},
			{
				1503,
				new BrushData(1503, "Base/Levels/Samurai/Brushes/Tile/Samurai_MultiSegmentAnimationTile_Chuan_M", typeof(MultiSegmentAnimationTile))
			},
			{
				1504,
				new BrushData(1504, "Base/Levels/Samurai/Brushes/Tile/Samurai_MultiSegmentAnimationTile_Chuan_R", typeof(MultiSegmentAnimationTile))
			},
			{
				1505,
				new BrushData(1505, "Base/Levels/Samurai/Brushes/Tile/Samurai_MultiSegmentAnimationTile_Chuan_F", typeof(MultiSegmentAnimationTile))
			},
			{
				1506,
				new BrushData(1506, "Base/Levels/Samurai/Brushes/Tile/Samurai_MultiSegmentAnimationTile_Chuan_S", typeof(MultiSegmentAnimationTile))
			},
			{
				1507,
				new BrushData(1507, "Base/Levels/Samurai/Brushes/Tile/Samurai_MultiSegmentAnimationTile_Chuan_B", typeof(MultiSegmentAnimationTile))
			},
			{
				1508,
				new BrushData(1508, "Base/Levels/Samurai/Brushes/Tile/Samurai_FreeCollideTile", typeof(WideTilePro))
			},
			{
				1511,
				new BrushData(1511, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Wood", typeof(NormalTile))
			},
			{
				1512,
				new BrushData(1512, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Wood_Fire", typeof(NormalTile))
			},
			{
				1513,
				new BrushData(1513, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Wood_Move_F", typeof(TrapRootTile))
			},
			{
				1514,
				new BrushData(1514, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Wood_Move_Child", typeof(TrapChildTile))
			},
			{
				1515,
				new BrushData(1515, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_HorizonMove_Wood", typeof(HorizonMoveTile))
			},
			{
				1516,
				new BrushData(1516, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Move_Wood", typeof(MoveAllDirTile))
			},
			{
				1517,
				new BrushData(1517, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Wood_pian", typeof(NormalTile))
			},
			{
				1518,
				new BrushData(1518, "Base/Levels/Samurai/Brushes/Tile/Samurai_MultiSegmentAnimationTile_Wood01", typeof(MultiSegmentAnimationTile))
			},
			{
				1519,
				new BrushData(1519, "Base/Levels/Samurai/Brushes/Tile/Samurai_MultiSegmentAnimationTile_Wood02", typeof(MultiSegmentAnimationTile))
			},
			{
				1520,
				new BrushData(1520, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_HorizonMove_Chuan", typeof(HorizonMoveTile))
			},
			{
				1521,
				new BrushData(1521, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Stone", typeof(WideTilePro))
			},
			{
				1522,
				new BrushData(1522, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Stone_Move_F", typeof(TrapRootTile))
			},
			{
				1523,
				new BrushData(1523, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Stone_Move_Child", typeof(TrapChildTile))
			},
			{
				1524,
				new BrushData(1524, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Stone_GlassRootTile", typeof(GlassRootTile))
			},
			{
				1525,
				new BrushData(1525, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Stone_GlassChildTile", typeof(GlassChildTile))
			},
			{
				1530,
				new BrushData(1530, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_part5", typeof(NormalTile))
			},
			{
				1531,
				new BrushData(1531, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Xizi1x1", typeof(NormalTile))
			},
			{
				1533,
				new BrushData(1533, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Xizi3x1", typeof(WideTilePro))
			},
			{
				1535,
				new BrushData(1535, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Xizi5x1", typeof(WideTilePro))
			},
			{
				1536,
				new BrushData(1536, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Xizi_Move_F", typeof(TrapRootTile))
			},
			{
				1537,
				new BrushData(1537, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Xizi_Move_Child", typeof(TrapChildTile))
			},
			{
				1538,
				new BrushData(1538, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Xizi_Fire", typeof(NormalTile))
			},
			{
				1539,
				new BrushData(1539, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Move_Xizi1x1", typeof(MoveAllDirTile))
			},
			{
				1540,
				new BrushData(1540, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_HorizonMove_Xizi", typeof(HorizonMoveTile))
			},
			{
				1541,
				new BrushData(1541, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Enemy1x1", typeof(NormalTile))
			},
			{
				1542,
				new BrushData(1542, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_GlassRoot_Xizi", typeof(GlassRootTile))
			},
			{
				1543,
				new BrushData(1543, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_GlassChild_Xizi", typeof(GlassChildTile))
			},
			{
				1544,
				new BrushData(1544, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_GlassRoot_part5", typeof(GlassRootTile))
			},
			{
				1545,
				new BrushData(1545, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_GlassChild_part5", typeof(GlassChildTile))
			},
			{
				1556,
				new BrushData(1556, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_HorizonMove_Muban3", typeof(HorizonMoveTile))
			},
			{
				1557,
				new BrushData(1557, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_HorizonMove_Muban4", typeof(HorizonMoveTile))
			},
			{
				1558,
				new BrushData(1558, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_HorizonMove_Muban1", typeof(HorizonMoveTile))
			},
			{
				1559,
				new BrushData(1559, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_HorizonMove_Muban2", typeof(HorizonMoveTile))
			},
			{
				1560,
				new BrushData(1560, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Muban0", typeof(WideTilePro))
			},
			{
				1561,
				new BrushData(1561, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Muban1", typeof(NormalTile))
			},
			{
				1562,
				new BrushData(1562, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Muban2", typeof(NormalTile))
			},
			{
				1563,
				new BrushData(1563, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Muban3", typeof(NormalTile))
			},
			{
				1564,
				new BrushData(1564, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Muban4", typeof(NormalTile))
			},
			{
				1565,
				new BrushData(1565, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Muban5", typeof(NormalTile))
			},
			{
				1566,
				new BrushData(1566, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Move_Muban1", typeof(MoveAllDirTile))
			},
			{
				1567,
				new BrushData(1567, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Move_Muban2", typeof(MoveAllDirTile))
			},
			{
				1568,
				new BrushData(1568, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Move_Muban3", typeof(MoveAllDirTile))
			},
			{
				1569,
				new BrushData(1569, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Move_Muban4", typeof(MoveAllDirTile))
			},
			{
				1570,
				new BrushData(1570, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_Move_Muban5", typeof(MoveAllDirTile))
			},
			{
				1571,
				new BrushData(1571, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapRoot_Muban1", typeof(TrapRootTile))
			},
			{
				1572,
				new BrushData(1572, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapRoot_Muban2", typeof(TrapRootTile))
			},
			{
				1573,
				new BrushData(1573, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapRoot_Muban3", typeof(TrapRootTile))
			},
			{
				1574,
				new BrushData(1574, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapRoot_Muban4", typeof(TrapRootTile))
			},
			{
				1575,
				new BrushData(1575, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapRoot_Muban5", typeof(TrapRootTile))
			},
			{
				1576,
				new BrushData(1576, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapChild_Muban1", typeof(TrapChildTile))
			},
			{
				1577,
				new BrushData(1577, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapChild_Muban2", typeof(TrapChildTile))
			},
			{
				1578,
				new BrushData(1578, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapChild_Muban3", typeof(TrapChildTile))
			},
			{
				1579,
				new BrushData(1579, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapChild_Muban4", typeof(TrapChildTile))
			},
			{
				1580,
				new BrushData(1580, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_TrapChild_Muban5", typeof(TrapChildTile))
			},
			{
				1599,
				new BrushData(1599, "Base/Levels/Samurai/Brushes/Tile/Samurai_Tile_End01_True", typeof(HangingWinTile))
			},
			{
				1600,
				new BrushData(1600, "Base/Levels/Thief_2/Brushes/Tile/Theif2_CraneTile_Enemy_R", typeof(CraneTile))
			},
			{
				1601,
				new BrushData(1601, "Base/Levels/Thief_2/Brushes/Tile/Theif2_CraneTile_Enemy_00", typeof(CraneTile))
			},
			{
				1602,
				new BrushData(1602, "Base/Levels/Thief_2/Brushes/Tile/Theif2_CraneJumpTile_2", typeof(CraneJumpTile))
			},
			{
				9001,
				new BrushData(9001, "Base/Levels/Level5/Brushes/Tile/Fate_JumpTile_white", typeof(JumpDistanceQTETile))
			}
		};

		private static Dictionary<int, BrushData> m_enemyBrushs = new Dictionary<int, BrushData>
		{
			{
				0,
				new BrushData(0, "Base/Levels/Level1/Brushes/Enemy/Effect_common_tiaodianzhiyin", typeof(AnimEnemyPro))
			},
			{
				1,
				new BrushData(1, "Base/Levels/Level1/Brushes/Enemy/Award_Crown", typeof(CrownAward))
			},
			{
				2,
				new BrushData(2, "Base/Levels/Level1/Brushes/Enemy/Award_Diamond", typeof(DiamondAward))
			},
			{
				3,
				new BrushData(3, "Base/Levels/Level1/Brushes/Enemy/Effect_common_tiaodianzhiyin_duan", typeof(AnimEnemyPro))
			},
			{
				4,
				new BrushData(4, "Base/Levels/Level1/Brushes/Enemy/Award_Diamond-3", typeof(DiamondAward))
			},
			{
				5,
				new BrushData(5, "Base/Levels/Level1/Brushes/Enemy/Effect_Danger", typeof(AnimEnemyPro))
			},
			{
				6,
				new BrushData(6, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_role_sprite", typeof(AnimEnemyProByTutorialEnemyRole))
			},
			{
				7,
				new BrushData(7, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_road", typeof(AnimEnemyPro))
			},
			{
				8,
				new BrushData(8, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Effect_Jump_UP", typeof(TwoEffectTriggerPro))
			},
			{
				9,
				new BrushData(9, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_road_L", typeof(AnimEnemyPro))
			},
			{
				10,
				new BrushData(10, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_road_R", typeof(AnimEnemyPro))
			},
			{
				11,
				new BrushData(11, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_road1", typeof(AnimEnemyPro))
			},
			{
				12,
				new BrushData(12, "Base/Levels/Level2/Brushes/Enemy/Enemy_RedSea_QiChuan", typeof(AnimParticleEnemy))
			},
			{
				13,
				new BrushData(13, "Base/Levels/Level2/Brushes/Enemy/Enemy_Desert_JingJi01", typeof(AnimEnemy))
			},
			{
				15,
				new BrushData(15, "Base/Levels/Level2/Brushes/Enemy/Enemy_RedSea_JinZiTa", typeof(AnimFinshHideEnemy))
			},
			{
				17,
				new BrushData(17, "Base/Levels/Level2/Brushes/Enemy/Midground_LV1_P2_Mid_Plant01", typeof(AnimEnemy))
			},
			{
				18,
				new BrushData(18, "Base/Levels/Level2/Brushes/Enemy/Midground_LV1_P2_Mid_Tengman_1", typeof(AnimEnemy))
			},
			{
				19,
				new BrushData(19, "Base/Levels/Level2/Brushes/Enemy/Enemy_RedSea_SheAttack_01", typeof(AnimEnemy))
			},
			{
				20,
				new BrushData(20, "Base/Levels/Level2/Brushes/Enemy/Enemy_RedSea_SheAttack_02", typeof(AnimParticleEnemy))
			},
			{
				22,
				new BrushData(22, "Base/Levels/Level2/Brushes/Enemy/Midground_LV1_P1_Qiang01", typeof(MoveAllEnemy))
			},
			{
				25,
				new BrushData(25, "Base/Levels/Level2/Brushes/Enemy/Midground_LV1_P1_DiaoXiang", typeof(AnimEnemy))
			},
			{
				26,
				new BrushData(26, "Base/Levels/Level2/Brushes/Enemy/Midground_LV1_P1_Zhu_Ani01", typeof(AnimEnemy))
			},
			{
				29,
				new BrushData(29, "Base/Levels/Level2/Brushes/Enemy/Midground_LV1_P2_Ene_Rock01", typeof(AnimEnemy))
			},
			{
				30,
				new BrushData(30, "Base/Levels/Level2/Brushes/Enemy/Enemy_Empty", typeof(NormalEnemy))
			},
			{
				33,
				new BrushData(33, "Base/Levels/Level2/Brushes/Enemy/Midground_Shinei_chilun_Shun", typeof(AnimEnemy))
			},
			{
				34,
				new BrushData(34, "Base/Levels/Level2/Brushes/Enemy/Midground_Shinei_chilun_Ni", typeof(AnimEnemy))
			},
			{
				35,
				new BrushData(35, "Base/Levels/Level2/Brushes/Enemy/Midground_Shinei_chilun_zhou", typeof(NormalEnemy))
			},
			{
				36,
				new BrushData(36, "Base/Levels/Level2/Brushes/Enemy/Midground_P3_MagicCube_ChiLun_Tile", typeof(AnimEnemy))
			},
			{
				37,
				new BrushData(37, "Base/Levels/Level2/Brushes/Enemy/Midground_P3_MagicCube_ChiLun_Tile1", typeof(AnimEnemy))
			},
			{
				38,
				new BrushData(38, "Base/Levels/Level2/Brushes/Enemy/Enemy_InDoor_Dun", typeof(AnimEnemy))
			},
			{
				39,
				new BrushData(39, "Base/Levels/Level2/Brushes/Enemy/Effect_LV1_TX_LIZI", typeof(EffectEnemy))
			},
			{
				40,
				new BrushData(40, "Base/Levels/Level2/Brushes/Enemy/Effect_LV1_TX_SunShafts", typeof(EffectEnemy))
			},
			{
				41,
				new BrushData(41, "Base/Levels/Level2/Brushes/Enemy/Effect_LV1_TX_LiuSha", typeof(EffectEnemy))
			},
			{
				42,
				new BrushData(42, "Base/Levels/Level2/Brushes/Enemy/Effect_LV1_TX_SunShafts_little", typeof(EffectEnemy))
			},
			{
				43,
				new BrushData(43, "Base/Levels/Level2/Brushes/Enemy/Enemy_InDoor_Abubis", typeof(NormalEnemy))
			},
			{
				44,
				new BrushData(44, "Base/Levels/Level2/Brushes/Enemy/Midground_Redsea_Tree_Ani", typeof(AnimEnemy))
			},
			{
				45,
				new BrushData(45, "Base/Levels/Level2/Brushes/Enemy/AiJi_Redsea_stone01_left", typeof(AnimParticleEnemy))
			},
			{
				47,
				new BrushData(47, "Base/Levels/Level2/Brushes/Enemy/AiJi_Redsea_stone02_left", typeof(AnimParticleEnemy))
			},
			{
				48,
				new BrushData(48, "Base/Levels/Level2/Brushes/Enemy/AiJi_Redsea_stone02_right", typeof(AnimParticleEnemy))
			},
			{
				50,
				new BrushData(50, "Base/Levels/Level2/Brushes/Enemy/AiJi_Redsea_stone03_right", typeof(AnimParticleEnemy))
			},
			{
				51,
				new BrushData(51, "Base/Levels/Level1/Brushes/Enemy/Tile_XiaoZhen_ShiBan01_Off_ON", typeof(RockStarEffect))
			},
			{
				52,
				new BrushData(52, "Base/Levels/Level1/Brushes/Enemy/Tile_XiaoZhen_ShiBan02_Off_ON", typeof(RockStarEffect))
			},
			{
				53,
				new BrushData(53, "Base/Levels/Level1/Brushes/Enemy/Tile_XiaoZhen_ShiBan03_Off_ON", typeof(RockStarEffect))
			},
			{
				54,
				new BrushData(54, "Base/Levels/Level1/Brushes/Enemy/Tile_XiaoZhen_ShiBan04_Off_ON", typeof(RockStarEffect))
			},
			{
				59,
				new BrushData(59, "Base/Levels/Level1/Brushes/Enemy/XiaoZhen_JiaoTangDaMen", typeof(AnimEnemy))
			},
			{
				60,
				new BrushData(60, "Base/Levels/Level1/Brushes/Enemy/XiaoZhen_LuDeng01_ON_Off", typeof(DoorEnemy))
			},
			{
				62,
				new BrushData(62, "Base/Levels/Level1/Brushes/Enemy/Enemy_XiongKong_Star", typeof(SuperStarEnemy))
			},
			{
				63,
				new BrushData(63, "Base/Levels/Level1/Brushes/Enemy/Midground_Star_Down", typeof(AnimEnemy))
			},
			{
				64,
				new BrushData(64, "Base/Levels/Level1/Brushes/Enemy/Tile_DuBai_Star", typeof(AnimEnemy))
			},
			{
				65,
				new BrushData(65, "Base/Levels/Level1/Brushes/Enemy/Tile_XiaoZhen_Star_UP", typeof(AnimEnemy))
			},
			{
				67,
				new BrushData(67, "Base/Levels/Level1/Brushes/Enemy/Effect_Begin_Star", typeof(BeginStarEffect))
			},
			{
				68,
				new BrushData(68, "Base/Levels/Level2/Brushes/Enemy/Award_Diamond_lv2", typeof(DiamondAward))
			},
			{
				69,
				new BrushData(69, "Base/Levels/Level2/Brushes/Enemy/Award_Crown_lv2", typeof(CrownAward))
			},
			{
				70,
				new BrushData(70, "Base/Levels/Level2/Brushes/Enemy/AiJi_Redsea_stone01_right", typeof(AnimParticleEnemy))
			},
			{
				71,
				new BrushData(71, "Base/Levels/Level2/Brushes/Enemy/AiJi_Redsea_stone03_left", typeof(AnimParticleEnemy))
			},
			{
				72,
				new BrushData(72, "Base/Levels/Level2/Brushes/Enemy/Effect_LV2_QTE_Star", typeof(TwoEffectTriggerPro))
			},
			{
				73,
				new BrushData(73, "Base/Levels/Level2/Brushes/Enemy/Effect_LV2_QTE_Star_Road_lv2", typeof(TwoEffectTriggerPro))
			},
			{
				74,
				new BrushData(74, "Base/Levels/Level2/Brushes/Enemy/AiJi_Zhuzi_Ani", typeof(AnimEnemy))
			},
			{
				75,
				new BrushData(75, "Base/Levels/Level2/Brushes/Enemy/AiJi_Zhuzi_Ani_01", typeof(AnimEnemy))
			},
			{
				76,
				new BrushData(76, "Base/Levels/Level1/Brushes/Enemy/XWZ_Effect_Background01", typeof(EffectEnemy))
			},
			{
				77,
				new BrushData(77, "Base/Levels/Level1/Brushes/Enemy/XiaoWangZi_End_star", typeof(AnimEnemyPro))
			},
			{
				78,
				new BrushData(78, "Base/Levels/Level1/Brushes/Enemy/Effect_roomshuai", typeof(DesertGateWay))
			},
			{
				79,
				new BrushData(79, "Base/Levels/Level1/Brushes/Enemy/XiaoWangZi_Enemy_Success", typeof(AnimEnemyPro))
			},
			{
				80,
				new BrushData(80, "Base/Levels/Level1/Brushes/Enemy/XiaoWangZi_Enemy_projector", typeof(AnimEnemyPro))
			},
			{
				81,
				new BrushData(81, "Base/Levels/Level1/Brushes/Enemy/Effect_BigStar_JumpUP", typeof(TwoEffectTriggerPro))
			},
			{
				82,
				new BrushData(82, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Effect_BigStar_JumpUP", typeof(TwoEffectTriggerPro))
			},
			{
				83,
				new BrushData(83, "Base/Levels/Level1/Brushes/Enemy/Effect_BigStar_JumpUP_L", typeof(TwoEffectTriggerPro))
			},
			{
				84,
				new BrushData(84, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Effect_BigStar_JumpUP_L", typeof(TwoEffectTriggerPro))
			},
			{
				91,
				new BrushData(91, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_Tile1", typeof(AnimEnemyPro))
			},
			{
				92,
				new BrushData(92, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_Tile2", typeof(AnimEnemyPro))
			},
			{
				93,
				new BrushData(93, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_Tile_L", typeof(AnimEnemyPro))
			},
			{
				94,
				new BrushData(94, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_Tile_R", typeof(AnimEnemyPro))
			},
			{
				101,
				new BrushData(101, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground01", typeof(NormalEnemy))
			},
			{
				102,
				new BrushData(102, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground02", typeof(NormalEnemy))
			},
			{
				103,
				new BrushData(103, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground03", typeof(NormalEnemy))
			},
			{
				104,
				new BrushData(104, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground04", typeof(NormalEnemy))
			},
			{
				105,
				new BrushData(105, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground05", typeof(NormalEnemy))
			},
			{
				106,
				new BrushData(106, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground06", typeof(NormalEnemy))
			},
			{
				107,
				new BrushData(107, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground07", typeof(NormalEnemy))
			},
			{
				108,
				new BrushData(108, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground08", typeof(NormalEnemy))
			},
			{
				109,
				new BrushData(109, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground09", typeof(NormalEnemy))
			},
			{
				110,
				new BrushData(110, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground10", typeof(NormalEnemy))
			},
			{
				111,
				new BrushData(111, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground11", typeof(NormalEnemy))
			},
			{
				112,
				new BrushData(112, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground12", typeof(NormalEnemy))
			},
			{
				113,
				new BrushData(113, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground13", typeof(NormalEnemy))
			},
			{
				114,
				new BrushData(114, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground14", typeof(NormalEnemy))
			},
			{
				115,
				new BrushData(115, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground15", typeof(NormalEnemy))
			},
			{
				116,
				new BrushData(116, "Base/Levels/Level3/Brushes/Enemy/Home_Enemy_Midground16", typeof(NormalEnemy))
			},
			{
				150,
				new BrushData(150, "Base/Levels/Level1/Brushes/Enemy/XiaoWangZi_Enemy_Road04", typeof(AnimEnemyPro))
			},
			{
				151,
				new BrushData(151, "Base/Levels/Level1/Brushes/Enemy/XiaoWangZi_Enemy_BigStar", typeof(AnimEnemyPro))
			},
			{
				300,
				new BrushData(300, "Base/Levels/Level1/Brushes/Midground/Midground_HongFangZi_Men_Off_Open", typeof(DoorEnemy))
			},
			{
				301,
				new BrushData(301, "Base/Levels/Level1/Brushes/Midground/Midground_HuangFangZi_Men_Off_Open", typeof(DoorEnemy))
			},
			{
				302,
				new BrushData(302, "Base/Levels/Level1/Brushes/Midground/Midground_LanFangZi_ChuangHu_Off_Open", typeof(DoorEnemy))
			},
			{
				303,
				new BrushData(303, "Base/Levels/Level1/Brushes/Midground/Midground_LanFangZi_GeLou_Off_Open", typeof(DoorEnemy))
			},
			{
				304,
				new BrushData(304, "Base/Levels/Level1/Brushes/Midground/Midground_LanFangZi_Men_Off_Open", typeof(DoorEnemy))
			},
			{
				305,
				new BrushData(305, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_chuanghu01_Off_Open", typeof(DoorEnemy))
			},
			{
				306,
				new BrushData(306, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_chuanghu02_Off_Open", typeof(DoorEnemy))
			},
			{
				307,
				new BrushData(307, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_chuanghu03_Off_Open", typeof(DoorEnemy))
			},
			{
				308,
				new BrushData(308, "Base/Levels/Level1/Brushes/Midground/Midground_JiaoTang_DaMen", typeof(AnimEnemyPro))
			},
			{
				309,
				new BrushData(309, "Base/Levels/Level1/Brushes/Tile/Tile_P1_Jump_small_Kong", typeof(AnimEnemyPro))
			},
			{
				310,
				new BrushData(310, "Base/Levels/Level1/Brushes/Tile/Tile_Road_Bug_Small", typeof(AnimEnemyPro))
			},
			{
				311,
				new BrushData(311, "Base/Levels/Level1/Brushes/Tile/Tile_Road_Small_Bug", typeof(AnimEnemyPro))
			},
			{
				312,
				new BrushData(312, "Base/Levels/Level1/Brushes/Midground/Midground_denghai_01", typeof(AnimEnemy))
			},
			{
				313,
				new BrushData(313, "Base/Levels/Level1/Brushes/Midground/Midground_denghai_02", typeof(AnimEnemy))
			},
			{
				314,
				new BrushData(314, "Base/Levels/Level1/Brushes/Midground/Midground_denghai_03", typeof(AnimEnemy))
			},
			{
				315,
				new BrushData(315, "Base/Levels/Level1/Brushes/Enemy/Effect_hongfangzifangzi_a_001", typeof(NormalEnemy))
			},
			{
				316,
				new BrushData(316, "Base/Levels/Level1/Brushes/Enemy/Effect_huangfangzi_c_01", typeof(NormalEnemy))
			},
			{
				317,
				new BrushData(317, "Base/Levels/Level1/Brushes/Enemy/Effect_huangfangzi_c_03", typeof(NormalEnemy))
			},
			{
				318,
				new BrushData(318, "Base/Levels/Level1/Brushes/Enemy/Effect_lanfangzi_d_01", typeof(NormalEnemy))
			},
			{
				319,
				new BrushData(319, "Base/Levels/Level1/Brushes/Enemy/Effect_lanfangzi_d_02", typeof(NormalEnemy))
			},
			{
				320,
				new BrushData(320, "Base/Levels/Level1/Brushes/Enemy/XiaoWangZi_Enemy_Road02", typeof(AnimEnemyPro))
			},
			{
				321,
				new BrushData(321, "Base/Levels/Level1/Brushes/Enemy/Effect_hongfangzifangzi_a_002", typeof(NormalEnemy))
			},
			{
				322,
				new BrushData(322, "Base/Levels/Level1/Brushes/Enemy/Effect_hongfangzifangzi_a_003", typeof(NormalEnemy))
			},
			{
				323,
				new BrushData(323, "Base/Levels/Level1/Brushes/Enemy/XiaoWangZi_Enemy_Road03", typeof(AnimEnemyPro))
			},
			{
				324,
				new BrushData(324, "Base/Levels/Level1/Brushes/Midground/Midground_Yu01", typeof(AnimEnemyPro))
			},
			{
				325,
				new BrushData(325, "Base/Levels/Level1/Brushes/Enemy/Enemy_YanCong01", typeof(NormalEnemy))
			},
			{
				326,
				new BrushData(326, "Base/Levels/Level1/Brushes/Enemy/Enemy_YanCong02", typeof(NormalEnemy))
			},
			{
				327,
				new BrushData(327, "Base/Levels/Level1/Brushes/Enemy/Enemy_YanCong03", typeof(NormalEnemy))
			},
			{
				328,
				new BrushData(328, "Base/Levels/Level1/Brushes/Enemy/XiaoWangZi_Enemy_Road01", typeof(AnimEnemyPro))
			},
			{
				329,
				new BrushData(329, "Base/Levels/Level1/Brushes/Enemy/Effect_JiaoTang_ZhongLou", typeof(NormalEnemy))
			},
			{
				330,
				new BrushData(330, "Base/Levels/Level1/Brushes/Enemy/Effect_JiaoTang_ZhongLou01", typeof(NormalEnemy))
			},
			{
				331,
				new BrushData(331, "Base/Levels/Level1/Brushes/Enemy/Effect_JiaoTang_Qiang01", typeof(NormalEnemy))
			},
			{
				332,
				new BrushData(332, "Base/Levels/Level1/Brushes/Enemy/Tile_DuBai_Star-10", typeof(AnimEnemyPro))
			},
			{
				333,
				new BrushData(333, "Base/Levels/Level1/Brushes/Enemy/Tile_XiaoZhen_Star_Down", typeof(AnimEnemyPro))
			},
			{
				334,
				new BrushData(334, "Base/Levels/Level1/Brushes/Enemy/Effect_JiaoTang_DaMen_QiangBi", typeof(NormalEnemy))
			},
			{
				335,
				new BrushData(335, "Base/Levels/Level1/Brushes/Enemy/Effect_huangfangzi_c_03-15", typeof(NormalEnemy))
			},
			{
				336,
				new BrushData(336, "Base/Levels/Level1/Brushes/Enemy/Effect_huangfangzi_c_03 15", typeof(NormalEnemy))
			},
			{
				337,
				new BrushData(337, "Base/Levels/Level3/Brushes/Enemy/Home_XueBeng_01", typeof(AnimEnemyPro))
			},
			{
				338,
				new BrushData(338, "Base/Levels/Level3/Brushes/Enemy/Home_XueBeng_02", typeof(AnimEnemyPro))
			},
			{
				339,
				new BrushData(339, "Base/Levels/Level3/Brushes/Enemy/Home_ShiZhen_Men", typeof(AnimEnemyPro))
			},
			{
				340,
				new BrushData(340, "Base/Levels/Level3/Brushes/Enemy/Home_diban_b_Up", typeof(AnimEnemyPro))
			},
			{
				341,
				new BrushData(341, "Base/Levels/Level3/Brushes/Enemy/Home_pingtai_02_Up", typeof(AnimEnemyPro))
			},
			{
				342,
				new BrushData(342, "Base/Levels/Level3/Brushes/Enemy/Home_XueBeng_03", typeof(AnimEnemyPro))
			},
			{
				343,
				new BrushData(343, "Base/Levels/Level3/Brushes/Enemy/Home_Yun03_Kai", typeof(AnimEnemyPro))
			},
			{
				344,
				new BrushData(344, "Base/Levels/Level3/Brushes/Enemy/Home_Yun03_Kai01", typeof(AnimEnemyPro))
			},
			{
				345,
				new BrushData(345, "Base/Levels/Level3/Brushes/Enemy/Home_Yun03_Kai02", typeof(AnimEffectShowEnemy))
			},
			{
				346,
				new BrushData(346, "Base/Levels/Level3/Brushes/Enemy/Home_Yun03_Kai03", typeof(AnimEffectShowEnemy))
			},
			{
				347,
				new BrushData(347, "Base/Levels/Level3/Brushes/Enemy/Home_Yun03_Kai04", typeof(AnimEffectShowEnemy))
			},
			{
				349,
				new BrushData(349, "Base/Levels/Level3/Brushes/Enemy/Home_shitou_Up01", typeof(AnimEnemyPro))
			},
			{
				350,
				new BrushData(350, "Base/Levels/Level3/Brushes/Enemy/Home_shitou_Up02", typeof(AnimEnemyPro))
			},
			{
				351,
				new BrushData(351, "Base/Levels/Level3/Brushes/Enemy/Home_shitou_Up03", typeof(AnimEnemyPro))
			},
			{
				352,
				new BrushData(352, "Base/Levels/Level3/Brushes/Enemy/Home_shitou_Up04", typeof(AnimEnemyPro))
			},
			{
				353,
				new BrushData(353, "Base/Levels/Level3/Brushes/Enemy/Home_Yun03_Kai05", typeof(AnimEnemyPro))
			},
			{
				354,
				new BrushData(354, "Base/Levels/Level3/Brushes/Enemy/Crown_Fragment", typeof(CrownFragment))
			},
			{
				355,
				new BrushData(355, "Base/Levels/Level3/Brushes/Enemy/Crown_FromFragment", typeof(CrownFromFragment))
			},
			{
				356,
				new BrushData(356, "Base/Levels/Level3/Brushes/Enemy/Diamond_Fragment", typeof(DiamondFragment))
			},
			{
				357,
				new BrushData(357, "Base/Levels/Level3/Brushes/Enemy/Diamond_FromFragment", typeof(DiamondFromFragment))
			},
			{
				358,
				new BrushData(358, "Base/Levels/Level3/Brushes/Enemy/Home_huaxue_Down01", typeof(AnimEnemyPro))
			},
			{
				359,
				new BrushData(359, "Base/Levels/Level3/Brushes/Enemy/Home_huaxue_Down02", typeof(AnimEnemyPro))
			},
			{
				360,
				new BrushData(360, "Base/Levels/Level3/Brushes/Enemy/Home_huaxue_Down03", typeof(AnimEnemyPro))
			},
			{
				361,
				new BrushData(361, "Base/Levels/Level3/Brushes/Enemy/Home_Yun03_Kai06", typeof(AnimEnemyPro))
			},
			{
				362,
				new BrushData(362, "Base/Levels/Level3/Brushes/Enemy/Home_Yun03_Kai03_1", typeof(AnimEffectShowEnemy))
			},
			{
				363,
				new BrushData(363, "Base/Levels/Level3/Brushes/Enemy/Home_guandishizhu_00_R", typeof(AnimEnemyPro))
			},
			{
				364,
				new BrushData(364, "Base/Levels/Level3/Brushes/Enemy/Home_YinDao_TiaoDian", typeof(AnimEnemyPro))
			},
			{
				365,
				new BrushData(365, "Base/Levels/Level3/Brushes/Enemy/Effect_Jump_UP", typeof(TwoEffectTriggerPro))
			},
			{
				366,
				new BrushData(366, "Base/Levels/Level3/Brushes/Enemy/BaiRiMeng_Yu01", typeof(PathToMoveEnemy))
			},
			{
				367,
				new BrushData(367, "Base/Levels/Level3/Brushes/Enemy/HOME_ShiZhu_KaiChang", typeof(MoveAllEnemy))
			},
			{
				368,
				new BrushData(368, "Base/Levels/Level3/Brushes/Enemy/HOME_FuHao01", typeof(ChangeEmissionTrigger))
			},
			{
				369,
				new BrushData(369, "Base/Levels/Level3/Brushes/Enemy/Home_HuaKai", typeof(AnimEnemyPro))
			},
			{
				370,
				new BrushData(370, "Base/Levels/Level3/Brushes/Enemy/Effect_jiantou01", typeof(PathToMoveEffect))
			},
			{
				371,
				new BrushData(371, "Base/Levels/Level3/Brushes/Enemy/HOME_ShiBan_Down", typeof(CollidePerformTrigger))
			},
			{
				372,
				new BrushData(372, "Base/Levels/Level3/Brushes/Enemy/HOME_ShiBan_UP", typeof(AnimEnemyPro))
			},
			{
				373,
				new BrushData(373, "Base/Levels/Level3/Brushes/Enemy/Home_yuanpan_ALL_01", typeof(InscriptionStoneEffect))
			},
			{
				374,
				new BrushData(374, "Base/Levels/Level3/Brushes/Enemy/Home_yuanpan_ALL_02", typeof(InscriptionStoneEffect))
			},
			{
				375,
				new BrushData(375, "Base/Levels/Level3/Brushes/Enemy/Home_yuanpan_ALL_03", typeof(InscriptionStoneEffect))
			},
			{
				376,
				new BrushData(376, "Base/Levels/Level3/Brushes/Enemy/Home_yuanpan_ALL_04", typeof(InscriptionStoneEffect))
			},
			{
				377,
				new BrushData(377, "Base/Levels/Level3/Brushes/Enemy/Home_yuanpan_ALL_05", typeof(InscriptionStoneEffect))
			},
			{
				378,
				new BrushData(378, "Base/Levels/Level3/Brushes/Enemy/Enemy_Empty_Kong", typeof(NormalEnemy))
			},
			{
				379,
				new BrushData(379, "Base/Levels/Level3/Brushes/Enemy/Enemy_Bird_03", typeof(PathToMoveEnemy))
			},
			{
				380,
				new BrushData(380, "Base/Levels/Level3/Brushes/Enemy/Enemy_DissolvePillar", typeof(DissolveEnemy))
			},
			{
				381,
				new BrushData(381, "Base/Levels/Level3/Brushes/Enemy/Enemy_FengXiao_01", typeof(AnimEnemyPro))
			},
			{
				382,
				new BrushData(382, "Base/Levels/Level3/Brushes/Enemy/Home_shizhu_FuWen", typeof(AnimEnemyPro))
			},
			{
				383,
				new BrushData(383, "Base/Levels/Level3/Brushes/Enemy/Home_ShiZhu_Up", typeof(AnimEnemyPro))
			},
			{
				384,
				new BrushData(384, "Base/Levels/Level3/Brushes/Enemy/Home_ShiZhu_Down", typeof(AnimEnemyPro))
			},
			{
				385,
				new BrushData(385, "Base/Levels/Level3/Brushes/Enemy/Home_ShiQiao_Down01", typeof(CollidePerformTrigger))
			},
			{
				386,
				new BrushData(386, "Base/Levels/Level3/Brushes/Enemy/Home_ShiQiao_Up01", typeof(AnimEnemyPro))
			},
			{
				387,
				new BrushData(387, "Base/Levels/Level3/Brushes/Enemy/Home_ShiQiao_Up02", typeof(AnimEnemyPro))
			},
			{
				388,
				new BrushData(388, "Base/Levels/Level3/Brushes/Enemy/Home_ShiQiao_Down02", typeof(CollidePerformTrigger))
			},
			{
				389,
				new BrushData(389, "Base/Levels/Level3/Brushes/Enemy/Home_ShiQiao_Down03", typeof(CollidePerformTrigger))
			},
			{
				390,
				new BrushData(390, "Base/Levels/Level3/Brushes/Enemy/Home_ShiQiao_Down04", typeof(CollidePerformTrigger))
			},
			{
				391,
				new BrushData(391, "Base/Levels/Level3/Brushes/Enemy/Home_guandishizhu_01_R", typeof(AnimEnemyPro))
			},
			{
				392,
				new BrushData(392, "Base/Levels/Level3/Brushes/Enemy/Home_guandishizhu_02_R", typeof(AnimEnemyPro))
			},
			{
				393,
				new BrushData(393, "Base/Levels/Level3/Brushes/Enemy/Home_GuangPian_01", typeof(AnimEnemy))
			},
			{
				394,
				new BrushData(394, "Base/Levels/Level3/Brushes/Enemy/Home_yuanpan_ALL_00", typeof(InscriptionStoneEffect))
			},
			{
				395,
				new BrushData(395, "Base/Levels/Level3/Brushes/Enemy/Enemy_PuGongYing", typeof(DandelionEnemy))
			},
			{
				396,
				new BrushData(396, "Base/Levels/Level3/Brushes/Enemy/Effect_fengxue", typeof(EffectEnemy))
			},
			{
				397,
				new BrushData(397, "Base/Levels/Level3/Brushes/Enemy/Effect_LV3_QTE_new", typeof(AnimParticleEffect))
			},
			{
				398,
				new BrushData(398, "Base/Levels/Level3/Brushes/Enemy/Effect_Jump_UP_LV3", typeof(TwoEffectTriggerPro))
			},
			{
				399,
				new BrushData(399, "Base/Levels/Level3/Brushes/Enemy/Home_yuanpan_Templete", typeof(InscriptionStoneEffect))
			},
			{
				400,
				new BrushData(400, "Base/Levels/Level3/Brushes/Enemy/Home_guandishizhu_01_An", typeof(AnimEnemyPro))
			},
			{
				401,
				new BrushData(401, "Base/Levels/Level3/Brushes/Enemy/EnemyRock", typeof(PathToMoveEffect))
			},
			{
				402,
				new BrushData(402, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road02_easy", typeof(AnimEnemyPro))
			},
			{
				403,
				new BrushData(403, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road03_easy", typeof(AnimEnemyPro))
			},
			{
				404,
				new BrushData(404, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road04_easy", typeof(AnimEnemyPro))
			},
			{
				405,
				new BrushData(405, "Base/Levels/Level5/Brushes/Enemy/Fate_QiPan_blackmove", typeof(MoveAllEnemy))
			},
			{
				406,
				new BrushData(406, "Base/Levels/Level5/Brushes/Enemy/Fate_QiPan_blackmove0", typeof(MoveAllEnemy))
			},
			{
				407,
				new BrushData(407, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_QiPan_qizi_MoveThree", typeof(MoveThreeEnemy))
			},
			{
				408,
				new BrushData(408, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road01_easy", typeof(AnimEnemyPro))
			},
			{
				409,
				new BrushData(409, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_black_Empty", typeof(NormalEnemy))
			},
			{
				410,
				new BrushData(410, "Base/Levels/Level5/Brushes/Enemy/Fate_Hand_E_04", typeof(AnimEnemyPro))
			},
			{
				411,
				new BrushData(411, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_black_up_L", typeof(AnimEnemyPro))
			},
			{
				412,
				new BrushData(412, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_black_up_R", typeof(AnimEnemyPro))
			},
			{
				413,
				new BrushData(413, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_black_nor_L", typeof(AnimEnemyPro))
			},
			{
				414,
				new BrushData(414, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_black_nor_R", typeof(AnimEnemyPro))
			},
			{
				415,
				new BrushData(415, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_black_down_L", typeof(AnimEnemyPro))
			},
			{
				416,
				new BrushData(416, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_black_down_R", typeof(AnimEnemyPro))
			},
			{
				417,
				new BrushData(417, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_hand_L", typeof(AnimEnemyPro))
			},
			{
				418,
				new BrushData(418, "Base/Levels/Level5/Brushes/Enemy/Fate_Piano_anim01", typeof(AnimEnemyPro))
			},
			{
				419,
				new BrushData(419, "Base/Levels/Level5/Brushes/Enemy/Tutorial_Line", typeof(AnimEnemyPro))
			},
			{
				420,
				new BrushData(420, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_hand_zhua", typeof(AnimEnemyPro))
			},
			{
				421,
				new BrushData(421, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_360_white", typeof(AnimEnemyPro))
			},
			{
				422,
				new BrushData(422, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_360_black01", typeof(AnimEnemyPro))
			},
			{
				423,
				new BrushData(423, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_360_black02", typeof(AnimEnemyPro))
			},
			{
				424,
				new BrushData(424, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_360_black03", typeof(AnimEnemyPro))
			},
			{
				425,
				new BrushData(425, "Base/Levels/Level5/Brushes/Enemy/Fate_Effect_muou", typeof(EffectEnemy))
			},
			{
				426,
				new BrushData(426, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_SuperJump", typeof(JumpDistanceTrigger))
			},
			{
				427,
				new BrushData(427, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_HandStart", typeof(AnimEnemyPlayByEvnet))
			},
			{
				428,
				new BrushData(428, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_QiPan_qizi01_MoveThree", typeof(MoveThreeEnemy))
			},
			{
				429,
				new BrushData(429, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_QiPan_qizi01_Move", typeof(MoveAllEnemy))
			},
			{
				430,
				new BrushData(430, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_QiPan_qizi02_Move", typeof(MoveAllEnemy))
			},
			{
				431,
				new BrushData(431, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_xiaqi_hand", typeof(AnimEnemyPro))
			},
			{
				432,
				new BrushData(432, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_xiaqi_KingB", typeof(AnimEnemyPro))
			},
			{
				433,
				new BrushData(433, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_xiaqi_KnightB2", typeof(AnimEnemyPro))
			},
			{
				434,
				new BrushData(434, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_xiaqi_QueenB", typeof(AnimEnemyPro))
			},
			{
				435,
				new BrushData(435, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_QiPan_black_MoveThree", typeof(MoveThreeEnemy))
			},
			{
				436,
				new BrushData(436, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_End_Wuzhe", typeof(AnimEnemyPro))
			},
			{
				437,
				new BrushData(437, "Base/Levels/Level5/Brushes/Enemy/Fate_Hand_E_03", typeof(AnimEnemyPro))
			},
			{
				438,
				new BrushData(438, "Base/Levels/Level5/Brushes/Enemy/Fate_Effect_jiewei_glow", typeof(EffectEnemy))
			},
			{
				439,
				new BrushData(439, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_TiXianShou", typeof(AnimEnemyPro))
			},
			{
				440,
				new BrushData(440, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Mubu", typeof(AnimEnemyPro))
			},
			{
				441,
				new BrushData(441, "Base/Levels/Level5/Brushes/Enemy/Fate_ChiLunZu_anim01", typeof(AnimEnemyPro))
			},
			{
				442,
				new BrushData(442, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_TiXianShou_Tutorial", typeof(AnimEnemyPro))
			},
			{
				443,
				new BrushData(443, "Base/Levels/Level5/Brushes/Enemy/Award_Crown_Tutorial", typeof(CrownAward))
			},
			{
				444,
				new BrushData(444, "Base/Levels/Level5/Brushes/Enemy/Award_Diamond_Tutorial", typeof(DiamondAward))
			},
			{
				445,
				new BrushData(445, "Base/Levels/Level5/Brushes/Enemy/Fate_Effect_jiewei_glow_Tutorial", typeof(EffectEnemy))
			},
			{
				446,
				new BrushData(446, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_End_clock_Tutorial", typeof(AnimEnemyPro))
			},
			{
				447,
				new BrushData(447, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_chui_anim01", typeof(AnimEnemyPro))
			},
			{
				448,
				new BrushData(448, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_muou_Q-1", typeof(AnimEnemyPro))
			},
			{
				449,
				new BrushData(449, "Base/Levels/Level5/Brushes/Enemy/Enemy_bianshen1_left", typeof(AnimEnemyPro))
			},
			{
				450,
				new BrushData(450, "Base/Levels/Level5/Brushes/Enemy/Enemy_bianshen1_right", typeof(AnimEnemyPro))
			},
			{
				451,
				new BrushData(451, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin01_Tile", typeof(AnimEnemyPro))
			},
			{
				452,
				new BrushData(452, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin01", typeof(AnimEnemyPro))
			},
			{
				453,
				new BrushData(453, "Base/Levels/Level5/Brushes/Enemy/Fate_Wutai_up01", typeof(AnimParticleEnemy))
			},
			{
				454,
				new BrushData(454, "Base/Levels/Level5/Brushes/Enemy/Fate_Wutai_up02", typeof(AnimEnemyPro))
			},
			{
				455,
				new BrushData(455, "Base/Levels/Level5/Brushes/Enemy/Enemy_Wuzhequn_03_easy", typeof(AnimEnemyPro))
			},
			{
				456,
				new BrushData(456, "Base/Levels/Level5/Brushes/Enemy/Enemy_Wuzhequn_03", typeof(AnimEnemyPro))
			},
			{
				457,
				new BrushData(457, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Background_effect01", typeof(EffectEnemy))
			},
			{
				458,
				new BrushData(458, "Base/Levels/Level5/Brushes/Enemy/Fate_ChiLunZu_anim03", typeof(AnimEnemyPro))
			},
			{
				459,
				new BrushData(459, "Base/Levels/Level5/Brushes/Enemy/Enemy_Wuzhe_01", typeof(AnimEnemyPro))
			},
			{
				460,
				new BrushData(460, "Base/Levels/Level5/Brushes/Enemy/Fate_End_Hand", typeof(AnimEnemyPro))
			},
			{
				461,
				new BrushData(461, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin01", typeof(AnimEnemyPro))
			},
			{
				462,
				new BrushData(462, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin02", typeof(AnimEnemyPro))
			},
			{
				463,
				new BrushData(463, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin03", typeof(AnimEnemyPro))
			},
			{
				464,
				new BrushData(464, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin04", typeof(AnimEnemyPro))
			},
			{
				465,
				new BrushData(465, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin03_Tile", typeof(AnimEnemyPro))
			},
			{
				466,
				new BrushData(466, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin06", typeof(AnimParticleEnemy))
			},
			{
				467,
				new BrushData(467, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_blackmove", typeof(MoveAllEnemy))
			},
			{
				468,
				new BrushData(468, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Jump", typeof(AnimEnemyPro))
			},
			{
				469,
				new BrushData(469, "Base/Levels/Level5/Brushes/Enemy/Fate_ChiLunZu_anim02", typeof(AnimEnemyPro))
			},
			{
				470,
				new BrushData(470, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_muou_Q", typeof(AnimEnemyPro))
			},
			{
				471,
				new BrushData(471, "Base/Levels/Level5/Brushes/Enemy/Enemy_Wuzhe_02", typeof(AnimEnemyPro))
			},
			{
				472,
				new BrushData(472, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Background_effect02", typeof(EffectEnemy))
			},
			{
				473,
				new BrushData(473, "Base/Levels/Level5/Brushes/Enemy/Fate_Hand_E_02", typeof(AnimEnemyPro))
			},
			{
				474,
				new BrushData(474, "Base/Levels/Level5/Brushes/Enemy/Fate_ChiLunZu_anim01_end", typeof(AnimEnemyPro))
			},
			{
				475,
				new BrushData(475, "Base/Levels/Level5/Brushes/Enemy/Enemy_bianshen", typeof(AnimEnemyPro))
			},
			{
				476,
				new BrushData(476, "Base/Levels/Level5/Brushes/Enemy/Fate_EndDoor_anim01", typeof(AnimEnemyPro))
			},
			{
				477,
				new BrushData(477, "Base/Levels/Level5/Brushes/Enemy/Enemy_Wuzhe_05", typeof(AnimEnemyPro))
			},
			{
				478,
				new BrushData(478, "Base/Levels/Level5/Brushes/Enemy/Fate_ChiLunZu_anim05", typeof(AnimEnemyPro))
			},
			{
				479,
				new BrushData(479, "Base/Levels/Level5/Brushes/Enemy/Enemy_Wuzhe_02_s", typeof(AnimEnemyPro))
			},
			{
				480,
				new BrushData(480, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_xiaqi_KingB_shadow", typeof(EffectEnemy))
			},
			{
				481,
				new BrushData(481, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road01", typeof(AnimEnemyPro))
			},
			{
				482,
				new BrushData(482, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road02", typeof(AnimEnemyPro))
			},
			{
				483,
				new BrushData(483, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road03", typeof(AnimEnemyPro))
			},
			{
				484,
				new BrushData(484, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road04", typeof(AnimEnemyPro))
			},
			{
				485,
				new BrushData(485, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin06_noshadow", typeof(AnimParticleEnemy))
			},
			{
				486,
				new BrushData(486, "Base/Levels/Level5/Brushes/Enemy/Fate_Hand_E_01", typeof(AnimEnemyPro))
			},
			{
				487,
				new BrushData(487, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road02_moreeasy", typeof(AnimEnemyPro))
			},
			{
				488,
				new BrushData(488, "Base/Levels/Level5/Brushes/Enemy/Enemy_Wuzhe_13_s", typeof(AnimEnemyPro))
			},
			{
				489,
				new BrushData(489, "Base/Levels/Level5/Brushes/Enemy/Enemy_Wuzhe_13", typeof(AnimEnemyPro))
			},
			{
				490,
				new BrushData(490, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_guan_anim01", typeof(AnimEnemyPro))
			},
			{
				491,
				new BrushData(491, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_chilun_anim01", typeof(AnimEnemyPro))
			},
			{
				492,
				new BrushData(492, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Hand_xiaqi", typeof(AnimEnemyPro))
			},
			{
				493,
				new BrushData(493, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_End_clock", typeof(AnimParticleEnemy))
			},
			{
				494,
				new BrushData(494, "Base/Levels/Level5/Brushes/Enemy/Fate_RoleLineTrigger", typeof(RoleLineTrigger))
			},
			{
				495,
				new BrushData(495, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin04_Easy", typeof(AnimEnemyPro))
			},
			{
				496,
				new BrushData(496, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin03_Tile_Easy", typeof(AnimEnemyPro))
			},
			{
				497,
				new BrushData(497, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin02_Easy", typeof(AnimEnemyPro))
			},
			{
				498,
				new BrushData(498, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_Tanqin01_Tile_Easy", typeof(AnimEnemyPro))
			},
			{
				499,
				new BrushData(499, "Base/Levels/Level5/Brushes/Tile/Tile_SuperJump", typeof(JumpDistanceTrigger))
			},
			{
				500,
				new BrushData(500, "Base/Levels/Level3/Brushes/Tile/Auto_Tile_SuperJump", typeof(JumpDistanceTrigger))
			},
			{
				501,
				new BrushData(501, "Base/Levels/LightMap/Brush/Enemy/Effect_ShaMo_Smoke", typeof(EffectEnemy))
			},
			{
				502,
				new BrushData(502, "Base/Levels/LightMap/Brush/Enemy/Enemy_ShaMo_Abubis", typeof(AnimParticleEnemy))
			},
			{
				503,
				new BrushData(503, "Base/Levels/LightMap/Brush/Enemy/Aiji_Muban_fei", typeof(AnimEnemyPro))
			},
			{
				504,
				new BrushData(504, "Base/Levels/LightMap/Brush/Enemy/Enemy_GongDi_Zhuzi01", typeof(AnimParticleEnemy))
			},
			{
				505,
				new BrushData(505, "Base/Levels/Level5/Brushes/Enemy/Enemy_Wuzhequn_03_moreeasy", typeof(AnimEnemyPro))
			},
			{
				506,
				new BrushData(506, "Base/Levels/Level5/Brushes/Enemy/Fate_Pinao_road03_moreeasy", typeof(AnimEnemyPro))
			},
			{
				507,
				new BrushData(507, "Base/Levels/LightMap/Brush/Enemy/Enemy_Desert_JingJi01_hard1", typeof(AnimEnemy))
			},
			{
				510,
				new BrushData(510, "Base/Levels/LightMap/Brush/Enemy/Enemy_GongDi_JingJi01", typeof(AnimEnemy))
			},
			{
				512,
				new BrushData(512, "Base/Levels/LightMap/Brush/Enemy/Effect_Liushapubu", typeof(EffectEnemy))
			},
			{
				513,
				new BrushData(513, "Base/Levels/LightMap/Brush/Enemy/Enemy_PathToMove", typeof(PathToMoveEnemy))
			},
			{
				514,
				new BrushData(514, "Base/Levels/LightMap/Brush/Enemy/Effect_liushapubu_shinei", typeof(EffectEnemy))
			},
			{
				515,
				new BrushData(515, "Base/Levels/LightMap/Brush/Enemy/AiJi_Muban_2", typeof(AnimParticleEnemy))
			},
			{
				516,
				new BrushData(516, "Base/Levels/LightMap/Brush/Enemy/AiJi_Muban_3", typeof(AnimParticleEnemy))
			},
			{
				518,
				new BrushData(518, "Base/Levels/LightMap/Brush/Enemy/Effect_Jump_UP", typeof(TwoEffectTriggerPro))
			},
			{
				520,
				new BrushData(520, "Base/Levels/LightMap/Brush/Enemy/Effect_xianjieliusha_1", typeof(EffectEnemy))
			},
			{
				521,
				new BrushData(521, "Base/Levels/LightMap/Brush/Enemy/Enemy_GongDi_Zhuzi02_normal", typeof(AnimParticleEnemy))
			},
			{
				525,
				new BrushData(525, "Base/Levels/LightMap/Brush/Enemy/Effect_FengSha_da", typeof(EffectEnemy))
			},
			{
				526,
				new BrushData(526, "Base/Levels/LightMap/Brush/Enemy/Effect_liusha_xiao", typeof(EffectEnemy))
			},
			{
				527,
				new BrushData(527, "Base/Levels/LightMap/Brush/Enemy/Enemy_GongDi_Zhuzi02", typeof(AnimParticleEnemy))
			},
			{
				528,
				new BrushData(528, "Base/Levels/LightMap/Brush/Enemy/Enemy_ShenDian_Dun", typeof(AnimEnemy))
			},
			{
				529,
				new BrushData(529, "Base/Levels/LightMap/Brush/Enemy/AiJi_XiaPo_Zhu_down", typeof(AnimEnemyPro))
			},
			{
				531,
				new BrushData(531, "Base/Levels/LightMap/Brush/Enemy/AiJi_XiaPo_Zhu", typeof(NormalEnemy))
			},
			{
				532,
				new BrushData(532, "Base/Levels/LightMap/Brush/Enemy/Enemy_GongDi_Zhuzi03", typeof(NormalEnemy))
			},
			{
				533,
				new BrushData(533, "Base/Levels/LightMap/Brush/Enemy/Enemy_GongDi_huopen", typeof(EffectEnemy))
			},
			{
				534,
				new BrushData(534, "Base/Levels/LightMap/Brush/Enemy/Enemy_Anubis_End", typeof(AnimParticleEnemy))
			},
			{
				535,
				new BrushData(535, "Base/Levels/LightMap/Brush/Enemy/Enemy_Desert_JingJi01", typeof(AnimEnemy))
			},
			{
				536,
				new BrushData(536, "Base/Levels/LightMap/Brush/Enemy/AiJi_Chuansongmen", typeof(DesertGateWay))
			},
			{
				537,
				new BrushData(537, "Base/Levels/LightMap/Brush/Enemy/Enemy_DiaoXiang", typeof(AnimEnemy))
			},
			{
				538,
				new BrushData(538, "Base/Levels/LightMap/Brush/Enemy/Enemy_Rock", typeof(AnimEnemy))
			},
			{
				539,
				new BrushData(539, "Base/Levels/LightMap/Brush/Enemy/Award_Diamond_lv4", typeof(DiamondAward))
			},
			{
				540,
				new BrushData(540, "Base/Levels/LightMap/Brush/Enemy/Award_Crown_lv4", typeof(CrownAward))
			},
			{
				541,
				new BrushData(541, "Base/Levels/LightMap/Brush/Enemy/Enemy_Enpty", typeof(NormalEnemy))
			},
			{
				542,
				new BrushData(542, "Base/Levels/LightMap/Brush/Enemy/Effect_Anubis_End", typeof(EffectEnemy))
			},
			{
				543,
				new BrushData(543, "Base/Levels/LightMap/Brush/Enemy/Effect_Men", typeof(EffectEnemy))
			},
			{
				544,
				new BrushData(544, "Base/Levels/LightMap/Brush/Enemy/Enemy_PathToMove1", typeof(PathToMoveEnemy))
			},
			{
				545,
				new BrushData(545, "Base/Levels/LightMap/Brush/Enemy/Effect_SunShafts_little", typeof(EffectEnemy))
			},
			{
				546,
				new BrushData(546, "Base/Levels/LightMap/Brush/Enemy/Effect_SunShafts_big", typeof(EffectEnemy))
			},
			{
				547,
				new BrushData(547, "Base/Levels/LightMap/Brush/Enemy/DropTrigger", typeof(NormalDropEnemy))
			},
			{
				548,
				new BrushData(548, "Base/Levels/LightMap/Brush/Enemy/Effect_luoshi_liusha", typeof(EffectEnemy))
			},
			{
				549,
				new BrushData(549, "Base/Levels/LightMap/Brush/Enemy/AiJi_jiewei_Effect", typeof(AnimEnemyPro))
			},
			{
				550,
				new BrushData(550, "Base/Levels/LightMap/Brush/Enemy/tongyong_kaichang", typeof(DesertGateWay))
			},
			{
				555,
				new BrushData(555, "Base/Levels/LightMap/Brush/Enemy/tongyong_jiewei", typeof(AnimEnemy))
			},
			{
				556,
				new BrushData(556, "Base/Levels/LightMap/Brush/Enemy/AiJi_Muban_4", typeof(AnimParticleEnemy))
			},
			{
				557,
				new BrushData(557, "Base/Levels/LightMap/Brush/Enemy/FreeMoveJump_Pharaohs", typeof(MoveAllEnemy))
			},
			{
				558,
				new BrushData(558, "Base/Levels/LightMap/Brush/Enemy/Tile_shendian_Anim01", typeof(AnimEnemyPro))
			},
			{
				559,
				new BrushData(559, "Base/Levels/LightMap/Brush/Enemy/Tile_shendian_Anim02", typeof(AnimEnemyPro))
			},
			{
				560,
				new BrushData(560, "Base/Levels/LightMap/Brush/Enemy/Tile_shendian_Anim03", typeof(AnimEnemyPro))
			},
			{
				561,
				new BrushData(561, "Base/Levels/LightMap/Brush/Enemy/Tile_shendian_Anim04", typeof(AnimEnemyPro))
			},
			{
				641,
				new BrushData(641, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_01_easy", typeof(AnimEnemyPro))
			},
			{
				642,
				new BrushData(642, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_02_easy", typeof(AnimEnemyPro))
			},
			{
				643,
				new BrushData(643, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_03_easy", typeof(AnimEnemyPro))
			},
			{
				644,
				new BrushData(644, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_04_easy", typeof(AnimEnemyPro))
			},
			{
				645,
				new BrushData(645, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_05_easy", typeof(AnimEnemyPro))
			},
			{
				648,
				new BrushData(648, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_03_normal", typeof(AnimEnemyPro))
			},
			{
				650,
				new BrushData(650, "Base/Levels/Level3/Brushes/Enemy/Home_dizhuan_002", typeof(AnimEnemyPro))
			},
			{
				651,
				new BrushData(651, "Base/Levels/Level3/Brushes/Enemy/Home_xuedi_01", typeof(AnimParticleEnemy))
			},
			{
				652,
				new BrushData(652, "Base/Levels/Level3/Brushes/Enemy/Home_xuedi_02", typeof(AnimParticleEnemy))
			},
			{
				653,
				new BrushData(653, "Base/Levels/Level3/Brushes/Enemy/Home_xuedi_03", typeof(AnimParticleEnemy))
			},
			{
				654,
				new BrushData(654, "Base/Levels/Level3/Brushes/Enemy/Home_dizhuan_003", typeof(AnimEnemyPro))
			},
			{
				655,
				new BrushData(655, "Base/Levels/Level3/Brushes/Enemy/Home_dizhuan_004", typeof(AnimEnemyPro))
			},
			{
				656,
				new BrushData(656, "Base/Levels/Level3/Brushes/Enemy/Home_dizhuan_005", typeof(AnimEnemyPro))
			},
			{
				657,
				new BrushData(657, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_02", typeof(AnimEnemyPro))
			},
			{
				658,
				new BrushData(658, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_01", typeof(AnimEnemyPro))
			},
			{
				659,
				new BrushData(659, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_03", typeof(AnimEnemyPro))
			},
			{
				660,
				new BrushData(660, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_04", typeof(AnimEnemyPro))
			},
			{
				661,
				new BrushData(661, "Base/Levels/Level3/Brushes/Enemy/Home_shipan_05", typeof(AnimEnemyPro))
			},
			{
				702,
				new BrushData(702, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_Gong01", typeof(AnimEnemyPro))
			},
			{
				703,
				new BrushData(703, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_PianoBallani01_left", typeof(AnimEnemyPro))
			},
			{
				705,
				new BrushData(705, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_PianoBallani02_left", typeof(AnimEnemyPro))
			},
			{
				706,
				new BrushData(706, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_PianoBallani02_easy_left", typeof(AnimEnemyPro))
			},
			{
				707,
				new BrushData(707, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_Ballani02", typeof(AnimEnemyPro))
			},
			{
				708,
				new BrushData(708, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_LaBa_left", typeof(AnimEnemyPro))
			},
			{
				709,
				new BrushData(709, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_LaBa_right", typeof(AnimEnemyPro))
			},
			{
				710,
				new BrushData(710, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Effect_Jump", typeof(TwoEffectTriggerPro))
			},
			{
				711,
				new BrushData(711, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_1to3", typeof(AnimEnemyPro))
			},
			{
				712,
				new BrushData(712, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_3to1", typeof(AnimEnemyPro))
			},
			{
				713,
				new BrushData(713, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_1to2", typeof(AnimEnemyPro))
			},
			{
				714,
				new BrushData(714, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_3to2", typeof(AnimEnemyPro))
			},
			{
				716,
				new BrushData(716, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_2to1", typeof(AnimEnemyPro))
			},
			{
				717,
				new BrushData(717, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_2to3", typeof(AnimEnemyPro))
			},
			{
				718,
				new BrushData(718, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_End_yueqi", typeof(AnimEnemyPro))
			},
			{
				719,
				new BrushData(719, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Effect_Jump_Big", typeof(TwoEffectTriggerPro))
			},
			{
				720,
				new BrushData(720, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_Empty", typeof(NormalEnemy))
			},
			{
				721,
				new BrushData(721, "Base/Levels/Waltz/Brushes/Enemy/Waltz_End_Violin_01", typeof(AnimEnemyPro))
			},
			{
				722,
				new BrushData(722, "Base/Levels/Waltz/Brushes/Enemy/Waltz_End_Violin_02", typeof(AnimEnemyPro))
			},
			{
				723,
				new BrushData(723, "Base/Levels/Waltz/Brushes/Enemy/Waltz_End_Violin_03", typeof(AnimEnemyPro))
			},
			{
				725,
				new BrushData(725, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_Paper_yes", typeof(AnimEnemyPro))
			},
			{
				726,
				new BrushData(726, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_Paper_no", typeof(AnimEnemyPro))
			},
			{
				727,
				new BrushData(727, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_Piano_SuperJump", typeof(AnimEnemyPro))
			},
			{
				728,
				new BrushData(728, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_Qinbing", typeof(NormalEnemy))
			},
			{
				729,
				new BrushData(729, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_jiguang", typeof(AnimEnemyPro))
			},
			{
				731,
				new BrushData(731, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_Chaani_big_left", typeof(AnimEnemyPro))
			},
			{
				732,
				new BrushData(732, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_Chaani_big_right", typeof(AnimEnemyPro))
			},
			{
				733,
				new BrushData(733, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_zhiluo", typeof(AnimEnemyPro))
			},
			{
				734,
				new BrushData(734, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_toL1", typeof(AnimEnemyPro))
			},
			{
				735,
				new BrushData(735, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_toR1", typeof(AnimEnemyPro))
			},
			{
				736,
				new BrushData(736, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_toF1", typeof(AnimEnemyPro))
			},
			{
				737,
				new BrushData(737, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_toF2", typeof(AnimEnemyPro))
			},
			{
				738,
				new BrushData(738, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_toB1", typeof(AnimEnemyPro))
			},
			{
				739,
				new BrushData(739, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Tile_toB2", typeof(AnimEnemyPro))
			},
			{
				740,
				new BrushData(740, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad03_normal_right", typeof(AnimEnemyPro))
			},
			{
				741,
				new BrushData(741, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad04_normal_right", typeof(AnimEnemyPro))
			},
			{
				742,
				new BrushData(742, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoLujing02_normal", typeof(AnimEnemyPro))
			},
			{
				743,
				new BrushData(743, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad03_normal", typeof(AnimEnemyPro))
			},
			{
				744,
				new BrushData(744, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad04_normal", typeof(AnimEnemyPro))
			},
			{
				745,
				new BrushData(745, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoLujing01_normal", typeof(AnimEnemyPro))
			},
			{
				747,
				new BrushData(747, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_Jiguang", typeof(AnimEnemyPro))
			},
			{
				748,
				new BrushData(748, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad03_easy", typeof(AnimEnemyPro))
			},
			{
				749,
				new BrushData(749, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad03_easy_right", typeof(AnimEnemyPro))
			},
			{
				750,
				new BrushData(750, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_zhidai_01", typeof(AnimEnemyPro))
			},
			{
				760,
				new BrushData(760, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad03_right", typeof(AnimEnemyPro))
			},
			{
				761,
				new BrushData(761, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad04_right", typeof(AnimEnemyPro))
			},
			{
				762,
				new BrushData(762, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoLujing02", typeof(AnimEnemyPro))
			},
			{
				763,
				new BrushData(763, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad03", typeof(AnimEnemyPro))
			},
			{
				764,
				new BrushData(764, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoRoad04", typeof(AnimEnemyPro))
			},
			{
				765,
				new BrushData(765, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_PianoLujing01", typeof(AnimEnemyPro))
			},
			{
				766,
				new BrushData(766, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Enemy_mofabang", typeof(AnimParticleEnemy))
			},
			{
				783,
				new BrushData(783, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Effect_Zhihuibang", typeof(EffectEnemy))
			},
			{
				784,
				new BrushData(784, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_Effect03", typeof(EffectEnemy))
			},
			{
				785,
				new BrushData(785, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Effect_Crown", typeof(TwoEffectTriggerPro))
			},
			{
				787,
				new BrushData(787, "Base/Levels/Waltz/Brushes/Enemy/Waltz_effect_Background02", typeof(EffectEnemy))
			},
			{
				788,
				new BrushData(788, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Effect_particle04", typeof(EffectEnemy))
			},
			{
				789,
				new BrushData(789, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Effect_Background01", typeof(EffectEnemy))
			},
			{
				790,
				new BrushData(790, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Effect_particle_end", typeof(EffectEnemy))
			},
			{
				792,
				new BrushData(792, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Midground_liushengji", typeof(AnimatorEnemy))
			},
			{
				793,
				new BrushData(793, "Base/Levels/Waltz/Brushes/Enemy/Waltz_Effect_startglow02", typeof(ParticleSystemAlphaEnemy))
			},
			{
				794,
				new BrushData(794, "Base/Levels/Waltz/Brushes/Enemy/MovableEnemy", typeof(MovableEnemy))
			},
			{
				900,
				new BrushData(900, "Base/Levels/Theif/Brushes/Enemy/Theif_Award_Diamond", typeof(DiamondAward))
			},
			{
				901,
				new BrushData(901, "Base/Levels/Theif/Brushes/Enemy/Theif_Effect_Jump_UP", typeof(TwoEffectTriggerPro))
			},
			{
				902,
				new BrushData(902, "Base/Levels/Theif/Brushes/Enemy/Theif_Effect_Jump_SwingingRope", typeof(TwoEffectTriggerPro))
			},
			{
				903,
				new BrushData(903, "Base/Levels/Theif/Brushes/Enemy/Theif_Effect_Jump", typeof(TwoEffectTriggerPro))
			},
			{
				904,
				new BrushData(904, "Base/Levels/Theif/Brushes/Enemy/Theif_FreeMoveBarrelEnemy", typeof(FreeMoveBarrelEnemy))
			},
			{
				905,
				new BrushData(905, "Base/Levels/Theif/Brushes/Enemy/Thief_TieMen", typeof(AnimEnemyPro))
			},
			{
				906,
				new BrushData(906, "Base/Levels/Theif/Brushes/Enemy/Thief_ChangQiang", typeof(AnimEnemyPro))
			},
			{
				907,
				new BrushData(907, "Base/Levels/Theif/Brushes/Enemy/Thief_DuanJian", typeof(AnimEnemyPro))
			},
			{
				908,
				new BrushData(908, "Base/Levels/Theif/Brushes/Enemy/Thief_WeiBing_L", typeof(AnimEnemyPro))
			},
			{
				909,
				new BrushData(909, "Base/Levels/Theif/Brushes/Enemy/Thief_WeiBing_L01", typeof(AnimEnemyPro))
			},
			{
				910,
				new BrushData(910, "Base/Levels/Theif/Brushes/Enemy/Thief_WeiBing_R", typeof(AnimEnemyPro))
			},
			{
				911,
				new BrushData(911, "Base/Levels/Theif/Brushes/Enemy/Thief_XiangXiang_01", typeof(AnimEnemyPro))
			},
			{
				912,
				new BrushData(912, "Base/Levels/Theif/Brushes/Enemy/Thief_XiangXiang_02", typeof(AnimEnemyPro))
			},
			{
				913,
				new BrushData(913, "Base/Levels/Theif/Brushes/Enemy/Thief_XiangXiang_03", typeof(AnimEnemyPro))
			},
			{
				914,
				new BrushData(914, "Base/Levels/Theif/Brushes/Enemy/Thief_XiangXiang_04", typeof(AnimEnemyPro))
			},
			{
				915,
				new BrushData(915, "Base/Levels/Theif/Brushes/Enemy/Theif_Drop_Crown", typeof(FreeMoveCrownByCouple))
			},
			{
				916,
				new BrushData(916, "Base/Levels/Theif/Brushes/Enemy/Theif_Drop_Diamond", typeof(FreeMoveDiamondByCouple))
			},
			{
				917,
				new BrushData(917, "Base/Levels/Theif/Brushes/Enemy/Thief_XiangXiangMaChe_01", typeof(AnimEnemyPro))
			},
			{
				918,
				new BrushData(918, "Base/Levels/Theif/Brushes/Enemy/Thief_XiangXiang_05", typeof(AnimEnemyPro))
			},
			{
				919,
				new BrushData(919, "Base/Levels/Theif/Brushes/Enemy/Thief_XiangXiangMaChe_tong01", typeof(AnimEnemyPro))
			},
			{
				920,
				new BrushData(920, "Base/Levels/Theif/Brushes/Enemy/Thief_XiangXiangMaChe_tong02", typeof(AnimEnemyPro))
			},
			{
				921,
				new BrushData(921, "Base/Levels/Theif/Brushes/Enemy/Thief_FuZi", typeof(AnimEnemyPro))
			},
			{
				922,
				new BrushData(922, "Base/Levels/Theif/Brushes/Enemy/Thief_WeiBing_R01", typeof(AnimEnemyPro))
			},
			{
				923,
				new BrushData(923, "Base/Levels/Theif/Brushes/Enemy/Thief_StartTreasureChest", typeof(StartAnimEnemy))
			},
			{
				924,
				new BrushData(924, "Base/Levels/Theif/Brushes/Enemy/Thief_HuaPenDiaoLuo_01", typeof(AnimEnemyPro))
			},
			{
				925,
				new BrushData(925, "Base/Levels/Theif/Brushes/Enemy/Thief_LiangYiJia_01", typeof(AnimEnemyPro))
			},
			{
				926,
				new BrushData(926, "Base/Levels/Theif/Brushes/Enemy/Thief_PingTaiTaXian_02", typeof(AnimEnemyProByCouple))
			},
			{
				927,
				new BrushData(927, "Base/Levels/Theif/Brushes/Enemy/Thief_moodHappy", typeof(RoleChangeMoodByEvent))
			},
			{
				928,
				new BrushData(928, "Base/Levels/Theif/Brushes/Enemy/Thief_CoverFace_Begin", typeof(RoleShowMaskEffect))
			},
			{
				929,
				new BrushData(929, "Base/Levels/Theif/Brushes/Enemy/Thief_CoverFace_End", typeof(RoleShowMaskEffect))
			},
			{
				930,
				new BrushData(930, "Base/Levels/Theif/Brushes/Enemy/Theif_Midground_deng_b", typeof(AnimEnemyProByCouple))
			},
			{
				931,
				new BrushData(931, "Base/Levels/Theif/Brushes/Enemy/Thief_Projector_END", typeof(AnimEnemyPro))
			},
			{
				932,
				new BrushData(932, "Base/Levels/Theif/Brushes/Enemy/Thief_Crown_Fragment", typeof(CrownFragment))
			},
			{
				933,
				new BrushData(933, "Base/Levels/Theif/Brushes/Enemy/Thief_Crown_FromFragment", typeof(CrownFromFragment))
			},
			{
				934,
				new BrushData(934, "Base/Levels/Theif/Brushes/Enemy/Theif_Effect_Jump_SwingingRope_SP", typeof(TwoEffectTriggerSpecial))
			},
			{
				935,
				new BrushData(935, "Base/Levels/Theif/Brushes/Enemy/Thief_XiangXiang_01_H", typeof(AnimEnemyPro))
			},
			{
				1000,
				new BrushData(1000, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_Stone01", typeof(AnimEnemyPro))
			},
			{
				1001,
				new BrushData(1001, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Enemy_Stone02", typeof(AnimEnemyPro))
			},
			{
				1002,
				new BrushData(1002, "Base/Levels/Tutorial/Brushes/Enemy/Tutorial_Yun_Kai", typeof(AnimEffectShowEnemy))
			},
			{
				1003,
				new BrushData(1003, "Base/Levels/Tutorial/Brushes/Enemy/Fate_ChiLunZu_anim01_Tutorial", typeof(AnimEnemyPro))
			},
			{
				1101,
				new BrushData(1101, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Enemy_yuepu_05", typeof(AnimEnemyPro))
			},
			{
				1102,
				new BrushData(1102, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Enemy_yuepu_06", typeof(AnimEnemyPro))
			},
			{
				1103,
				new BrushData(1103, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Enemy_SuperJump", typeof(JumpDistanceTrigger))
			},
			{
				1104,
				new BrushData(1104, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Start", typeof(DesertGateWay))
			},
			{
				1105,
				new BrushData(1105, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_Jump_001", typeof(AnimEnemyPro))
			},
			{
				1106,
				new BrushData(1106, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_Jump_002", typeof(AnimEnemyPro))
			},
			{
				1107,
				new BrushData(1107, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_Jump_001_S", typeof(AnimEnemyPro))
			},
			{
				1108,
				new BrushData(1108, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_Jump_002_S", typeof(AnimEnemyPro))
			},
			{
				1110,
				new BrushData(1110, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_guankou_a", typeof(AnimEnemyPro))
			},
			{
				1111,
				new BrushData(1111, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_guankou_b", typeof(AnimEnemyPro))
			},
			{
				1115,
				new BrushData(1115, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_Jump_001_easy", typeof(AnimEnemyPro))
			},
			{
				1116,
				new BrushData(1116, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_Jump_002_easy", typeof(AnimEnemyPro))
			},
			{
				1117,
				new BrushData(1117, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_Jump_001_S_easy", typeof(AnimEnemyPro))
			},
			{
				1118,
				new BrushData(1118, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Danhuangguan_Jump_002_S_easy", typeof(AnimEnemyPro))
			},
			{
				1119,
				new BrushData(1119, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Laba", typeof(AnimEnemyPro))
			},
			{
				1123,
				new BrushData(1123, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Enemy_Hao01_S", typeof(AnimEnemyPro))
			},
			{
				1124,
				new BrushData(1124, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Enemy_Hao01", typeof(AnimEnemyPro))
			},
			{
				1125,
				new BrushData(1125, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Gu02_S", typeof(AnimEnemyPro))
			},
			{
				1126,
				new BrushData(1126, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_Gu02", typeof(AnimEnemyPro))
			},
			{
				1127,
				new BrushData(1127, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_douya", typeof(AnimEnemyPro))
			},
			{
				1128,
				new BrushData(1128, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_douya02", typeof(AnimEnemyPro))
			},
			{
				1129,
				new BrushData(1129, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_koufengqin_01", typeof(AnimEnemyPro))
			},
			{
				1130,
				new BrushData(1130, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_koufengqin_02", typeof(AnimEnemyPro))
			},
			{
				1131,
				new BrushData(1131, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_shoufengqin", typeof(AnimEnemyPro))
			},
			{
				1132,
				new BrushData(1132, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_shoufengqin_S", typeof(AnimEnemyPro))
			},
			{
				1133,
				new BrushData(1133, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_koufengqin_01_S", typeof(AnimEnemyPro))
			},
			{
				1139,
				new BrushData(1139, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Enemy_Empty_yuepu", typeof(NormalEnemy))
			},
			{
				1140,
				new BrushData(1140, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Enemy_Empty_piano", typeof(NormalEnemy))
			},
			{
				1163,
				new BrushData(1163, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Midground_xiaohao_anim01", typeof(AnimEnemyPro))
			},
			{
				1181,
				new BrushData(1181, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Enemy_End", typeof(AnimEnemyPro))
			},
			{
				1182,
				new BrushData(1182, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Effect_tiaodian01", typeof(EffectEnemy))
			},
			{
				1183,
				new BrushData(1183, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Effect_Crown", typeof(TwoEffectTriggerPro))
			},
			{
				1189,
				new BrushData(1189, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Rotation01_Left", typeof(AnimEnemyPro))
			},
			{
				1190,
				new BrushData(1190, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Rotation01_Right", typeof(AnimEnemyPro))
			},
			{
				1191,
				new BrushData(1191, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Enemy_Smoke01", typeof(EffectEnemy))
			},
			{
				1192,
				new BrushData(1192, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Effect_tiaodaiyinfu01", typeof(AnimEnemyPro))
			},
			{
				1196,
				new BrushData(1196, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Effect_jiewei01", typeof(EffectEnemy))
			},
			{
				1198,
				new BrushData(1198, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Crown_Fragment", typeof(CrownFragment))
			},
			{
				1199,
				new BrushData(1199, "Base/Levels/Fantasia_Jazz/Brushes/Enemy/Jazz_Crown_FromFragment", typeof(CrownFromFragment))
			},
			{
				1201,
				new BrushData(1201, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_HuaSha_LuoShi01", typeof(AnimEnemyPro))
			},
			{
				1202,
				new BrushData(1202, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_HuaSha_LuoShi02", typeof(AnimEnemyPro))
			},
			{
				1203,
				new BrushData(1203, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_HuaSha_LuoShi03", typeof(AnimEnemyPro))
			},
			{
				1204,
				new BrushData(1204, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_HuaSha_LuoShi04", typeof(AnimEnemyPro))
			},
			{
				1205,
				new BrushData(1205, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_HuaSha_LuoShi05", typeof(AnimEnemyPro))
			},
			{
				1206,
				new BrushData(1206, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_HuaSha_LuoShi_End", typeof(AnimEnemyPro))
			},
			{
				1207,
				new BrushData(1207, "Base/Levels/Home_Rainbow/Brushes/Enemy/RandomAnimEnemy", typeof(RandomAnimEnemy))
			},
			{
				1208,
				new BrushData(1208, "Base/Levels/Home_Rainbow/Brushes/Enemy/NongWu_LuoShi", typeof(AnimParticleEnemy))
			},
			{
				1209,
				new BrushData(1209, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Huang", typeof(AnimEnemyPro))
			},
			{
				1210,
				new BrushData(1210, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Huang02", typeof(AnimEnemyPro))
			},
			{
				1211,
				new BrushData(1211, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Huang03", typeof(AnimEnemyPro))
			},
			{
				1212,
				new BrushData(1212, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Huang04", typeof(AnimEnemyPro))
			},
			{
				1213,
				new BrushData(1213, "Base/Levels/Home_Rainbow/Brushes/Enemy/GuoChang", typeof(AnimEnemyPro))
			},
			{
				1214,
				new BrushData(1214, "Base/Levels/Home_Rainbow/Brushes/Enemy/RandomNormalDiamond", typeof(RandomNormalDiamond))
			},
			{
				1215,
				new BrushData(1215, "Base/Levels/Home_Rainbow/Brushes/Enemy/ShanDouShiBanLu_02", typeof(NormalEnemy))
			},
			{
				1216,
				new BrushData(1216, "Base/Levels/Home_Rainbow/Brushes/Enemy/ShanDouShiBanLu03_01", typeof(NormalEnemy))
			},
			{
				1217,
				new BrushData(1217, "Base/Levels/Home_Rainbow/Brushes/Enemy/ShanDouShiBanLu03_02", typeof(NormalEnemy))
			},
			{
				1218,
				new BrushData(1218, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Huang_Mid01", typeof(AnimEnemyPro))
			},
			{
				1219,
				new BrushData(1219, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Hong_Mid01", typeof(AnimEnemyPro))
			},
			{
				1220,
				new BrushData(1220, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Lan_Mid01", typeof(AnimEnemyPro))
			},
			{
				1221,
				new BrushData(1221, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Hong_Mid02", typeof(AnimEnemyPro))
			},
			{
				1222,
				new BrushData(1222, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Hong_Mid03", typeof(AnimEnemyPro))
			},
			{
				1223,
				new BrushData(1223, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Lan_Mid02", typeof(AnimEnemyPro))
			},
			{
				1224,
				new BrushData(1224, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Hong_Mid04", typeof(AnimEnemyPro))
			},
			{
				1225,
				new BrushData(1225, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_Effect_tiaodian01", typeof(AnimEnemyPro))
			},
			{
				1226,
				new BrushData(1226, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_ShanDong_glow02", typeof(AnimEnemyPro))
			},
			{
				1227,
				new BrushData(1227, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_ShanDong_glow01", typeof(AnimEnemyPro))
			},
			{
				1228,
				new BrushData(1228, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Huang_ShanDong", typeof(AnimEnemyPro))
			},
			{
				1229,
				new BrushData(1229, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_01_Ground02", typeof(AnimEnemyPro))
			},
			{
				1230,
				new BrushData(1230, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_01_Ground03", typeof(AnimEnemyPro))
			},
			{
				1231,
				new BrushData(1231, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_01_Ground04", typeof(AnimEnemyPro))
			},
			{
				1232,
				new BrushData(1232, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_02_Ground01", typeof(AnimEnemyPro))
			},
			{
				1233,
				new BrushData(1233, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_02_Ground02", typeof(AnimEnemyPro))
			},
			{
				1234,
				new BrushData(1234, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_02_Ground03", typeof(AnimEnemyPro))
			},
			{
				1235,
				new BrushData(1235, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_03_Ground01", typeof(AnimEnemyPro))
			},
			{
				1236,
				new BrushData(1236, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_03_Ground02", typeof(AnimEnemyPro))
			},
			{
				1237,
				new BrushData(1237, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_03_Ground03", typeof(AnimEnemyPro))
			},
			{
				1238,
				new BrushData(1238, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_03_Ground04", typeof(AnimEnemyPro))
			},
			{
				1239,
				new BrushData(1239, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_03_Ground05", typeof(AnimEnemyPro))
			},
			{
				1240,
				new BrushData(1240, "Base/Levels/Home_Rainbow/Brushes/Enemy/Effect_Jump_UP_Rainbow", typeof(TwoEffectTriggerPro))
			},
			{
				1241,
				new BrushData(1241, "Base/Levels/Home_Rainbow/Brushes/Enemy/Effect_Jump_UP_Rainbow01", typeof(TwoEffectTriggerPro))
			},
			{
				1242,
				new BrushData(1242, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_04_Ground01", typeof(AnimEnemyPro))
			},
			{
				1243,
				new BrushData(1243, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_04_Ground02", typeof(AnimEnemyPro))
			},
			{
				1244,
				new BrushData(1244, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_04_Ground03", typeof(AnimEnemyPro))
			},
			{
				1245,
				new BrushData(1245, "Base/Levels/Home_Rainbow/Brushes/Enemy/Award_Diamond_RainBow", typeof(DiamondAward))
			},
			{
				1246,
				new BrushData(1246, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_Huang_ShanDong_NEW", typeof(AnimEnemyPro))
			},
			{
				1247,
				new BrushData(1247, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_Enemy_ShanDong", typeof(AnimEnemyPro))
			},
			{
				1248,
				new BrushData(1248, "Base/Levels/Home_Rainbow/Brushes/Enemy/fuwen_huang_end01", typeof(AnimEnemyPro))
			},
			{
				1249,
				new BrushData(1249, "Base/Levels/Home_Rainbow/Brushes/Enemy/fuwen_huang_end02", typeof(AnimEnemyPro))
			},
			{
				1250,
				new BrushData(1250, "Base/Levels/Home_Rainbow/Brushes/Enemy/fuwen_huang_end03", typeof(AnimEnemyPro))
			},
			{
				1251,
				new BrushData(1251, "Base/Levels/Home_Rainbow/Brushes/Enemy/fuwen_huang_end04", typeof(AnimEnemyPro))
			},
			{
				1252,
				new BrushData(1252, "Base/Levels/Home_Rainbow/Brushes/Enemy/RainBow_Effect_Jump_Big", typeof(TwoEffectTriggerPro))
			},
			{
				1253,
				new BrushData(1253, "Base/Levels/Home_Rainbow/Brushes/Enemy/Enemy_Capsule_Kong", typeof(AnimEnemyPro))
			},
			{
				1254,
				new BrushData(1254, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_shizhu_a", typeof(AnimEnemyPro))
			},
			{
				1255,
				new BrushData(1255, "Base/Levels/Home_Rainbow/Brushes/Enemy/RainBow_Yun01_Kai01", typeof(AnimEnemyPro))
			},
			{
				1256,
				new BrushData(1256, "Base/Levels/Home_Rainbow/Brushes/Enemy/Enemy_YangTuo_02", typeof(AnimEnemyPro))
			},
			{
				1257,
				new BrushData(1257, "Base/Levels/Home_Rainbow/Brushes/Enemy/Enemy_YangTuo_03", typeof(AnimEnemyPro))
			},
			{
				1258,
				new BrushData(1258, "Base/Levels/Home_Rainbow/Brushes/Enemy/Enemy_YangTuo_04", typeof(AnimEnemyPro))
			},
			{
				1259,
				new BrushData(1259, "Base/Levels/Home_Rainbow/Brushes/Enemy/FuWen_ShanDongFuWen", typeof(AnimEnemyPro))
			},
			{
				1260,
				new BrushData(1260, "Base/Levels/Home_Rainbow/Brushes/Enemy/Rainbow_Effect_shizhu01", typeof(AnimEnemyPro))
			},
			{
				1261,
				new BrushData(1261, "Base/Levels/Home_Rainbow/Brushes/Enemy/RainBow_Yun01_Kai02", typeof(AnimEnemyPro))
			},
			{
				1262,
				new BrushData(1262, "Base/Levels/Home_Rainbow/Brushes/Enemy/YiJi_04_Follow_Enemy", typeof(AnimEnemyPro))
			},
			{
				1270,
				new BrushData(1270, "Base/Levels/Home_Rainbow/Brushes/Enemy/Enemy_YangTuo_Normal", typeof(AlpacaEnemy))
			},
			{
				1300,
				new BrushData(1300, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_hongfangzifangzi_a_001", typeof(NormalEnemy))
			},
			{
				1301,
				new BrushData(1301, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_hongfangzifangzi_a_002", typeof(NormalEnemy))
			},
			{
				1302,
				new BrushData(1302, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_hongfangzifangzi_a_003", typeof(NormalEnemy))
			},
			{
				1303,
				new BrushData(1303, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_huangfangzi_c_01", typeof(NormalEnemy))
			},
			{
				1304,
				new BrushData(1304, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_huangfangzi_c_03", typeof(NormalEnemy))
			},
			{
				1305,
				new BrushData(1305, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_JiaoTang_DaMen_QiangBi", typeof(NormalEnemy))
			},
			{
				1306,
				new BrushData(1306, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_JiaoTang_Qiang01", typeof(NormalEnemy))
			},
			{
				1307,
				new BrushData(1307, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_JiaoTang_ZhongLou", typeof(NormalEnemy))
			},
			{
				1308,
				new BrushData(1308, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_JiaoTang_ZhongLou01", typeof(NormalEnemy))
			},
			{
				1309,
				new BrushData(1309, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_lanfangzi_d_01", typeof(NormalEnemy))
			},
			{
				1310,
				new BrushData(1310, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_lanfangzi_d_02", typeof(NormalEnemy))
			},
			{
				1311,
				new BrushData(1311, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_YanCong01", typeof(NormalEnemy))
			},
			{
				1312,
				new BrushData(1312, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_YanCong02", typeof(NormalEnemy))
			},
			{
				1313,
				new BrushData(1313, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_YanCong03", typeof(NormalEnemy))
			},
			{
				1314,
				new BrushData(1314, "Base/Levels/WeirdDream/Brushes/Tile/WeirdDream_Road_Small_Bug", typeof(AnimEnemyPro))
			},
			{
				1315,
				new BrushData(1315, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Enemy_Road01", typeof(AnimEnemyPro))
			},
			{
				1316,
				new BrushData(1316, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Enemy_Road02", typeof(AnimEnemyPro))
			},
			{
				1317,
				new BrushData(1317, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Enemy_Road03", typeof(AnimEnemyPro))
			},
			{
				1318,
				new BrushData(1318, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Enemy_Road04", typeof(AnimEnemyPro))
			},
			{
				1319,
				new BrushData(1319, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_LuDeng01_ON_Off", typeof(DoorEnemy))
			},
			{
				1320,
				new BrushData(1320, "Base/Levels/WeirdDream/Brushes/Midground/WeirdDream_JiaoTang_DaMen", typeof(AnimEnemyPro))
			},
			{
				1321,
				new BrushData(1321, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_XiaoZhen_Star_UP", typeof(AnimEnemy))
			},
			{
				1322,
				new BrushData(1322, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_XiaoZhen_Star_Down", typeof(AnimEnemyPro))
			},
			{
				1323,
				new BrushData(1323, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_DuBai_Star", typeof(AnimEnemy))
			},
			{
				1324,
				new BrushData(1324, "Base/Levels/WeirdDream/Brushes/Midground/WeirdDream_Yu01", typeof(AnimEnemyPro))
			},
			{
				1325,
				new BrushData(1325, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_End_star", typeof(AnimEnemyPro))
			},
			{
				1326,
				new BrushData(1326, "Base/Levels/WeirdDream/Brushes/Midground/WeirdDream_denghai_01", typeof(AnimEnemy))
			},
			{
				1327,
				new BrushData(1327, "Base/Levels/WeirdDream/Brushes/Midground/WeirdDream_denghai_02", typeof(AnimEnemy))
			},
			{
				1328,
				new BrushData(1328, "Base/Levels/WeirdDream/Brushes/Midground/WeirdDream_denghai_03", typeof(AnimEnemy))
			},
			{
				1329,
				new BrushData(1329, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_room", typeof(DesertGateWay))
			},
			{
				1330,
				new BrushData(1330, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Planet", typeof(AnimEnemyPro))
			},
			{
				1331,
				new BrushData(1331, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Enemy_Success", typeof(AnimEnemyPro))
			},
			{
				1332,
				new BrushData(1332, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Enemy_SuperJump", typeof(JumpDistanceTrigger))
			},
			{
				1333,
				new BrushData(1333, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Enemy_Beerhead", typeof(AnimEnemyPro))
			},
			{
				1334,
				new BrushData(1334, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Effect_Jump_Effect", typeof(TwoEffectTriggerPro))
			},
			{
				1335,
				new BrushData(1335, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Effect_Background01", typeof(EffectEnemy))
			},
			{
				1336,
				new BrushData(1336, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Enemy_BigStar", typeof(AnimEnemyPro))
			},
			{
				1337,
				new BrushData(1337, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_hongfangzifangzi_a_004", typeof(AnimEnemyPro))
			},
			{
				1338,
				new BrushData(1338, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_hongfangzifangzi_a_002", typeof(AnimEnemyPro))
			},
			{
				1339,
				new BrushData(1339, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_hongfangzifangzi_a_003", typeof(AnimEnemyPro))
			},
			{
				1340,
				new BrushData(1340, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_huangfangzi_c_02", typeof(AnimEnemyPro))
			},
			{
				1341,
				new BrushData(1341, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_huangfangzi_c_04", typeof(AnimEnemyPro))
			},
			{
				1342,
				new BrushData(1342, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_JiaoTang_DaMen_QiangBi", typeof(AnimEnemyPro))
			},
			{
				1343,
				new BrushData(1343, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_JiaoTang_Qiang01", typeof(AnimEnemyPro))
			},
			{
				1344,
				new BrushData(1344, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_JiaoTang_ZhongLou", typeof(AnimEnemyPro))
			},
			{
				1345,
				new BrushData(1345, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_JiaoTang_ZhongLou01", typeof(AnimEnemyPro))
			},
			{
				1346,
				new BrushData(1346, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_lanfangzi_d_01", typeof(AnimEnemyPro))
			},
			{
				1347,
				new BrushData(1347, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_lanfangzi_d_02", typeof(AnimEnemyPro))
			},
			{
				1348,
				new BrushData(1348, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_YanCong01", typeof(AnimEnemyPro))
			},
			{
				1349,
				new BrushData(1349, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_YanCong02", typeof(AnimEnemyPro))
			},
			{
				1350,
				new BrushData(1350, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_YanCong03", typeof(AnimEnemyPro))
			},
			{
				1351,
				new BrushData(1351, "Base/Levels/WeirdDream/Brushes/Enemy/WeirdDream_Enemy_SuperJump_1G", typeof(JumpDistanceTrigger))
			},
			{
				1400,
				new BrushData(1400, "Base/Levels/LightMap/Brush/Enemy/Enemy_ShaMo_shachong_A", typeof(AnimParticleEnemy))
			},
			{
				1401,
				new BrushData(1401, "Base/Levels/LightMap/Brush/Enemy/Aiji_Shachong_B", typeof(AnimParticleEnemy))
			},
			{
				1402,
				new BrushData(1402, "Base/Levels/LightMap/Brush/Enemy/Aiji_Shachong_C", typeof(AnimParticleEnemy))
			},
			{
				1403,
				new BrushData(1403, "Base/Levels/LightMap/Brush/Enemy/Aiji_Shachong_D", typeof(AnimParticleEnemy))
			},
			{
				1404,
				new BrushData(1404, "Base/Levels/LightMap/Brush/Enemy/Aiji_Shachong_E", typeof(AnimParticleEnemy))
			},
			{
				1405,
				new BrushData(1405, "Base/Levels/LightMap/Brush/Enemy/Aiji_Shachong_F", typeof(AnimParticleEnemy))
			},
			{
				1406,
				new BrushData(1406, "Base/Levels/LightMap/Brush/Enemy/Aiji_Shachong_G", typeof(AnimParticleEnemy))
			},
			{
				1407,
				new BrushData(1407, "Base/Levels/LightMap/Brush/Enemy/Aiji_Dikuai_1", typeof(AnimParticleEnemy))
			},
			{
				1408,
				new BrushData(1408, "Base/Levels/LightMap/Brush/Enemy/Aiji_Jiewei_01", typeof(AnimParticleEnemy))
			},
			{
				1409,
				new BrushData(1409, "Base/Levels/LightMap/Brush/Enemy/AiJi_Muban_2X2", typeof(AnimParticleEnemy))
			},
			{
				1410,
				new BrushData(1410, "Base/Levels/LightMap/Brush/Enemy/Aiji_Shizhu_Up", typeof(AnimEnemyPro))
			},
			{
				1411,
				new BrushData(1411, "Base/Levels/LightMap/Brush/Enemy/Aiji_Shizhu_Loop", typeof(AnimEnemyPro))
			},
			{
				1412,
				new BrushData(1412, "Base/Levels/LightMap/Brush/Enemy/AiJi_Muban_1X3", typeof(AnimParticleEnemy))
			},
			{
				1413,
				new BrushData(1413, "Base/Levels/LightMap/Brush/Enemy/Aiji_Effect_Jump", typeof(TwoEffectTriggerPro))
			},
			{
				1414,
				new BrushData(1414, "Base/Levels/LightMap/Brush/Enemy/Effect_SunShafts_little_HN", typeof(EffectEnemy))
			},
			{
				1415,
				new BrushData(1415, "Base/Levels/LightMap/Brush/Enemy/AiJi_Luoshi_1", typeof(AnimParticleEnemy))
			},
			{
				1416,
				new BrushData(1416, "Base/Levels/LightMap/Brush/Enemy/AiJi_Luoshi_2", typeof(AnimParticleEnemy))
			},
			{
				1417,
				new BrushData(1417, "Base/Levels/LightMap/Brush/Enemy/AiJi_Muban_4_HN", typeof(AnimParticleEnemy))
			},
			{
				1500,
				new BrushData(1500, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_SuperJump", typeof(JumpDistanceTrigger))
			},
			{
				1501,
				new BrushData(1501, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Tile_Wood_zhankai", typeof(AnimEnemyPro))
			},
			{
				1502,
				new BrushData(1502, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_Yun01", typeof(AnimEnemyPro))
			},
			{
				1503,
				new BrushData(1503, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_Yinghua", typeof(AnimEnemyPro))
			},
			{
				1504,
				new BrushData(1504, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_Huaban", typeof(AnimEnemyPro))
			},
			{
				1505,
				new BrushData(1505, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Start", typeof(DesertGateWay))
			},
			{
				1506,
				new BrushData(1506, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Effect_Jump_UP1", typeof(TwoEffectTriggerPro))
			},
			{
				1507,
				new BrushData(1507, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_zhu_ani", typeof(AnimEnemyPro))
			},
			{
				1521,
				new BrushData(1521, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_Taigu_Anim01", typeof(AnimEnemyPro))
			},
			{
				1522,
				new BrushData(1522, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_Taigu_Anim02", typeof(AnimEnemyPro))
			},
			{
				1530,
				new BrushData(1530, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_pingfeng", typeof(AnimEnemyPro))
			},
			{
				1531,
				new BrushData(1531, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_pingfeng_Anim01", typeof(AnimEnemyPro))
			},
			{
				1532,
				new BrushData(1532, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_pingfeng_Anim02", typeof(AnimEnemyPro))
			},
			{
				1541,
				new BrushData(1541, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_Feibiao_Anim01", typeof(AnimEnemyPro))
			},
			{
				1542,
				new BrushData(1542, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_Feibiao_Anim02", typeof(AnimEnemyPro))
			},
			{
				1551,
				new BrushData(1551, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_Jian_Anim01", typeof(AnimEnemyPro))
			},
			{
				1552,
				new BrushData(1552, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_yueliang", typeof(AnimEnemyPro))
			},
			{
				1553,
				new BrushData(1553, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_xingyun", typeof(AnimEnemyPro))
			},
			{
				1554,
				new BrushData(1554, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_yinghualu", typeof(AnimEnemyPro))
			},
			{
				1555,
				new BrushData(1555, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Effect_Jump_UP", typeof(TwoEffectTriggerPro))
			},
			{
				1556,
				new BrushData(1556, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Effect_Jump_UP_L", typeof(TwoEffectTriggerPro))
			},
			{
				1557,
				new BrushData(1557, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Effect_Jump_UP_R", typeof(TwoEffectTriggerPro))
			},
			{
				1558,
				new BrushData(1558, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_yinghualu_s", typeof(AnimEnemyPro))
			},
			{
				1561,
				new BrushData(1561, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_San_On", typeof(AnimEnemyPro))
			},
			{
				1562,
				new BrushData(1562, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_San_Off", typeof(AnimEnemyPro))
			},
			{
				1563,
				new BrushData(1563, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_men_b", typeof(AnimEnemyPro))
			},
			{
				1564,
				new BrushData(1564, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Tile_Onfire", typeof(AnimEnemyPro))
			},
			{
				1571,
				new BrushData(1571, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Tile_Wood_Fire", typeof(AnimEnemyPro))
			},
			{
				1581,
				new BrushData(1581, "Base/Levels/Samurai/Brushes/Enemy/Samurai_CutListenForAssassinEnemy", typeof(CutListenForAssassinEnemy))
			},
			{
				1582,
				new BrushData(1582, "Base/Levels/Samurai/Brushes/Enemy/Samurai_CutListenForAssassinEnemy", typeof(ContinuousCutListenForAssassinEnemy))
			},
			{
				1583,
				new BrushData(1583, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_cike_att01", typeof(AnimEnemyPro))
			},
			{
				1584,
				new BrushData(1584, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_cike_att02", typeof(AnimEnemyPro))
			},
			{
				1585,
				new BrushData(1585, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_cike_att03", typeof(AnimEnemyPro))
			},
			{
				1586,
				new BrushData(1586, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_cike_att04", typeof(AnimEnemyPro))
			},
			{
				1587,
				new BrushData(1587, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_cike_att06", typeof(AnimEnemyPro))
			},
			{
				1588,
				new BrushData(1588, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_kushanshui_01", typeof(AnimEnemyPro))
			},
			{
				1589,
				new BrushData(1589, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_kushanshui", typeof(AnimEnemyPro))
			},
			{
				1590,
				new BrushData(1590, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_kushanshui_03", typeof(AnimEnemyPro))
			},
			{
				1591,
				new BrushData(1591, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Effect_Zhaoshi01", typeof(TwoEffectTriggerPro))
			},
			{
				1592,
				new BrushData(1592, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_pingfeng_b", typeof(AnimEnemyPro))
			},
			{
				1593,
				new BrushData(1593, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_men_a_2", typeof(AnimEnemyPro))
			},
			{
				1594,
				new BrushData(1594, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Tile_3x3", typeof(AnimEnemyPro))
			},
			{
				1595,
				new BrushData(1595, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Tile_2x2", typeof(AnimEnemyPro))
			},
			{
				1596,
				new BrushData(1596, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Midground_pingfeng_c", typeof(AnimEnemyPro))
			},
			{
				1597,
				new BrushData(1597, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_cike_att05", typeof(AnimEnemyPro))
			},
			{
				1598,
				new BrushData(1598, "Base/Levels/Samurai/Brushes/Enemy/Samurai_CutListenForAssassinEnemy_1", typeof(ContinuousCutListenForAssassinEnemy))
			},
			{
				1599,
				new BrushData(1599, "Base/Levels/Samurai/Brushes/Enemy/Samurai_Enemy_kushanshui_04", typeof(AnimEnemyPro))
			},
			{
				1603,
				new BrushData(1603, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_GunTong_01", typeof(AnimEnemyPro))
			},
			{
				1604,
				new BrushData(1604, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_GunTong_02", typeof(AnimEnemyPro))
			},
			{
				1605,
				new BrushData(1605, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_GunTong_03", typeof(AnimEnemyPro))
			},
			{
				1606,
				new BrushData(1606, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_Gou", typeof(AnimEnemyPro))
			},
			{
				1607,
				new BrushData(1607, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_MuBanFeiTian", typeof(AnimEnemyPro))
			},
			{
				1608,
				new BrushData(1608, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_MaTouTong", typeof(AnimEnemyPro))
			},
			{
				1609,
				new BrushData(1609, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_Chuan01", typeof(AnimEnemyPro))
			},
			{
				1610,
				new BrushData(1610, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_Chuan02", typeof(AnimEnemyPro))
			},
			{
				1611,
				new BrushData(1611, "Base/Levels/Thief_2/Brushes/Enemy/Thief02_XiangXiang_03", typeof(AnimEnemyPro))
			},
			{
				1612,
				new BrushData(1612, "Base/Levels/Thief_2/Brushes/Enemy/Thief02_XiangXiang_04", typeof(AnimEnemyPro))
			},
			{
				1613,
				new BrushData(1613, "Base/Levels/Thief_2/Brushes/Enemy/Thief02_FanDaoXiang_01", typeof(AnimEnemyPro))
			},
			{
				1614,
				new BrushData(1614, "Base/Levels/Thief_2/Brushes/Enemy/Thief02_FanDaoXiang_02", typeof(AnimEnemyPro))
			},
			{
				1616,
				new BrushData(1616, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_KaiChuang", typeof(AnimEnemyPro))
			},
			{
				1617,
				new BrushData(1617, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_TuiChe", typeof(AnimEnemyPro))
			},
			{
				1618,
				new BrushData(1618, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_XieHuo", typeof(AnimEnemyPro))
			},
			{
				1619,
				new BrushData(1619, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_KangTong", typeof(AnimEnemyPro))
			},
			{
				1620,
				new BrushData(1620, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_ChuanSong00", typeof(AnimEnemyPro))
			},
			{
				1621,
				new BrushData(1621, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_FuNvJingXia", typeof(AnimEnemyPro))
			},
			{
				1622,
				new BrushData(1622, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_MaGanJingXia", typeof(AnimEnemyPro))
			},
			{
				1623,
				new BrushData(1623, "Base/Levels/Thief_2/Brushes/Enemy/Thief02_DiaoXiangZi", typeof(AnimEnemyPro))
			},
			{
				1624,
				new BrushData(1624, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_Effect_YinYing", typeof(EffectEnemy))
			},
			{
				1625,
				new BrushData(1625, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_Effect_ShuiHua", typeof(EffectEnemy))
			},
			{
				1627,
				new BrushData(1627, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_XiZao", typeof(AnimEnemyPro))
			},
			{
				1628,
				new BrushData(1628, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_XiangXiang_02", typeof(AnimEnemyPro))
			},
			{
				1629,
				new BrushData(1629, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_XiangXiang_01", typeof(AnimEnemyPro))
			},
			{
				1630,
				new BrushData(1630, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_XiangXiangMaChe_01", typeof(AnimEnemyPro))
			},
			{
				1631,
				new BrushData(1631, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_XiangXiang_03", typeof(AnimEnemyPro))
			},
			{
				1632,
				new BrushData(1632, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_Success", typeof(AnimEnemyPro))
			},
			{
				1633,
				new BrushData(1633, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_GunTong_01_X", typeof(AnimEnemyPro))
			},
			{
				1634,
				new BrushData(1634, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_GunTong_02_X", typeof(AnimEnemyPro))
			},
			{
				1635,
				new BrushData(1635, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_KangTong_X", typeof(AnimEnemyPro))
			},
			{
				1636,
				new BrushData(1636, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_Chuan01_X", typeof(AnimEnemyPro))
			},
			{
				1637,
				new BrushData(1637, "Base/Levels/Thief_2/Brushes/Enemy/Thief2_Enemy_Effect_Yu", typeof(EffectEnemy))
			},
			{
				1638,
				new BrushData(1638, "Base/Levels/Thief_2/Brushes/Enemy/Theif_Effect_Jump_UP2", typeof(TwoEffectTriggerPro))
			},
			{
				9000,
				new BrushData(9000, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_MagicBlack00", typeof(AnimEnemyPro))
			},
			{
				9001,
				new BrushData(9001, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_MagicBlack01", typeof(AnimEnemyPro))
			},
			{
				9002,
				new BrushData(9002, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_MagicBlack02", typeof(AnimEnemyPro))
			},
			{
				9003,
				new BrushData(9003, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_MagicBlack03", typeof(AnimEnemyPro))
			},
			{
				9004,
				new BrushData(9004, "Base/Levels/Level5/Brushes/Enemy/Fate_Enemy_MagicBlack04", typeof(AnimEnemyPro))
			},
			{
				9005,
				new BrushData(9005, "Base/Levels/Level5/Brushes/Enemy/Fate_Effect_Jump", typeof(TwoEffectTriggerPro))
			},
			{
				9006,
				new BrushData(9006, "Base/Levels/Level5/Brushes/Enemy/Fate_Effect_JumpTile", typeof(TwoEffectTriggerPro))
			},
			{
				10001,
				new BrushData(10001, "Base/Levels/Commons/Brushes/Enemy/Award_Crown", typeof(CrownAward))
			},
			{
				10002,
				new BrushData(10002, "Base/Levels/Commons/Brushes/Enemy/Award_Diamond", typeof(DiamondAward))
			},
			{
				10003,
				new BrushData(10003, "Base/Levels/Commons/Brushes/Enemy/Award_Diamond-3", typeof(DiamondAward))
			},
			{
				10004,
				new BrushData(10004, "Base/Levels/Commons/Brushes/Enemy/Crown_Fragment", typeof(CrownFragment))
			},
			{
				10005,
				new BrushData(10005, "Base/Levels/Commons/Brushes/Enemy/Crown_FromFragment", typeof(CrownFromFragment))
			},
			{
				10006,
				new BrushData(10006, "Base/Levels/Commons/Brushes/Enemy/Diamond_Fragment", typeof(DiamondFragment))
			},
			{
				10007,
				new BrushData(10007, "Base/Levels/Commons/Brushes/Enemy/Diamond_FromFragment", typeof(DiamondFromFragment))
			},
			{
				10008,
				new BrushData(10008, "Base/Levels/Commons/Brushes/Enemy/Theif_Drop_Crown", typeof(FreeMoveCrownByCouple))
			},
			{
				10009,
				new BrushData(10009, "Base/Levels/Commons/Brushes/Enemy/Theif_Drop_Diamond", typeof(FreeMoveDiamondByCouple))
			},
			{
				10011,
				new BrushData(10011, "Base/Levels/Commons/Brushes/Enemy/Tutorial_Award_Crown", typeof(CrownAward))
			},
			{
				10012,
				new BrushData(10012, "Base/Levels/Commons/Brushes/Enemy/Tutorial_Award_Diamond", typeof(DiamondAward))
			},
			{
				10013,
				new BrushData(10013, "Base/Levels/Commons/Brushes/Enemy/Award_Diamond_RainBow", typeof(DiamondAward))
			}
		};

		private static Dictionary<int, BrushData> m_midgroundBrushs = new Dictionary<int, BrushData>
		{
			{
				1,
				new BrushData(1, "Base/Levels/Level2/Brushes/Midground/AiJi_shamo", typeof(UnrecycleMidground))
			},
			{
				2,
				new BrushData(2, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_stone01", typeof(NormalMidground))
			},
			{
				3,
				new BrushData(3, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_stone02", typeof(NormalMidground))
			},
			{
				4,
				new BrushData(4, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_stone03", typeof(NormalMidground))
			},
			{
				5,
				new BrushData(5, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_stone04", typeof(NormalMidground))
			},
			{
				6,
				new BrushData(6, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_stone05", typeof(NormalMidground))
			},
			{
				7,
				new BrushData(7, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_zhu01", typeof(NormalMidground))
			},
			{
				8,
				new BrushData(8, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_zhu02", typeof(NormalMidground))
			},
			{
				9,
				new BrushData(9, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_stone02_1", typeof(NormalMidground))
			},
			{
				10,
				new BrushData(10, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_stone02_2", typeof(NormalMidground))
			},
			{
				11,
				new BrushData(11, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_stone02_3", typeof(NormalMidground))
			},
			{
				13,
				new BrushData(13, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_zhu01_2", typeof(NormalMidground))
			},
			{
				15,
				new BrushData(15, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_zhu01_5", typeof(NormalMidground))
			},
			{
				16,
				new BrushData(16, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_zhu01_7", typeof(NormalMidground))
			},
			{
				17,
				new BrushData(17, "Base/Levels/Level2/Brushes/Midground/AiJi_Shamo_zhu02_1", typeof(NormalMidground))
			},
			{
				19,
				new BrushData(19, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_Abubis", typeof(NormalMidground))
			},
			{
				20,
				new BrushData(20, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu01", typeof(NormalMidground))
			},
			{
				23,
				new BrushData(23, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu02", typeof(NormalMidground))
			},
			{
				24,
				new BrushData(24, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu03", typeof(NormalMidground))
			},
			{
				25,
				new BrushData(25, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu03_1", typeof(NormalMidground))
			},
			{
				26,
				new BrushData(26, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu03_2", typeof(NormalMidground))
			},
			{
				27,
				new BrushData(27, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu03_3", typeof(NormalMidground))
			},
			{
				29,
				new BrushData(29, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu05", typeof(NormalMidground))
			},
			{
				30,
				new BrushData(30, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu05_1", typeof(NormalMidground))
			},
			{
				31,
				new BrushData(31, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu05_2", typeof(NormalMidground))
			},
			{
				34,
				new BrushData(34, "Base/Levels/Level2/Brushes/Midground/AiJi_Redsea_Midground_shatan", typeof(UnrecycleMidground))
			},
			{
				35,
				new BrushData(35, "Base/Levels/Level2/Brushes/Midground/Midground_Shinei_DiaoXiang01", typeof(NormalMidground))
			},
			{
				36,
				new BrushData(36, "Base/Levels/Level2/Brushes/Midground/Midground_Shamo_FeiQIChuan", typeof(NormalMidground))
			},
			{
				37,
				new BrushData(37, "Base/Levels/Level2/Brushes/Midground/Midground_Shamo_Matou", typeof(UnrecycleMidground))
			},
			{
				38,
				new BrushData(38, "Base/Levels/Level2/Brushes/Midground/Midground_Shinei_Anubis_01", typeof(NormalMidground))
			},
			{
				39,
				new BrushData(39, "Base/Levels/Level2/Brushes/Midground/Midground_Shinei_dadizuo", typeof(NormalMidground))
			},
			{
				41,
				new BrushData(41, "Base/Levels/Level2/Brushes/Midground/Midground_Shinei_End_qiang", typeof(NormalMidground))
			},
			{
				42,
				new BrushData(42, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu06", typeof(NormalMidground))
			},
			{
				43,
				new BrushData(43, "Base/Levels/Level2/Brushes/Midground/Midground_Shinei_Ta01", typeof(NormalMidground))
			},
			{
				44,
				new BrushData(44, "Base/Levels/Level2/Brushes/Midground/AiJi_Redsea_Midground_NiLuoHe", typeof(UnrecycleMidground))
			},
			{
				45,
				new BrushData(45, "Base/Levels/Level2/Brushes/Midground/AiJi_shamo_start", typeof(UnrecycleMidground))
			},
			{
				46,
				new BrushData(46, "Base/Levels/Level2/Brushes/Midground/AiJi_Redsea_Shatan", typeof(UnrecycleMidground))
			},
			{
				47,
				new BrushData(47, "Base/Levels/Level1/Brushes/Midground/Tile_XiaoZhen_DiMian01", typeof(NormalMidground))
			},
			{
				56,
				new BrushData(56, "Base/Levels/Level1/Brushes/Midground/HongFangZi01_Open01", typeof(NormalMidground))
			},
			{
				57,
				new BrushData(57, "Base/Levels/Level1/Brushes/Midground/HongFangZi01_Open02", typeof(NormalMidground))
			},
			{
				58,
				new BrushData(58, "Base/Levels/Level1/Brushes/Midground/HongFangZi01_Open03", typeof(NormalMidground))
			},
			{
				59,
				new BrushData(59, "Base/Levels/Level1/Brushes/Midground/HongFangZi01_Open04", typeof(NormalMidground))
			},
			{
				60,
				new BrushData(60, "Base/Levels/Level1/Brushes/Midground/HuangFangZi01_Open01", typeof(NormalMidground))
			},
			{
				61,
				new BrushData(61, "Base/Levels/Level1/Brushes/Midground/HuangFangZi01_Open02", typeof(NormalMidground))
			},
			{
				62,
				new BrushData(62, "Base/Levels/Level1/Brushes/Midground/HuangFangZi01_Open03", typeof(NormalMidground))
			},
			{
				63,
				new BrushData(63, "Base/Levels/Level1/Brushes/Midground/LanFangZi01_Open01", typeof(NormalMidground))
			},
			{
				64,
				new BrushData(64, "Base/Levels/Level1/Brushes/Midground/LanFangZi01_Open02", typeof(NormalMidground))
			},
			{
				65,
				new BrushData(65, "Base/Levels/Level1/Brushes/Midground/LanFangZi01_Open03", typeof(NormalMidground))
			},
			{
				66,
				new BrushData(66, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShiQiang01", typeof(NormalMidground))
			},
			{
				67,
				new BrushData(67, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShiQiang02", typeof(NormalMidground))
			},
			{
				68,
				new BrushData(68, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShiQiang03", typeof(NormalMidground))
			},
			{
				69,
				new BrushData(69, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_HuaTan01", typeof(NormalMidground))
			},
			{
				70,
				new BrushData(70, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_HuaTan02", typeof(NormalMidground))
			},
			{
				71,
				new BrushData(71, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_HuaTan03", typeof(NormalMidground))
			},
			{
				72,
				new BrushData(72, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_HuaTan04", typeof(NormalMidground))
			},
			{
				73,
				new BrushData(73, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_HuaTan05", typeof(NormalMidground))
			},
			{
				74,
				new BrushData(74, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_HuaTan06", typeof(NormalMidground))
			},
			{
				75,
				new BrushData(75, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_Star_CaoDi", typeof(UnrecycleMidground))
			},
			{
				76,
				new BrushData(76, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_JiaoTangZhongLou", typeof(NormalMidground))
			},
			{
				78,
				new BrushData(78, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_Star_CaoCong", typeof(NormalMidground))
			},
			{
				79,
				new BrushData(79, "Base/Levels/Level1/Brushes/Midground/Midground_FlowerSea", typeof(NormalMidground))
			},
			{
				80,
				new BrushData(80, "Base/Levels/Level1/Brushes/Midground/Planet", typeof(NormalMidground))
			},
			{
				81,
				new BrushData(81, "Base/Levels/Level2/Brushes/Midground/AiJi_Shinei_zhu04", typeof(NormalMidground))
			},
			{
				82,
				new BrushData(82, "Base/Levels/Level1/Brushes/Midground/AiJi_Shinei_zhu05_3", typeof(NormalMidground))
			},
			{
				300,
				new BrushData(300, "Base/Levels/Level1/Brushes/Midground/Midground_HongFangZi01", typeof(NormalMidground))
			},
			{
				301,
				new BrushData(301, "Base/Levels/Level1/Brushes/Midground/Midground_HongFangZi02", typeof(NormalMidground))
			},
			{
				302,
				new BrushData(302, "Base/Levels/Level1/Brushes/Midground/Midground_HongFangZi03", typeof(NormalMidground))
			},
			{
				303,
				new BrushData(303, "Base/Levels/Level1/Brushes/Midground/Midground_HongFangZi_WuDing01", typeof(NormalMidground))
			},
			{
				304,
				new BrushData(304, "Base/Levels/Level1/Brushes/Midground/Midground_HongFangZi_WuDing02", typeof(NormalMidground))
			},
			{
				305,
				new BrushData(305, "Base/Levels/Level1/Brushes/Midground/Midground_HongFangZi_WuDing_ShiKuai", typeof(NormalMidground))
			},
			{
				306,
				new BrushData(306, "Base/Levels/Level1/Brushes/Midground/Midground_HongFangZi_YuDa", typeof(NormalMidground))
			},
			{
				307,
				new BrushData(307, "Base/Levels/Level1/Brushes/Midground/Midground_HuangFangZi01", typeof(NormalMidground))
			},
			{
				308,
				new BrushData(308, "Base/Levels/Level1/Brushes/Midground/Midground_HuangFangZi02", typeof(NormalMidground))
			},
			{
				309,
				new BrushData(309, "Base/Levels/Level1/Brushes/Midground/Midground_HuangFangZi_YuDa", typeof(NormalMidground))
			},
			{
				310,
				new BrushData(310, "Base/Levels/Level1/Brushes/Midground/Midground_JiaoTang_ZhongLou", typeof(NormalMidground))
			},
			{
				311,
				new BrushData(311, "Base/Levels/Level1/Brushes/Midground/Midground_JiaoTang_Qiang01", typeof(NormalMidground))
			},
			{
				312,
				new BrushData(312, "Base/Levels/Level1/Brushes/Midground/Midground_LanFangZi01", typeof(NormalMidground))
			},
			{
				313,
				new BrushData(313, "Base/Levels/Level1/Brushes/Midground/Midground_LanFangZi02", typeof(NormalMidground))
			},
			{
				314,
				new BrushData(314, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShiQiang01_01", typeof(NormalMidground))
			},
			{
				315,
				new BrushData(315, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShiQiang01_02", typeof(NormalMidground))
			},
			{
				316,
				new BrushData(316, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShiQiang01_03", typeof(NormalMidground))
			},
			{
				317,
				new BrushData(317, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShiQiang02_01", typeof(NormalMidground))
			},
			{
				318,
				new BrushData(318, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShiQiang02_02", typeof(NormalMidground))
			},
			{
				319,
				new BrushData(319, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShiQiang03", typeof(NormalMidground))
			},
			{
				320,
				new BrushData(320, "Base/Levels/Level1/Brushes/Midground/Midground_HongFangZi04", typeof(NormalMidground))
			},
			{
				321,
				new BrushData(321, "Base/Levels/Level1/Brushes/Midground/Midground_LanFangZi03", typeof(NormalMidground))
			},
			{
				322,
				new BrushData(322, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_GuaPai01", typeof(NormalMidground))
			},
			{
				323,
				new BrushData(323, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_GuaPai02", typeof(NormalMidground))
			},
			{
				324,
				new BrushData(324, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_GuaPai03", typeof(NormalMidground))
			},
			{
				325,
				new BrushData(325, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_GuaPai04", typeof(NormalMidground))
			},
			{
				326,
				new BrushData(326, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_JiZhiZhen", typeof(NormalMidground))
			},
			{
				327,
				new BrushData(327, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_LuBianTan01", typeof(NormalMidground))
			},
			{
				328,
				new BrushData(328, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_LuBianTan02", typeof(NormalMidground))
			},
			{
				329,
				new BrushData(329, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShuiGuoTan", typeof(NormalMidground))
			},
			{
				330,
				new BrushData(330, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShuiGuo01", typeof(NormalMidground))
			},
			{
				331,
				new BrushData(331, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShuiGuo02", typeof(NormalMidground))
			},
			{
				332,
				new BrushData(332, "Base/Levels/Level1/Brushes/Midground/XiaoZhen_ShuiGuo03", typeof(NormalMidground))
			},
			{
				333,
				new BrushData(333, "Base/Levels/Level1/Brushes/Midground/Midground_ShuCha01", typeof(NormalMidground))
			},
			{
				334,
				new BrushData(334, "Base/Levels/Level1/Brushes/Midground/Midground_ShuCha02", typeof(NormalMidground))
			},
			{
				335,
				new BrushData(335, "Base/Levels/Level1/Brushes/Midground/Midground_ShuCha03", typeof(NormalMidground))
			},
			{
				336,
				new BrushData(336, "Base/Levels/Level1/Brushes/Midground/Midground_ShuCha04", typeof(NormalMidground))
			},
			{
				337,
				new BrushData(337, "Base/Levels/Level1/Brushes/Midground/Midground_ShuCha05", typeof(NormalMidground))
			},
			{
				338,
				new BrushData(338, "Base/Levels/Level1/Brushes/Midground/Midground_ShuCha06", typeof(NormalMidground))
			},
			{
				339,
				new BrushData(339, "Base/Levels/Level1/Brushes/Midground/Midground_Yu", typeof(NormalMidground))
			},
			{
				401,
				new BrushData(401, "Base/Levels/Level5/Brushes/Midground/Fate_Midground_qipan_white", typeof(NormalMidground))
			},
			{
				402,
				new BrushData(402, "Base/Levels/Level5/Brushes/Midground/Fate_Midground_qipan_black", typeof(NormalMidground))
			},
			{
				501,
				new BrushData(501, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdi_01", typeof(NormalMidground))
			},
			{
				502,
				new BrushData(502, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdi_02", typeof(NormalMidground))
			},
			{
				503,
				new BrushData(503, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdian_01", typeof(NormalMidground))
			},
			{
				504,
				new BrushData(504, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdian_02", typeof(NormalMidground))
			},
			{
				505,
				new BrushData(505, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdian_03", typeof(NormalMidground))
			},
			{
				506,
				new BrushData(506, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_shamo_01", typeof(UnrecycleMidground))
			},
			{
				507,
				new BrushData(507, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_shamo_02", typeof(UnrecycleMidground))
			},
			{
				508,
				new BrushData(508, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdian_04", typeof(NormalMidground))
			},
			{
				512,
				new BrushData(512, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdian_08", typeof(NormalMidground))
			},
			{
				513,
				new BrushData(513, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdi_03", typeof(NormalMidground))
			},
			{
				514,
				new BrushData(514, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_shamo_xianjie1", typeof(UnrecycleMidground))
			},
			{
				515,
				new BrushData(515, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_shamo_xianjie2", typeof(UnrecycleMidground))
			},
			{
				520,
				new BrushData(520, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdian_09", typeof(NormalMidground))
			},
			{
				522,
				new BrushData(522, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_shamo_xianjie3", typeof(UnrecycleMidground))
			},
			{
				523,
				new BrushData(523, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_shamo_xiapo", typeof(UnrecycleMidground))
			},
			{
				524,
				new BrushData(524, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_gongdian_start", typeof(NormalMidground))
			},
			{
				525,
				new BrushData(525, "Base/Levels/LightMap/Brush/Midground/Level1/AiJi_shamo_03", typeof(UnrecycleMidground))
			},
			{
				526,
				new BrushData(526, "Base/Levels/LightMap/Brush/Midground/Level1/Background_end", typeof(UnrecycleMidground))
			},
			{
				527,
				new BrushData(527, "Base/Levels/LightMap/Brush/Midground/Level1/Aiji_GongDi_huopen_2", typeof(NormalMidground))
			},
			{
				1000,
				new BrushData(1000, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan", typeof(NormalMidground))
			},
			{
				1001,
				new BrushData(1001, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_01", typeof(NormalMidground))
			},
			{
				1002,
				new BrushData(1002, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_02", typeof(NormalMidground))
			},
			{
				1003,
				new BrushData(1003, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_03", typeof(NormalMidground))
			},
			{
				1004,
				new BrushData(1004, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_04", typeof(NormalMidground))
			},
			{
				1005,
				new BrushData(1005, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_05", typeof(NormalMidground))
			},
			{
				1006,
				new BrushData(1006, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_06", typeof(NormalMidground))
			},
			{
				1007,
				new BrushData(1007, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_07", typeof(NormalMidground))
			},
			{
				1008,
				new BrushData(1008, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_08", typeof(NormalMidground))
			},
			{
				1009,
				new BrushData(1009, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_09", typeof(NormalMidground))
			},
			{
				1010,
				new BrushData(1010, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Danhuangguan_00", typeof(NormalMidground))
			},
			{
				1101,
				new BrushData(1101, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Gu", typeof(SynchronizedAnimationMidground))
			},
			{
				1102,
				new BrushData(1102, "Base/Levels/Fantasia_Jazz/Brushes/Midground/Jazz_Midground_Gu_S", typeof(SynchronizedAnimationMidground))
			},
			{
				1501,
				new BrushData(1501, "Base/Levels/Samurai/Brushes/Midground/Samurai_Tile_Muban", typeof(NormalMidground))
			}
		};

		private static Dictionary<int, BrushData> m_triggerBoxBrushs = new Dictionary<int, BrushData>
		{
			{
				1,
				new BrushData(1, "Base/Levels/Level1/Brushes/TriggerBox/Trigger_WinBeforeFinish", typeof(WinBeforeFinishTrigger))
			},
			{
				2,
				new BrushData(2, "Base/Levels/Level1/Brushes/TriggerBox/Trigger_StartToolTrigger", typeof(StartToolTrigger))
			},
			{
				3,
				new BrushData(3, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_Effect_jiantou02", typeof(PathGuideTrigger))
			},
			{
				4,
				new BrushData(4, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_Ship_Separate", typeof(DepartVehicleTrigger))
			},
			{
				5,
				new BrushData(5, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_MaTou01", typeof(CameraAnimTrigger))
			},
			{
				6,
				new BrushData(6, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_MaTou02", typeof(CameraAnimTrigger))
			},
			{
				7,
				new BrushData(7, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_MaTou03", typeof(CameraAnimTrigger))
			},
			{
				8,
				new BrushData(8, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_MaTou_END", typeof(CameraAnimTrigger))
			},
			{
				10,
				new BrushData(10, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan01", typeof(CameraAnimTrigger))
			},
			{
				11,
				new BrushData(11, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_ANBS_END", typeof(CameraAnimTrigger))
			},
			{
				12,
				new BrushData(12, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_BackGroundTrigger", typeof(BackGroundTrigger))
			},
			{
				13,
				new BrushData(13, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_ShiNei01", typeof(CameraAnimTrigger))
			},
			{
				14,
				new BrushData(14, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_ShiNei02", typeof(CameraAnimTrigger))
			},
			{
				15,
				new BrushData(15, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_ShiNei03", typeof(CameraAnimTrigger))
			},
			{
				16,
				new BrushData(16, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_ShiNei_End", typeof(CameraAnimTrigger))
			},
			{
				17,
				new BrushData(17, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QTE_ANBS00", typeof(CameraAnimTrigger))
			},
			{
				18,
				new BrushData(18, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_EnableInput", typeof(EnableInputTrigger))
			},
			{
				19,
				new BrushData(19, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_DisableInput", typeof(DisableInputTrigger))
			},
			{
				20,
				new BrushData(20, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan04", typeof(CameraAnimTrigger))
			},
			{
				21,
				new BrushData(21, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan05", typeof(CameraAnimTrigger))
			},
			{
				22,
				new BrushData(22, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan06", typeof(CameraAnimTrigger))
			},
			{
				23,
				new BrushData(23, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_ShakeCamera", typeof(ShakeCameraTrigger))
			},
			{
				24,
				new BrushData(24, "Base/Levels/Level1/Brushes/TriggerBox/Effect_LV1_QTE_new", typeof(AnimEffectTrigger))
			},
			{
				25,
				new BrushData(25, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_TreeKill", typeof(TreeKillTrigger))
			},
			{
				27,
				new BrushData(27, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShaMo_DuoDuanTiao01", typeof(CameraAnimTrigger))
			},
			{
				28,
				new BrushData(28, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShaMo_DuoDuanTiao02", typeof(CameraAnimTrigger))
			},
			{
				29,
				new BrushData(29, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_UpMounts_Eagle", typeof(UpMountsTrigger))
			},
			{
				30,
				new BrushData(30, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_Eagle_ShaMo01", typeof(CameraAnimTrigger))
			},
			{
				31,
				new BrushData(31, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShiNei_Shuangceng01", typeof(CameraAnimTrigger))
			},
			{
				32,
				new BrushData(32, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShiNei_Shuangceng02", typeof(CameraAnimTrigger))
			},
			{
				33,
				new BrushData(33, "Base/Levels/Level2/Brushes/TriggerBox/Effect_ShiNei_ENDJump", typeof(AnimEffectTrigger))
			},
			{
				34,
				new BrushData(34, "Base/Levels/Level2/Brushes/TriggerBox/Effect_Eagle_Ride", typeof(AnimEffectTrigger))
			},
			{
				35,
				new BrushData(35, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_Input_Param", typeof(InputResetTrigger))
			},
			{
				36,
				new BrushData(36, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShaMo_DuoDuanTiao03_1", typeof(CameraAnimTrigger))
			},
			{
				37,
				new BrushData(37, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShaMo_DuoDuanTiao03_2", typeof(CameraAnimTrigger))
			},
			{
				38,
				new BrushData(38, "Base/Levels/Level1/Brushes/TriggerBox/WorldThemesTrigger", typeof(WorldThemesTrigger))
			},
			{
				39,
				new BrushData(39, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_DownMounts_Eagle", typeof(DownMountsTrigger))
			},
			{
				40,
				new BrushData(40, "Base/Levels/Level1/Brushes/TriggerBox/TriggerDropDie", typeof(DropDieTrigger))
			},
			{
				41,
				new BrushData(41, "Base/Levels/Level1/Brushes/TriggerBox/AutoMoveJumpTrigger3", typeof(AutoMoveJumpTrigger))
			},
			{
				42,
				new BrushData(42, "Base/Levels/Level1/Brushes/TriggerBox/TriggerFreeMove", typeof(FreeMoveTrigger))
			},
			{
				44,
				new BrushData(44, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShaMo_DuoDuanTiao03_3", typeof(CameraAnimTrigger))
			},
			{
				45,
				new BrushData(45, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShaMo_DuoDuanTiao03_4", typeof(CameraAnimTrigger))
			},
			{
				46,
				new BrushData(46, "Base/Levels/Level2/Brushes/TriggerBox/test", typeof(CameraAnimTrigger))
			},
			{
				47,
				new BrushData(47, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan07", typeof(CameraAnimTrigger))
			},
			{
				48,
				new BrushData(48, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan08", typeof(CameraAnimTrigger))
			},
			{
				49,
				new BrushData(49, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan09", typeof(CameraAnimTrigger))
			},
			{
				50,
				new BrushData(50, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan10", typeof(CameraAnimTrigger))
			},
			{
				51,
				new BrushData(51, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_Trigger_MagicCube_Left", typeof(MagicBoxTrigger))
			},
			{
				52,
				new BrushData(52, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_Trigger_MagicCube_Right", typeof(MagicBoxTrigger))
			},
			{
				53,
				new BrushData(53, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_Trigger_MagicCube_Cycle", typeof(MagicBoxTrigger))
			},
			{
				54,
				new BrushData(54, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShiNei_DaTiao01", typeof(CameraAnimTrigger))
			},
			{
				55,
				new BrushData(55, "Base/Levels/Level2/Brushes/TriggerBox/ForceHorizonMoveTrigger", typeof(ForceHorizonTrigger))
			},
			{
				57,
				new BrushData(57, "Base/Levels/Level1/Brushes/TriggerBox/AutoMoveJumpTrigger4", typeof(AutoMoveJumpTrigger))
			},
			{
				59,
				new BrushData(59, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing01", typeof(CameraAnimTrigger))
			},
			{
				60,
				new BrushData(60, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing02", typeof(CameraAnimTrigger))
			},
			{
				61,
				new BrushData(61, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing03", typeof(CameraAnimTrigger))
			},
			{
				62,
				new BrushData(62, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing04", typeof(CameraAnimTrigger))
			},
			{
				63,
				new BrushData(63, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing05", typeof(CameraAnimTrigger))
			},
			{
				64,
				new BrushData(64, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing06", typeof(CameraAnimTrigger))
			},
			{
				65,
				new BrushData(65, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing07", typeof(CameraAnimTrigger))
			},
			{
				66,
				new BrushData(66, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing08", typeof(CameraAnimTrigger))
			},
			{
				67,
				new BrushData(67, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing09", typeof(CameraAnimTrigger))
			},
			{
				68,
				new BrushData(68, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_HuaHai01", typeof(CameraAnimTrigger))
			},
			{
				69,
				new BrushData(69, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_TaiKong01", typeof(CameraAnimTrigger))
			},
			{
				70,
				new BrushData(70, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_TaiKong02", typeof(CameraAnimTrigger))
			},
			{
				71,
				new BrushData(71, "Base/Levels/Level1/Brushes/TriggerBox/ElevatorUpTrigger", typeof(ElevatorTrigger))
			},
			{
				72,
				new BrushData(72, "Base/Levels/Level1/Brushes/TriggerBox/ElevatorDownTrigger", typeof(ElevatorTrigger))
			},
			{
				73,
				new BrushData(73, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing10", typeof(CameraAnimTrigger))
			},
			{
				74,
				new BrushData(74, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing11", typeof(CameraAnimTrigger))
			},
			{
				75,
				new BrushData(75, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing12", typeof(CameraAnimTrigger))
			},
			{
				76,
				new BrushData(76, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing13", typeof(CameraAnimTrigger))
			},
			{
				77,
				new BrushData(77, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing14", typeof(CameraAnimTrigger))
			},
			{
				78,
				new BrushData(78, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing15", typeof(CameraAnimTrigger))
			},
			{
				79,
				new BrushData(79, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_WuDing16", typeof(CameraAnimTrigger))
			},
			{
				80,
				new BrushData(80, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_TaiKong03", typeof(CameraAnimTrigger))
			},
			{
				81,
				new BrushData(81, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_TaiKong04", typeof(CameraAnimTrigger))
			},
			{
				82,
				new BrushData(82, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_TaiKong05", typeof(CameraAnimTrigger))
			},
			{
				83,
				new BrushData(83, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_TaiKong06", typeof(CameraAnimTrigger))
			},
			{
				84,
				new BrushData(84, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_END01", typeof(CameraAnimTrigger))
			},
			{
				85,
				new BrushData(85, "Base/Levels/Level1/Brushes/TriggerBox/ChangeBackgroundTrigger", typeof(ChangeBackgroundTrigger))
			},
			{
				86,
				new BrushData(86, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_XiaoZhen_WuDing01", typeof(CameraAnimTrigger))
			},
			{
				87,
				new BrushData(87, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_XiaoZhen_WuDing02", typeof(CameraAnimTrigger))
			},
			{
				88,
				new BrushData(88, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_XiaoZhen_WuDing03", typeof(CameraAnimTrigger))
			},
			{
				89,
				new BrushData(89, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_XiaoZhen_WuDing04", typeof(CameraAnimTrigger))
			},
			{
				90,
				new BrushData(90, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_XiaoZhen_WuDing05", typeof(CameraAnimTrigger))
			},
			{
				91,
				new BrushData(91, "Base/Levels/Level1/Brushes/TriggerBox/Effect_AutoMoveJumpTrigger", typeof(AnimEffectTrigger))
			},
			{
				92,
				new BrushData(92, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_END02", typeof(CameraAnimTrigger))
			},
			{
				93,
				new BrushData(93, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_XiaoZhen_WuDing01_End", typeof(CameraAnimTrigger))
			},
			{
				94,
				new BrushData(94, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_Yangshi", typeof(CameraAnimTrigger))
			},
			{
				95,
				new BrushData(95, "Base/Levels/Level1/Brushes/TriggerBox/XiaoWangZi_CamAN_Yangshi_huifu", typeof(CameraAnimTrigger))
			},
			{
				96,
				new BrushData(96, "Base/Levels/Level1/Brushes/TriggerBox/Trigger_BlackSand", typeof(BlackSandTrigger))
			},
			{
				97,
				new BrushData(97, "Base/Levels/Level2/Brushes/TriggerBox/TriggerDropDie_lv2", typeof(DropDieTrigger))
			},
			{
				98,
				new BrushData(98, "Base/Levels/Level2/Brushes/TriggerBox/Effect_LV1_QTE_new_lv2", typeof(AnimEffectTrigger))
			},
			{
				99,
				new BrushData(99, "Base/Levels/Level2/Brushes/TriggerBox/WorldThemesTrigger_lv2", typeof(WorldThemesTrigger))
			},
			{
				100,
				new BrushData(100, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_ShaMo_Bug01", typeof(CameraAnimTrigger))
			},
			{
				101,
				new BrushData(101, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan_super", typeof(CameraAnimTrigger))
			},
			{
				102,
				new BrushData(102, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan_super01", typeof(CameraAnimTrigger))
			},
			{
				103,
				new BrushData(103, "Base/Levels/Level2/Brushes/TriggerBox/Effect_LV1_QTE_new_Road_lv2", typeof(AnimEffectTrigger))
			},
			{
				104,
				new BrushData(104, "Brush/TriggerBox/CurvedBendBoxTrigger", typeof(CurvedBendBoxTrigger))
			},
			{
				105,
				new BrushData(105, "Base/Levels/LightMap/Brush/TriggerBox/RebirthBoxTrigger_StarryDream", typeof(RebirthBoxTrigger))
			},
			{
				106,
				new BrushData(106, "Base/Levels/Level1/Brushes/TriggerBox/CameraClipTrigger", typeof(CameraClipTrigger))
			},
			{
				107,
				new BrushData(107, "Base/Levels/Level1/Brushes/TriggerBox/Back_XiaoWangZi_Sky", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				108,
				new BrushData(108, "Base/Levels/Level1/Brushes/TriggerBox/WXZ_Trigger_CameraParticlePlayByName", typeof(ChangeCameraEffectByNameTrigger))
			},
			{
				109,
				new BrushData(109, "Base/Levels/LightMap/Brush/TriggerBox/Pharaohs_Trigger_CameraParticlePlayByName", typeof(ChangeCameraEffectByNameTrigger))
			},
			{
				111,
				new BrushData(111, "Brush/TriggerBox/OriginRebirthTrigger", typeof(OriginRebirthTrigger))
			},
			{
				114,
				new BrushData(114, "Base/Levels/Level2/Brushes/TriggerBox/Pharaohs_sub_CamEnd_1", typeof(CameraAnimTrigger))
			},
			{
				200,
				new BrushData(200, "Base/Levels/Tutorial/Brushes/TriggerBox/TutorialEndTrigger", typeof(TutorialEndTrigger))
			},
			{
				201,
				new BrushData(201, "Base/Levels/Tutorial/Brushes/TriggerBox/TutorialEndTrigger_Big", typeof(TutorialEndTrigger))
			},
			{
				301,
				new BrushData(301, "Base/Levels/Level1/Brushes/TriggerBox/Xiaowangzi_RoleMoveLimitTrigger", typeof(RoleMoveLimitTrigger))
			},
			{
				302,
				new BrushData(302, "Base/Levels/Level1/Brushes/TriggerBox/Trigger_Road_Shining", typeof(EmissionTileTrigger))
			},
			{
				303,
				new BrushData(303, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_Snake_Ship_Separate", typeof(SnakeDepartVehicleTrigger))
			},
			{
				304,
				new BrushData(304, "Base/Levels/Level1/Brushes/TriggerBox/Trigger_PathToMove_Pet", typeof(PathToMoveByPetTrigger))
			},
			{
				305,
				new BrushData(305, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_WaterKill", typeof(WaterKillTrigger))
			},
			{
				306,
				new BrushData(306, "Base/Levels/Level1/Brushes/TriggerBox/Effect_ElevatorDownTrigger", typeof(AnimEffectTrigger))
			},
			{
				307,
				new BrushData(307, "Base/Levels/Level1/Brushes/TriggerBox/Effect_ElevatorUpTrigger", typeof(AnimEffectTrigger))
			},
			{
				308,
				new BrushData(308, "Base/Levels/Level1/Brushes/TriggerBox/Effect_XingKong_JumpStar", typeof(AnimEffectTrigger))
			},
			{
				309,
				new BrushData(309, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_01", typeof(CameraAnimTrigger))
			},
			{
				310,
				new BrushData(310, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_02", typeof(CameraAnimTrigger))
			},
			{
				311,
				new BrushData(311, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_03", typeof(CameraAnimTrigger))
			},
			{
				312,
				new BrushData(312, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_04", typeof(CameraAnimTrigger))
			},
			{
				313,
				new BrushData(313, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_05", typeof(CameraAnimTrigger))
			},
			{
				314,
				new BrushData(314, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_END", typeof(CameraAnimTrigger))
			},
			{
				315,
				new BrushData(315, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_HuaXiang_01", typeof(CameraAnimTrigger))
			},
			{
				316,
				new BrushData(316, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_05", typeof(CameraAnimTrigger))
			},
			{
				317,
				new BrushData(317, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_06", typeof(CameraAnimTrigger))
			},
			{
				318,
				new BrushData(318, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_07", typeof(CameraAnimTrigger))
			},
			{
				319,
				new BrushData(319, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_08", typeof(CameraAnimTrigger))
			},
			{
				320,
				new BrushData(320, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_09", typeof(CameraAnimTrigger))
			},
			{
				321,
				new BrushData(321, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_10", typeof(CameraAnimTrigger))
			},
			{
				322,
				new BrushData(322, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_11", typeof(CameraAnimTrigger))
			},
			{
				323,
				new BrushData(323, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_12", typeof(CameraAnimTrigger))
			},
			{
				324,
				new BrushData(324, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_13", typeof(CameraAnimTrigger))
			},
			{
				325,
				new BrushData(325, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_14", typeof(CameraAnimTrigger))
			},
			{
				326,
				new BrushData(326, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_15", typeof(CameraAnimTrigger))
			},
			{
				327,
				new BrushData(327, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_16", typeof(CameraAnimTrigger))
			},
			{
				328,
				new BrushData(328, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_17", typeof(CameraAnimTrigger))
			},
			{
				329,
				new BrushData(329, "Base/Levels/Level3/Brushes/TriggerBox/WheatPlayerTrigger", typeof(EmissionTileTrigger))
			},
			{
				330,
				new BrushData(330, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_MaiTian_01", typeof(CameraAnimTrigger))
			},
			{
				331,
				new BrushData(331, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_MaiTian_02", typeof(CameraAnimTrigger))
			},
			{
				332,
				new BrushData(332, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_MaiTian_03", typeof(CameraAnimTrigger))
			},
			{
				333,
				new BrushData(333, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_MaiTian_04", typeof(CameraAnimTrigger))
			},
			{
				334,
				new BrushData(334, "Base/Levels/Level3/Brushes/TriggerBox/Effect_YuSan", typeof(AnimEffectTrigger))
			},
			{
				335,
				new BrushData(335, "Base/Levels/Level3/Brushes/TriggerBox/CrownFragmentTrigger", typeof(CrownFragmentTrigger))
			},
			{
				336,
				new BrushData(336, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_00", typeof(CameraAnimTrigger))
			},
			{
				337,
				new BrushData(337, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_Skateboard_BaiRiMeng", typeof(NormalSkateboardVehicle))
			},
			{
				338,
				new BrushData(338, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_06", typeof(CameraAnimTrigger))
			},
			{
				339,
				new BrushData(339, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_07", typeof(CameraAnimTrigger))
			},
			{
				340,
				new BrushData(340, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_DaNiao_08", typeof(CameraAnimTrigger))
			},
			{
				341,
				new BrushData(341, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_Effect_huaban", typeof(MountEffectToRoleTrigger))
			},
			{
				342,
				new BrushData(342, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_PathToMove_FixedByRoleAsFragement", typeof(PathToMoveFixedByRoleAsFragementTrigger))
			},
			{
				343,
				new BrushData(343, "Base/Levels/Level3/Brushes/TriggerBox/TriggerFragmentTrigger", typeof(TriggerFragmentTrigger))
			},
			{
				344,
				new BrushData(344, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_PathToMove_LessThan", typeof(PathToMoveByRoleAsFragementTrigger))
			},
			{
				345,
				new BrushData(345, "Base/Levels/Level3/Brushes/TriggerBox/HOME_PathToMove_Down", typeof(CameraAnimAsFragementTrigger))
			},
			{
				346,
				new BrushData(346, "Base/Levels/Level3/Brushes/TriggerBox/HOME_PathToMove_UP", typeof(CameraAnimAsFragementTrigger))
			},
			{
				347,
				new BrushData(347, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_04", typeof(CameraAnimTrigger))
			},
			{
				348,
				new BrushData(348, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_Ship_Separate", typeof(DepartVehicleAsFragementTrigger))
			},
			{
				349,
				new BrushData(349, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_Skateboard_Snow", typeof(NormalSkateboardVehicle))
			},
			{
				350,
				new BrushData(350, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_18", typeof(CameraAnimTrigger))
			},
			{
				351,
				new BrushData(351, "Base/Levels/Level3/Brushes/TriggerBox/Home_XueDong_01", typeof(CameraAnimTrigger))
			},
			{
				352,
				new BrushData(352, "Base/Levels/Level3/Brushes/TriggerBox/Home_XueDong_02", typeof(CameraAnimTrigger))
			},
			{
				353,
				new BrushData(353, "Base/Levels/Level3/Brushes/TriggerBox/Home_XueDong_03", typeof(CameraAnimTrigger))
			},
			{
				354,
				new BrushData(354, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_19", typeof(CameraAnimTrigger))
			},
			{
				355,
				new BrushData(355, "Base/Levels/Level3/Brushes/TriggerBox/Home_ShengShan_01", typeof(CameraAnimTrigger))
			},
			{
				356,
				new BrushData(356, "Base/Levels/Level3/Brushes/TriggerBox/Home_ShengShan_02", typeof(CameraAnimTrigger))
			},
			{
				357,
				new BrushData(357, "Base/Levels/Level3/Brushes/TriggerBox/HOME_CameraBlankTrigger", typeof(CameraBlankTrigger))
			},
			{
				358,
				new BrushData(358, "Base/Levels/Level3/Brushes/TriggerBox/Home_ShengShan_A", typeof(CameraAnimTrigger))
			},
			{
				359,
				new BrushData(359, "Base/Levels/Level3/Brushes/TriggerBox/Home_ShengShan_End", typeof(CameraAnimTrigger))
			},
			{
				360,
				new BrushData(360, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_Wind", typeof(OpenFollowTrigger))
			},
			{
				361,
				new BrushData(361, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_Wind", typeof(CloseFollowTrigger))
			},
			{
				362,
				new BrushData(362, "Base/Levels/Level3/Brushes/TriggerBox/HOME_PathToMove_Down01", typeof(CameraAnimAsFragementTrigger))
			},
			{
				363,
				new BrushData(363, "Base/Levels/Level3/Brushes/TriggerBox/HOME_PathToMove_UP01", typeof(CameraAnimAsFragementTrigger))
			},
			{
				364,
				new BrushData(364, "Base/Levels/Level3/Brushes/TriggerBox/Home_ShengShan_03", typeof(CameraAnimTrigger))
			},
			{
				365,
				new BrushData(365, "Base/Levels/Level3/Brushes/TriggerBox/Home_ShengShan_04", typeof(CameraAnimTrigger))
			},
			{
				366,
				new BrushData(366, "Base/Levels/Level3/Brushes/TriggerBox/Home_RoleMoveLimitTrigger", typeof(RoleMoveLimitTrigger))
			},
			{
				367,
				new BrushData(367, "Base/Levels/Level3/Brushes/TriggerBox/Home_Trigger_DropDieForword", typeof(DropDieForwordTrigger))
			},
			{
				368,
				new BrushData(368, "Base/Levels/Level3/Brushes/TriggerBox/Home_Trigger_DropDieStatic", typeof(DropDieStaticTrigger))
			},
			{
				369,
				new BrushData(369, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_Home_ShakeCamera", typeof(ShakeCameraTrigger))
			},
			{
				370,
				new BrushData(370, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_08_NEW", typeof(CameraAnimTrigger))
			},
			{
				371,
				new BrushData(371, "Base/Levels/Level3/Brushes/TriggerBox/BaiRiMeng_JingYu_04_NEW", typeof(CameraAnimTrigger))
			},
			{
				401,
				new BrushData(401, "Base/Levels/Level5/Brushes/TriggerBox/Fate_Trigger_WorldThemes", typeof(WorldThemesTrigger))
			},
			{
				402,
				new BrushData(402, "Base/Levels/Level5/Brushes/TriggerBox/Fate_Trigger_Road_Shining", typeof(EmissionTileTrigger))
			},
			{
				405,
				new BrushData(405, "Base/Levels/Level5/Brushes/TriggerBox/Fate_MoveForwardDir", typeof(MoveDirChangeTrigger))
			},
			{
				406,
				new BrushData(406, "Base/Levels/Level5/Brushes/TriggerBox/Fate_MoveBackwardDir", typeof(MoveDirChangeTrigger))
			},
			{
				407,
				new BrushData(407, "Base/Levels/Level5/Brushes/TriggerBox/Fate_JumpForwardDir", typeof(JumpDirTrigger))
			},
			{
				408,
				new BrushData(408, "Base/Levels/Level5/Brushes/TriggerBox/Fate_JumpBackwardDir", typeof(JumpDirTrigger))
			},
			{
				411,
				new BrushData(411, "Base/Levels/Level5/Brushes/TriggerBox/Trigger_Dancer", typeof(NPCDancerVehicle))
			},
			{
				412,
				new BrushData(412, "Base/Levels/Level5/Brushes/TriggerBox/Trigger_ChangeSpeed", typeof(ChangeRailwaySpeedTrigger))
			},
			{
				413,
				new BrushData(413, "Base/Levels/Level5/Brushes/TriggerBox/Trigger_JumpDancer", typeof(JumpDancerTrigger))
			},
			{
				414,
				new BrushData(414, "Base/Levels/Level5/Brushes/TriggerBox/Trigger_FlashMoveTrigger", typeof(FlashMoveTrigger))
			},
			{
				425,
				new BrushData(425, "Base/Levels/Level5/Brushes/TriggerBox/Fate_RoleMoveLimitTrigger", typeof(RoleMoveLimitTrigger))
			},
			{
				442,
				new BrushData(442, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CameraBlankTrigger", typeof(CameraBlankTrigger))
			},
			{
				451,
				new BrushData(451, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_01", typeof(CameraAnimTrigger))
			},
			{
				452,
				new BrushData(452, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_02", typeof(CameraAnimTrigger))
			},
			{
				453,
				new BrushData(453, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_03", typeof(CameraAnimTrigger))
			},
			{
				454,
				new BrushData(454, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_04", typeof(CameraAnimTrigger))
			},
			{
				455,
				new BrushData(455, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_05", typeof(CameraAnimTrigger))
			},
			{
				456,
				new BrushData(456, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_06", typeof(CameraAnimTrigger))
			},
			{
				457,
				new BrushData(457, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_07", typeof(CameraAnimTrigger))
			},
			{
				458,
				new BrushData(458, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_08", typeof(CameraAnimTrigger))
			},
			{
				459,
				new BrushData(459, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_09", typeof(CameraAnimTrigger))
			},
			{
				460,
				new BrushData(460, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_10", typeof(CameraAnimTrigger))
			},
			{
				461,
				new BrushData(461, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_QiPan01", typeof(CameraAnimTrigger))
			},
			{
				462,
				new BrushData(462, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_End", typeof(CameraAnimTrigger))
			},
			{
				463,
				new BrushData(463, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_QiPan02", typeof(CameraAnimTrigger))
			},
			{
				464,
				new BrushData(464, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_QiPan03", typeof(CameraAnimTrigger))
			},
			{
				465,
				new BrushData(465, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_Toxiaqi", typeof(CameraAnimTrigger))
			},
			{
				466,
				new BrushData(466, "Base/Levels/Level5/Brushes/TriggerBox/Trigger_RoleAnimatior", typeof(RoleAnimatiorTrigger))
			},
			{
				467,
				new BrushData(467, "Base/Levels/Level5/Brushes/TriggerBox/Fate_Trigger_DisableInput", typeof(DisableInputTrigger))
			},
			{
				468,
				new BrushData(468, "Base/Levels/Level5/Brushes/TriggerBox/Fate_Effect_Wuhui_Glow", typeof(FollowCameraEffectTrigger))
			},
			{
				469,
				new BrushData(469, "Base/Levels/Level5/Brushes/TriggerBox/Fate_TriggerDropDie", typeof(DropDieTrigger))
			},
			{
				470,
				new BrushData(470, "Base/Levels/Level5/Brushes/TriggerBox/RebirthBoxTrigger_Fate", typeof(RebirthBoxTrigger))
			},
			{
				471,
				new BrushData(471, "Base/Levels/Level5/Brushes/TriggerBox/Fate_UnpredictableDiamondAward", typeof(PathToMoveByUnpredictableDiamondAwardTrigger))
			},
			{
				472,
				new BrushData(472, "Base/Levels/Level5/Brushes/TriggerBox/Fate_UnpredictableCrownAward", typeof(PathToMoveByUnpredictableCrownAwardTrigger))
			},
			{
				473,
				new BrushData(473, "Base/Levels/Level5/Brushes/TriggerBox/Fate_ActiveDiamond", typeof(ActiveDiamond))
			},
			{
				474,
				new BrushData(474, "Base/Levels/Level5/Brushes/TriggerBox/Fate_CamAN_11", typeof(CameraAnimTrigger))
			},
			{
				475,
				new BrushData(475, "Base/Levels/Level5/Brushes/TriggerBox/Fate_TriggerDropDiePro", typeof(DropDieTriggerPro))
			},
			{
				501,
				new BrushData(501, "Base/Levels/LightMap/Brush/TriggerBox/AiJi_UnpredictableDiamondAward", typeof(PathToMoveByUnpredictableDiamondAwardTrigger))
			},
			{
				502,
				new BrushData(502, "Brush/TriggerBox/CurvedBendBoxTrigger", typeof(CurvedBendBoxTrigger))
			},
			{
				503,
				new BrushData(503, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_WinBeforeFinish", typeof(WinBeforeFinishTrigger))
			},
			{
				504,
				new BrushData(504, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_Wind", typeof(WindOpenTrigger))
			},
			{
				505,
				new BrushData(505, "Base/Levels/LightMap/Brush/TriggerBox/WorldThemesTrigger", typeof(WorldThemesTrigger))
			},
			{
				506,
				new BrushData(506, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_Wind", typeof(WindCloseTrigger))
			},
			{
				507,
				new BrushData(507, "Base/Levels/LightMap/Brush/TriggerBox/AiJi_TriggerDropDie", typeof(DropDieTrigger))
			},
			{
				508,
				new BrushData(508, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_ShakeCamera", typeof(ShakeCameraTrigger))
			},
			{
				510,
				new BrushData(510, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao1_1", typeof(CameraAnimTrigger))
			},
			{
				511,
				new BrushData(511, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao1_2", typeof(CameraAnimTrigger))
			},
			{
				512,
				new BrushData(512, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao1_3", typeof(CameraAnimTrigger))
			},
			{
				513,
				new BrushData(513, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao1_4", typeof(CameraAnimTrigger))
			},
			{
				514,
				new BrushData(514, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao2_1", typeof(CameraAnimTrigger))
			},
			{
				515,
				new BrushData(515, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao2_2", typeof(CameraAnimTrigger))
			},
			{
				516,
				new BrushData(516, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao2_3", typeof(CameraAnimTrigger))
			},
			{
				517,
				new BrushData(517, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao3_1", typeof(CameraAnimTrigger))
			},
			{
				518,
				new BrushData(518, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao3_2", typeof(CameraAnimTrigger))
			},
			{
				519,
				new BrushData(519, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao4_1", typeof(CameraAnimTrigger))
			},
			{
				520,
				new BrushData(520, "Base/Levels/LightMap/Brush/TriggerBox/AiJi_ShenDian_MagicCube_Left", typeof(MagicBoxTrigger))
			},
			{
				521,
				new BrushData(521, "Base/Levels/LightMap/Brush/TriggerBox/AiJi_ShenDian_MagicCube_Right", typeof(MagicBoxTrigger))
			},
			{
				522,
				new BrushData(522, "Base/Levels/LightMap/Brush/TriggerBox/AiJi_ShenDian_MagicCube_Cycle", typeof(MagicBoxTrigger))
			},
			{
				523,
				new BrushData(523, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_RoleAnimatior", typeof(RoleAnimatiorTrigger))
			},
			{
				524,
				new BrushData(524, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_PathToMove_Pet", typeof(PathToMoveByRoleTrigger))
			},
			{
				525,
				new BrushData(525, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_Skateboard", typeof(NormalSkateboardVehicle))
			},
			{
				526,
				new BrushData(526, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_XiaPo1", typeof(CameraAnimTrigger))
			},
			{
				527,
				new BrushData(527, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_XiaPo2", typeof(CameraAnimTrigger))
			},
			{
				528,
				new BrushData(528, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_XiaPo3", typeof(CameraAnimTrigger))
			},
			{
				529,
				new BrushData(529, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_XiaPo4", typeof(CameraAnimTrigger))
			},
			{
				530,
				new BrushData(530, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_XiaPo5", typeof(CameraAnimTrigger))
			},
			{
				531,
				new BrushData(531, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_Jump01", typeof(CameraAnimTrigger))
			},
			{
				532,
				new BrushData(532, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_Jump02", typeof(CameraAnimTrigger))
			},
			{
				533,
				new BrushData(533, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_shendian_up", typeof(CameraAnimTrigger))
			},
			{
				534,
				new BrushData(534, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_shendian_down", typeof(CameraAnimTrigger))
			},
			{
				535,
				new BrushData(535, "Base/Levels/LightMap/Brush/TriggerBox/AiJi_BackGroundTrigger", typeof(BackGroundTrigger))
			},
			{
				536,
				new BrushData(536, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao5_1", typeof(CameraAnimTrigger))
			},
			{
				537,
				new BrushData(537, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao5_2", typeof(CameraAnimTrigger))
			},
			{
				538,
				new BrushData(538, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao5_3", typeof(CameraAnimTrigger))
			},
			{
				539,
				new BrushData(539, "Base/Levels/LightMap/Brush/TriggerBox/Effect_ShiNei_ENDJump", typeof(AnimEffectTrigger))
			},
			{
				540,
				new BrushData(540, "Base/Levels/LightMap/Brush/TriggerBox/AiJi_LM_CamAN_End01", typeof(CameraAnimTrigger))
			},
			{
				541,
				new BrushData(541, "Base/Levels/LightMap/Brush/TriggerBox/AiJi_LM_CamAN_End02", typeof(CameraAnimTrigger))
			},
			{
				542,
				new BrushData(542, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_Start1", typeof(CameraAnimTrigger))
			},
			{
				543,
				new BrushData(543, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_Start2", typeof(CameraAnimTrigger))
			},
			{
				544,
				new BrushData(544, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_Start3", typeof(CameraAnimTrigger))
			},
			{
				545,
				new BrushData(545, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_Start4", typeof(CameraAnimTrigger))
			},
			{
				546,
				new BrushData(546, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_Start5", typeof(CameraAnimTrigger))
			},
			{
				547,
				new BrushData(547, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_End1", typeof(CameraAnimTrigger))
			},
			{
				548,
				new BrushData(548, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_End2", typeof(CameraAnimTrigger))
			},
			{
				549,
				new BrushData(549, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_End3", typeof(CameraAnimTrigger))
			},
			{
				550,
				new BrushData(550, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_End4", typeof(CameraAnimTrigger))
			},
			{
				551,
				new BrushData(551, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_End5", typeof(CameraAnimTrigger))
			},
			{
				552,
				new BrushData(552, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_End6", typeof(CameraAnimTrigger))
			},
			{
				553,
				new BrushData(553, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_ChangeRole", typeof(ChangeRoleTrigger))
			},
			{
				554,
				new BrushData(554, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_HideBack", typeof(HideBackTrigger))
			},
			{
				555,
				new BrushData(555, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_Ship_Separate_lv4", typeof(DepartVehicleTrigger))
			},
			{
				556,
				new BrushData(556, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_DisableInput_lv4", typeof(EnableInputTrigger))
			},
			{
				557,
				new BrushData(557, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_EnableInput_lv4", typeof(DisableInputTrigger))
			},
			{
				558,
				new BrushData(558, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_Ship_Separate_lv4", typeof(StopPathToMoveTrigger))
			},
			{
				559,
				new BrushData(559, "Base/Levels/LightMap/Brush/TriggerBox/RebirthBoxTrigger", typeof(RebirthBoxTrigger))
			},
			{
				560,
				new BrushData(560, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_PathToMove_Pet", typeof(PathToMoveFixedByRoleTrigger))
			},
			{
				561,
				new BrushData(561, "Base/Levels/Level1/Brushes/TriggerBox/Trigger_PathToMove_FixedByRoleAsFragement", typeof(PathToMoveFixedByRoleAsFragementTrigger))
			},
			{
				562,
				new BrushData(562, "Base/Levels/Level1/Brushes/TriggerBox/TriggerFragmentTrigger", typeof(TriggerFragmentTrigger))
			},
			{
				563,
				new BrushData(563, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_PathToMove_Pet", typeof(PathToMoveByRoleAsFragementTrigger))
			},
			{
				564,
				new BrushData(564, "Base/Levels/Level2/Brushes/TriggerBox/AiJi_CamAN_QiChuan04", typeof(CameraAnimAsFragementTrigger))
			},
			{
				565,
				new BrushData(565, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_Ship_Separate_lv4", typeof(StopPathToMoveAsFragementTrigger))
			},
			{
				566,
				new BrushData(566, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_Ship_Separate", typeof(DepartVehicleAsFragementTrigger))
			},
			{
				567,
				new BrushData(567, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao5_4", typeof(CameraAnimTrigger))
			},
			{
				568,
				new BrushData(568, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao5_5", typeof(CameraAnimTrigger))
			},
			{
				569,
				new BrushData(569, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_RoleAnimatior", typeof(RoleAnimatiorAsFragementTrigger))
			},
			{
				570,
				new BrushData(570, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao1_new1", typeof(CameraAnimTrigger))
			},
			{
				571,
				new BrushData(571, "Base/Levels/LightMap/Brush/TriggerBox/LiuSha_CamAN_DuoDUanTiao1_new2", typeof(CameraAnimTrigger))
			},
			{
				572,
				new BrushData(572, "Base/Levels/LightMap/Brush/TriggerBox/RebirthBoxTrigger_Pharaohs", typeof(RebirthBoxTrigger))
			},
			{
				573,
				new BrushData(573, "Base/Levels/LightMap/Brush/TriggerBox/Shamo_Trigger_DropDieForword", typeof(DropDieForwordTrigger))
			},
			{
				574,
				new BrushData(574, "Base/Levels/LightMap/Brush/TriggerBox/Shamo_Trigger_DropDieStatic", typeof(DropDieStaticTrigger))
			},
			{
				575,
				new BrushData(575, "Base/Levels/LightMap/Brush/TriggerBox/Shamo_RoleMoveLimitTrigger", typeof(RoleMoveLimitTrigger))
			},
			{
				601,
				new BrushData(601, "Base/Levels/Level3/Brushes/TriggerBox/WhaleAutoMoveTrigger", typeof(WhaleAutoMoveTrigger))
			},
			{
				602,
				new BrushData(602, "Base/Levels/Level3/Brushes/TriggerBox/WhaleVehicle", typeof(WhaleVehicle))
			},
			{
				603,
				new BrushData(603, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_InputType", typeof(InputTypeTrigger))
			},
			{
				604,
				new BrushData(604, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_TengMan_A", typeof(CreeperTrigger))
			},
			{
				605,
				new BrushData(605, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_ShowBack", typeof(ShowBackgroundTrigger))
			},
			{
				606,
				new BrushData(606, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_CameraParticlePlay", typeof(ChangeCameraEffectTrigger))
			},
			{
				607,
				new BrushData(607, "Base/Levels/Level3/Brushes/TriggerBox/Trigger_CameraParticlePlayByName", typeof(ChangeCameraEffectByNameTrigger))
			},
			{
				701,
				new BrushData(701, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_01", typeof(CameraAnimTrigger))
			},
			{
				702,
				new BrushData(702, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_02", typeof(CameraAnimTrigger))
			},
			{
				703,
				new BrushData(703, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_03", typeof(CameraAnimTrigger))
			},
			{
				704,
				new BrushData(704, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_04", typeof(CameraAnimTrigger))
			},
			{
				705,
				new BrushData(705, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_05", typeof(CameraAnimTrigger))
			},
			{
				706,
				new BrushData(706, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_06", typeof(CameraAnimTrigger))
			},
			{
				707,
				new BrushData(707, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CameraTransferTrigger", typeof(CameraTransferTrigger))
			},
			{
				708,
				new BrushData(708, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CameraBlankTrigger", typeof(CameraBlankTrigger))
			},
			{
				709,
				new BrushData(709, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Trigger_Skateboard", typeof(NormalSkateboardVehicle))
			},
			{
				710,
				new BrushData(710, "Base/Levels/Waltz/Brushes/TriggerBox/UnpredictableDiamondAward", typeof(PathToMoveByUnpredictableDiamondAwardTrigger))
			},
			{
				711,
				new BrushData(711, "Base/Levels/Waltz/Brushes/TriggerBox/UnpredictableCrownAward", typeof(PathToMoveByUnpredictableCrownAwardTrigger))
			},
			{
				712,
				new BrushData(712, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_TriggerDropDie", typeof(DropDieTrigger))
			},
			{
				713,
				new BrushData(713, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_RoleMoveLimitTrigger", typeof(RoleMoveLimitTrigger))
			},
			{
				714,
				new BrushData(714, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_violintou01", typeof(CameraAnimTrigger))
			},
			{
				715,
				new BrushData(715, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_violintou02", typeof(CameraAnimTrigger))
			},
			{
				716,
				new BrushData(716, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_rolechange", typeof(CameraAnimTrigger))
			},
			{
				717,
				new BrushData(717, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_MusicBox", typeof(CameraAnimTrigger))
			},
			{
				718,
				new BrushData(718, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_End_Violin_05", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				719,
				new BrushData(719, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_End_Violin_06", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				720,
				new BrushData(720, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_End_Violin_07", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				721,
				new BrushData(721, "Base/Levels/Waltz/Brushes/TriggerBox/RelativeDisplacementMotionTriggerBox", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				722,
				new BrushData(722, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_TriggerDropDie_Capsuse", typeof(DiePlatformTrigger))
			},
			{
				724,
				new BrushData(724, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Midground_MusicBox", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				726,
				new BrushData(726, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Midground_Effect_YP", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				730,
				new BrushData(730, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Midground_Projector", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				731,
				new BrushData(731, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CameraLookAtSpeedTrigger", typeof(CameraLookAtSpeedTrigger))
			},
			{
				732,
				new BrushData(732, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CurvedBendBoxTrigger", typeof(CurvedBendBoxTrigger))
			},
			{
				734,
				new BrushData(734, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Trigger_Wind", typeof(CloseFollowTrigger))
			},
			{
				741,
				new BrushData(741, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_PathToMoveByCameraTrigger", typeof(PathToMoveByCameraTrigger))
			},
			{
				751,
				new BrushData(751, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_ChangeRuntimeAnimatorControllerTrigger", typeof(ChangeRuntimeAnimatorControllerTrigger))
			},
			{
				752,
				new BrushData(752, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_ChangeRoleSkinTrigger", typeof(ChangeRoleSkinTrigger))
			},
			{
				761,
				new BrushData(761, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_Cha", typeof(CameraAnimTrigger))
			},
			{
				762,
				new BrushData(762, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_CamAN_End", typeof(CameraAnimTrigger))
			},
			{
				770,
				new BrushData(770, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Midground_ViolinDance", typeof(ViolinDanceTrigger))
			},
			{
				771,
				new BrushData(771, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Midground_Huati", typeof(ViolinDanceTrigger))
			},
			{
				791,
				new BrushData(791, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_WorldThemesTrigger", typeof(WorldThemesTrigger))
			},
			{
				792,
				new BrushData(792, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Trigger_RoleAnimatior", typeof(RoleAnimatiorTrigger))
			},
			{
				793,
				new BrushData(793, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_ChangeEffect_zhihuibang", typeof(AnimEffectTrigger))
			},
			{
				794,
				new BrushData(794, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Trigger_Skateboard", typeof(NormalSkateboardVehicle))
			},
			{
				795,
				new BrushData(795, "Base/Levels/Waltz/Brushes/TriggerBox/ActiveDiamond", typeof(ActiveDiamond))
			},
			{
				796,
				new BrushData(796, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Trigger_Jiguang", typeof(WaltzStringLaser))
			},
			{
				797,
				new BrushData(797, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Trigger_Input_Param", typeof(InputResetTrigger))
			},
			{
				798,
				new BrushData(798, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Trigger_CameraParticlePlayByName", typeof(ChangeCameraEffectByNameTrigger))
			},
			{
				799,
				new BrushData(799, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Trigger_DropDieForword", typeof(DropDieForwordTrigger))
			},
			{
				800,
				new BrushData(800, "Base/Levels/Waltz/Brushes/TriggerBox/Waltz_Trigger_DropDieStatic", typeof(DropDieStaticTrigger))
			},
			{
				901,
				new BrushData(901, "Base/Levels/Theif/Brushes/TriggerBox/ThiefMovePathTrigger", typeof(ThiefMovePathTrigger))
			},
			{
				902,
				new BrushData(902, "Base/Levels/Theif/Brushes/TriggerBox/Trigger_CoupleAnimatior", typeof(CoupleAnimatorTrigger))
			},
			{
				903,
				new BrushData(903, "Base/Levels/Theif/Brushes/TriggerBox/CoupleMirrorDancer", typeof(CoupleMirrorDancer))
			},
			{
				904,
				new BrushData(904, "Base/Levels/Theif/Brushes/TriggerBox/DanceTogetherTrigger", typeof(DanceTogetherTrigger))
			},
			{
				911,
				new BrushData(911, "Base/Levels/Theif/Brushes/TriggerBox/Thief_SwingingRope", typeof(SwingingRopeTriggerBox))
			},
			{
				912,
				new BrushData(912, "Base/Levels/Theif/Brushes/TriggerBox/CarriageRiderTrigger", typeof(CarriageRider))
			},
			{
				913,
				new BrushData(913, "Base/Levels/Theif/Brushes/TriggerBox/Thief_SwingingRop_Chang", typeof(SwingingRopeTriggerBox))
			},
			{
				921,
				new BrushData(921, "Base/Levels/Theif/Brushes/TriggerBox/Theif_CameraMaskTrigger", typeof(CameraMaskTrigger))
			},
			{
				922,
				new BrushData(922, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_KaiChang_HuaShengZi", typeof(CameraAnimTrigger))
			},
			{
				923,
				new BrushData(923, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_KaiChang_HuaShengZi_END", typeof(CameraAnimTrigger))
			},
			{
				924,
				new BrushData(924, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_HuoChang_DangSheng01", typeof(CameraAnimTrigger))
			},
			{
				925,
				new BrushData(925, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_HuoChang_DangSheng02", typeof(CameraAnimTrigger))
			},
			{
				926,
				new BrushData(926, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_HuoChang_DangSheng03", typeof(CameraAnimTrigger))
			},
			{
				927,
				new BrushData(927, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_ChengMen", typeof(CameraAnimTrigger))
			},
			{
				928,
				new BrushData(928, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_ChengMen_END", typeof(CameraAnimTrigger))
			},
			{
				929,
				new BrushData(929, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_HeTi", typeof(CameraAnimTrigger))
			},
			{
				930,
				new BrushData(930, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_HeTi_END", typeof(CameraAnimTrigger))
			},
			{
				931,
				new BrushData(931, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_HeTi_01", typeof(CameraAnimTrigger))
			},
			{
				932,
				new BrushData(932, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_HeTi_02", typeof(CameraAnimTrigger))
			},
			{
				934,
				new BrushData(934, "Base/Levels/Theif/Brushes/TriggerBox/CoupleFollowTrigger", typeof(CoupleFollowTrigger))
			},
			{
				935,
				new BrushData(935, "Base/Levels/Theif/Brushes/TriggerBox/CoupleDetachTrigger", typeof(CoupleDetachTrigger))
			},
			{
				936,
				new BrushData(936, "Base/Levels/Theif/Brushes/TriggerBox/Thief_SwingingRop_Chang10", typeof(SwingingRopeTriggerBox))
			},
			{
				937,
				new BrushData(937, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_END", typeof(CameraAnimTrigger))
			},
			{
				938,
				new BrushData(938, "Base/Levels/Theif/Brushes/TriggerBox/DaiZei_CamAN_RuShiNei", typeof(CameraAnimTrigger))
			},
			{
				939,
				new BrushData(939, "Base/Levels/Theif/Brushes/TriggerBox/ThiefEndingTrigger", typeof(ThiefEndingTrigger))
			},
			{
				940,
				new BrushData(940, "Prefab/Couples/EndKissThiefTrigger", typeof(EndKissCouple))
			},
			{
				941,
				new BrushData(941, "Base/Levels/Theif/Brushes/TriggerBox/ThiefEndChestTrigger", typeof(EndKissChest))
			},
			{
				942,
				new BrushData(942, "Base/Levels/Theif/Brushes/TriggerBox/DaoZei_CamAN_END00", typeof(CameraAnimTrigger))
			},
			{
				943,
				new BrushData(943, "Base/Levels/Theif/Brushes/TriggerBox/Thief_RoleMoveLimitTrigger", typeof(RoleMoveLimitTrigger))
			},
			{
				944,
				new BrushData(944, "Base/Levels/Theif/Brushes/TriggerBox/GameWinTrigger", typeof(GameWinTrigger))
			},
			{
				945,
				new BrushData(945, "Base/Levels/Theif_2/Brushes/TriggerBox/Thief2_SwitchSendTrigger", typeof(SwitchSendTrigger))
			},
			{
				946,
				new BrushData(946, "Base/Levels/Theif_2/Brushes/TriggerBox/Thief2_SwitchListenTrigger", typeof(SwitchListenTrigger))
			},
			{
				998,
				new BrushData(998, "Brush/TriggerBox/GuideUiTrigger", typeof(GuideUiTrigger))
			},
			{
				1000,
				new BrushData(1000, "Base/Levels/Tutorial/Brushes/TriggerBox/Tutorial_anim01", typeof(CameraAnimTrigger))
			},
			{
				1001,
				new BrushData(1001, "Base/Levels/Tutorial/Brushes/TriggerBox/Tutorial_anim02", typeof(CameraAnimTrigger))
			},
			{
				1011,
				new BrushData(1011, "Base/Levels/Tutorial/Brushes/TriggerBox/Tutorial_CamAn_anim01", typeof(CameraAnimTrigger))
			},
			{
				1012,
				new BrushData(1012, "Base/Levels/Tutorial/Brushes/TriggerBox/Tutorial_CamAn_anim02", typeof(CameraAnimTrigger))
			},
			{
				1013,
				new BrushData(1013, "Base/Levels/Tutorial/Brushes/TriggerBox/Tutorial_CamAn_anim03", typeof(CameraAnimTrigger))
			},
			{
				999,
				new BrushData(999, "Base/Levels/LightMap/Brush/TriggerBox/BuyOutRebirthBoxTrigger", typeof(BuyOutRebirthBoxTrigger))
			},
			{
				1002,
				new BrushData(1002, "Base/Levels/Tutorial/Brushes/TriggerBox/BackMusicFadeOutTrigger", typeof(BackMusicFadeOutTrigger))
			},
			{
				1003,
				new BrushData(1003, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_PathToMove_Pet", typeof(PathToMoveByRoleForLerpTrigger))
			},
			{
				1101,
				new BrushData(1101, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_fushi", typeof(CameraAnimTrigger))
			},
			{
				1102,
				new BrushData(1102, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_fushi_huifu", typeof(CameraAnimTrigger))
			},
			{
				1103,
				new BrushData(1103, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_pingshi", typeof(CameraAnimTrigger))
			},
			{
				1104,
				new BrushData(1104, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_pingshi_huifu", typeof(CameraAnimTrigger))
			},
			{
				1105,
				new BrushData(1105, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_layuan", typeof(CameraAnimTrigger))
			},
			{
				1106,
				new BrushData(1106, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_layuan_huifu", typeof(CameraAnimTrigger))
			},
			{
				1107,
				new BrushData(1107, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_MusicWriter", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				1108,
				new BrushData(1108, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_douya02", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				1109,
				new BrushData(1109, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CurvedBendBoxTrigger", typeof(CurvedBendBoxTrigger))
			},
			{
				1110,
				new BrushData(1110, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_WorldThemesTrigger", typeof(WorldThemesTrigger))
			},
			{
				1111,
				new BrushData(1111, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_layuan01", typeof(CameraAnimTrigger))
			},
			{
				1112,
				new BrushData(1112, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_layuan02", typeof(CameraAnimTrigger))
			},
			{
				1113,
				new BrushData(1113, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_layuan03", typeof(CameraAnimTrigger))
			},
			{
				1114,
				new BrushData(1114, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_layuan04", typeof(CameraAnimTrigger))
			},
			{
				1115,
				new BrushData(1115, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_layuan05", typeof(CameraAnimTrigger))
			},
			{
				1116,
				new BrushData(1116, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Trigger_PathToMove", typeof(PathToMoveByRoleTrigger))
			},
			{
				1117,
				new BrushData(1117, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Trigger_Skateboard", typeof(NormalSkateboardVehicle))
			},
			{
				1118,
				new BrushData(1118, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Trigger_Skateboard_Separate", typeof(DepartVehicleTrigger))
			},
			{
				1119,
				new BrushData(1119, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Trigger_Skateboard_yuepu", typeof(NormalSkateboardVehicle))
			},
			{
				1120,
				new BrushData(1120, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_End", typeof(CameraAnimTrigger))
			},
			{
				1121,
				new BrushData(1121, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_RoleMoveLimitTrigger", typeof(RoleMoveLimitTrigger))
			},
			{
				1131,
				new BrushData(1131, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_Taijie01", typeof(CameraAnimTrigger))
			},
			{
				1132,
				new BrushData(1132, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_CamAN_Taijie01_huifu", typeof(CameraAnimTrigger))
			},
			{
				1141,
				new BrushData(1141, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Mofang_Danhuangguan01", typeof(LeftRightRotateListenTrigger))
			},
			{
				1142,
				new BrushData(1142, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Mofang_Danhuangguan02", typeof(LeftRightRotateListenTrigger))
			},
			{
				1143,
				new BrushData(1143, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Mofang_Danhuangguan03", typeof(LeftRightRotateListenTrigger))
			},
			{
				1144,
				new BrushData(1144, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Mofang_Danhuangguan04", typeof(LeftRightRotateListenTrigger))
			},
			{
				1145,
				new BrushData(1145, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Mofang_Danhuangguan05", typeof(LeftRightRotateListenTrigger))
			},
			{
				1146,
				new BrushData(1146, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Mofang_Danhuangguan06", typeof(LeftRightRotateListenTrigger))
			},
			{
				1151,
				new BrushData(1151, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_LeftRightRotateSendTrigger", typeof(LeftRightRotateSendTrigger))
			},
			{
				1161,
				new BrushData(1161, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_Mofang_01_a", typeof(LeftRightRotateListenTrigger))
			},
			{
				1162,
				new BrushData(1162, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_Mofang_01_b", typeof(LeftRightRotateListenTrigger))
			},
			{
				1163,
				new BrushData(1163, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_Mofang_02_b", typeof(LeftRightRotateListenTrigger))
			},
			{
				1164,
				new BrushData(1164, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_Mofang_02_c", typeof(LeftRightRotateListenTrigger))
			},
			{
				1165,
				new BrushData(1165, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_Mofang_03_a", typeof(LeftRightRotateListenTrigger))
			},
			{
				1166,
				new BrushData(1166, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_Mofang_03_b", typeof(LeftRightRotateListenTrigger))
			},
			{
				1167,
				new BrushData(1167, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_Mofang_03_c", typeof(LeftRightRotateListenTrigger))
			},
			{
				1168,
				new BrushData(1168, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Midground_Mofang_03_d", typeof(LeftRightRotateListenTrigger))
			},
			{
				1169,
				new BrushData(1169, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_OpenOff_Door_S", typeof(SwitchListenTrigger))
			},
			{
				1170,
				new BrushData(1170, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_OpenOffSendTrigger", typeof(SwitchSendTrigger))
			},
			{
				1171,
				new BrushData(1171, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_OpenOff_Door", typeof(SwitchListenTrigger))
			},
			{
				1172,
				new BrushData(1172, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Effect_jiantou01", typeof(SwitchListenTrigger))
			},
			{
				1173,
				new BrushData(1173, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_OpenOff_Door_S", typeof(SwitchListenTrigger))
			},
			{
				1181,
				new BrushData(1181, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Feiji_SendTrigger", typeof(PathGuideSendTrigger))
			},
			{
				1182,
				new BrushData(1182, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Feiji_ListenTrigger", typeof(PathGuideListenTrigger))
			},
			{
				1191,
				new BrushData(1191, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_Trigger_Skateboard_yuepuFly", typeof(NormalPathVehicle))
			},
			{
				1198,
				new BrushData(1198, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_UnpredictableDiamondAward", typeof(PathToMoveByUnpredictableDiamondAwardTrigger))
			},
			{
				1199,
				new BrushData(1199, "Base/Levels/Fantasia_Jazz/Brushes/TriggerBox/Jazz_UnpredictableCrownAward", typeof(PathToMoveByUnpredictableCrownAwardTrigger))
			},
			{
				1201,
				new BrushData(1201, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Trigger_Skateboard_Rainbow", typeof(SkateboardVehicleCanDepart))
			},
			{
				1202,
				new BrushData(1202, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Home_Rainbow_CamAN_ShanDong", typeof(CameraAnimTrigger))
			},
			{
				1203,
				new BrushData(1203, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Home_Rainbow_CamAN_ShanDongChuKou01", typeof(CameraAnimTrigger))
			},
			{
				1204,
				new BrushData(1204, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Home_Rainbow_CamAN_XueDi_End", typeof(CameraAnimTrigger))
			},
			{
				1205,
				new BrushData(1205, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Home_Rainbow_CamAN_YunWu_End", typeof(CameraAnimTrigger))
			},
			{
				1206,
				new BrushData(1206, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/YiJi_01", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				1207,
				new BrushData(1207, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/YiJi_02", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				1208,
				new BrushData(1208, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/YiJi_03", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				1209,
				new BrushData(1209, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Home_Rainbow_CamAN_END", typeof(CameraAnimTrigger))
			},
			{
				1210,
				new BrushData(1210, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/RandomAnimTrigger", typeof(RandomAnimTrigger))
			},
			{
				1211,
				new BrushData(1211, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/YiJi_04", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				1212,
				new BrushData(1212, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Rainbow_CameraBlankTrigger", typeof(CameraBlankTrigger))
			},
			{
				1213,
				new BrushData(1213, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Rainbow_Effect_FuWen", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				1215,
				new BrushData(1215, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/RandomTileTrigger", typeof(RandomTileTrigger))
			},
			{
				1216,
				new BrushData(1216, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Home_Rainbow_CamAN_FuWen_END01", typeof(CameraAnimTrigger))
			},
			{
				1217,
				new BrushData(1217, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Trigger_Skateboard_Rainbow_Sha", typeof(NormalSkateboardVehicle))
			},
			{
				1218,
				new BrushData(1218, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Trigger_Skateboard_YangTuo", typeof(NormalSkateboardVehicle))
			},
			{
				1219,
				new BrushData(1219, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Rainbow_DiamondRun_TriggerBox", typeof(PathToMoveByDiamondAwardSendTrigger))
			},
			{
				1220,
				new BrushData(1220, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Rainbow_UnpredictableDiamondAward", typeof(PathToMoveByDiamondAwardListenTrigger))
			},
			{
				1221,
				new BrushData(1221, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Rainbow_CrownRun_TriggerBox", typeof(PathToMoveByCrownAwardSendTrigger))
			},
			{
				1222,
				new BrushData(1222, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Rainbow_UnpredictableCrownAward", typeof(PathToMoveByCrownAwardListenTrigger))
			},
			{
				1223,
				new BrushData(1223, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Home_Rainbow_CamAN_ShanDongFuWen", typeof(CameraAnimTrigger))
			},
			{
				1225,
				new BrushData(1225, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Trigger_Skateboard_YangTuo", typeof(AlpacaVehicle))
			},
			{
				1226,
				new BrushData(1226, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/Home_Rainbow_CamAN_End_Back", typeof(CameraAnimTrigger))
			},
			{
				1227,
				new BrushData(1227, "Base/Levels/Home_Rainbow/Brushes/TriggerBox/RainBow_CameraBlankTrigger001", typeof(CameraBlankTrigger))
			},
			{
				1300,
				new BrushData(1300, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_AutoMoveJumpTrigger4", typeof(AutoMoveJumpTrigger))
			},
			{
				1301,
				new BrushData(1301, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_AutoMoveJumpTrigger3", typeof(AutoMoveJumpTrigger))
			},
			{
				1302,
				new BrushData(1302, "Base/Levels/WeirdDream/Brushes/TriggerBox/Back_WeirdDream_Sky", typeof(RelativeDisplacementMotionTriggerBox))
			},
			{
				1303,
				new BrushData(1303, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_ElevatorDownTrigger", typeof(AnimEffectTrigger))
			},
			{
				1304,
				new BrushData(1304, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_ElevatorUpTrigger", typeof(AnimEffectTrigger))
			},
			{
				1305,
				new BrushData(1305, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_AutoMoveJumpTrigger", typeof(AnimEffectTrigger))
			},
			{
				1306,
				new BrushData(1306, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_JuLi01", typeof(CameraAnimTrigger))
			},
			{
				1307,
				new BrushData(1307, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_AutoMoveJumpTrigger_02", typeof(AnimEffectTrigger))
			},
			{
				1308,
				new BrushData(1308, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_AutoMoveJumpTrigger_03", typeof(AnimEffectTrigger))
			},
			{
				1309,
				new BrushData(1309, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_Trigger_EnableInput", typeof(EnableInputTrigger))
			},
			{
				1310,
				new BrushData(1310, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_Trigger_DisableInput", typeof(DisableInputTrigger))
			},
			{
				1311,
				new BrushData(1311, "Base/Levels/Level1/Brushes/TriggerBox/Effect_AutoMoveJumpTrigger_02", typeof(AnimEffectTrigger))
			},
			{
				1312,
				new BrushData(1312, "Base/Levels/Level1/Brushes/TriggerBox/Effect_AutoMoveJumpTrigger_03", typeof(AnimEffectTrigger))
			},
			{
				1313,
				new BrushData(1313, "Base/Levels/Level1/Brushes/TriggerBox/Effect_AutoMoveJumpTrigger_04", typeof(AnimEffectTrigger))
			},
			{
				1314,
				new BrushData(1314, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_AutoMoveJumpTrigger_04", typeof(AnimEffectTrigger))
			},
			{
				1315,
				new BrushData(1315, "Base/Levels/WeirdDream/Brushes/TriggerBox/WeirdDream_AutoMoveJumpTrigger_01", typeof(AnimEffectTrigger))
			},
			{
				1400,
				new BrushData(1400, "Base/Levels/LightMap/Brush/TriggerBox/Pharaohs_HN_Camxiapo_1", typeof(CameraAnimTrigger))
			},
			{
				1401,
				new BrushData(1401, "Base/Levels/LightMap/Brush/TriggerBox/Trigger_HideBack", typeof(ChangeBackgroundTirgger))
			},
			{
				1402,
				new BrushData(1402, "Base/Levels/LightMap/Brush/TriggerBox/Pharaohs_HN_Camjiewei_1", typeof(CameraAnimTrigger))
			},
			{
				1403,
				new BrushData(1403, "Base/Levels/LightMap/Brush/TriggerBox/CurvedBendBoxTrigger", typeof(CurvedBendBoxTrigger))
			},
			{
				1404,
				new BrushData(1404, "Base/Levels/LightMap/Brush/TriggerBox/AiJi_Jiewei_01", typeof(ViolinDanceTrigger))
			},
			{
				1405,
				new BrushData(1405, "Base/Levels/Level2/Brushes/TriggerBox/Trigger_Skateboard_HN", typeof(NormalSkateboardVehicle))
			},
			{
				1406,
				new BrushData(1406, "Base/Levels/LightMap/Brush/TriggerBox/Pharaohs_HN_Camdatiao_1", typeof(CameraAnimTrigger))
			},
			{
				1500,
				new BrushData(1500, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_CurvedBendBoxTrigger", typeof(CurvedBendBoxTrigger))
			},
			{
				1501,
				new BrushData(1501, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_Wind", typeof(OpenFollowTrigger))
			},
			{
				1502,
				new BrushData(1502, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_WorldThemesTrigger", typeof(WorldThemesTrigger))
			},
			{
				1503,
				new BrushData(1503, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_CameraParticlePlayByName", typeof(ChangeCameraEffectByNameTrigger))
			},
			{
				1504,
				new BrushData(1504, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_ChangeRuntimeAnimatorController", typeof(ChangeRuntimeAnimatorControllerTrigger))
			},
			{
				1505,
				new BrushData(1505, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_TriggerDropDiePro", typeof(DropDieTriggerPro))
			},
			{
				1531,
				new BrushData(1531, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_CamAN_Pingshi01", typeof(CameraAnimTrigger))
			},
			{
				1532,
				new BrushData(1532, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_CamAN_Pingshi01_Huifu", typeof(CameraAnimTrigger))
			},
			{
				1533,
				new BrushData(1533, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_CamAN_Yueliang", typeof(CameraAnimTrigger))
			},
			{
				1534,
				new BrushData(1534, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_CamAN_Yueliang_Huifu", typeof(CameraAnimTrigger))
			},
			{
				1539,
				new BrushData(1539, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_CamAN_End01", typeof(CameraAnimTrigger))
			},
			{
				1580,
				new BrushData(1580, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_CutSend_danji", typeof(CutSendTrigger))
			},
			{
				1581,
				new BrushData(1581, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_CutSend", typeof(CutSendTrigger))
			},
			{
				1582,
				new BrushData(1582, "Base/Levels/Samurai/Brushes/TriggerBox/Diamond_Fragment", typeof(CutListenForDiamondFragment))
			},
			{
				1583,
				new BrushData(1583, "Base/Levels/Samurai/Brushes/TriggerBox/Crown_Fragment", typeof(CutListenForCrownFragment))
			},
			{
				1584,
				new BrushData(1584, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_ContinuousCutSend", typeof(ContinuousCutSendTrigger))
			},
			{
				1585,
				new BrushData(1585, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_RoleOnFireTrigger", typeof(RoleOnFireTrigger))
			},
			{
				1586,
				new BrushData(1586, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_RoleOutFireTrigger", typeof(RoleOutFireTrigger))
			},
			{
				1587,
				new BrushData(1587, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_Skateboard_yinghuaFly", typeof(NormalPathVehicle))
			},
			{
				1599,
				new BrushData(1599, "Base/Levels/Samurai/Brushes/TriggerBox/Samurai_Trigger_WinBeforeFinish", typeof(WinBeforeFinishTrigger))
			},
			{
				1600,
				new BrushData(1600, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_MuBan", typeof(CameraAnimTrigger))
			},
			{
				1601,
				new BrushData(1601, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_MuBan_2", typeof(CameraAnimTrigger))
			},
			{
				1602,
				new BrushData(1602, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_TiaoChuang", typeof(CameraAnimTrigger))
			},
			{
				1603,
				new BrushData(1603, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_GuoChang01", typeof(CameraAnimTrigger))
			},
			{
				1604,
				new BrushData(1604, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_GuoChangJieShu01", typeof(CameraAnimTrigger))
			},
			{
				1605,
				new BrushData(1605, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_GuoChang02", typeof(CameraAnimTrigger))
			},
			{
				1606,
				new BrushData(1606, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_WuDingHuaSheng_01", typeof(CameraAnimTrigger))
			},
			{
				1607,
				new BrushData(1607, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_GuoChang03", typeof(CameraAnimTrigger))
			},
			{
				1608,
				new BrushData(1608, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_GuoChangJieShu03", typeof(CameraAnimTrigger))
			},
			{
				1609,
				new BrushData(1609, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_GuoChangJieShu03_2", typeof(CameraAnimTrigger))
			},
			{
				1610,
				new BrushData(1610, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CamAN_Start02", typeof(CameraAnimTrigger))
			},
			{
				1611,
				new BrushData(1611, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_GuoChang01_1", typeof(CameraAnimTrigger))
			},
			{
				1612,
				new BrushData(1612, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_GuoChang01_2", typeof(CameraAnimTrigger))
			},
			{
				1613,
				new BrushData(1613, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_GuoChang01_3", typeof(CameraAnimTrigger))
			},
			{
				1614,
				new BrushData(1614, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_JinShiNei", typeof(CameraAnimTrigger))
			},
			{
				1615,
				new BrushData(1615, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_ChuShiNei", typeof(CameraAnimTrigger))
			},
			{
				1616,
				new BrushData(1616, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_JinGongDian", typeof(CameraAnimTrigger))
			},
			{
				1617,
				new BrushData(1617, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_CanmAN_End", typeof(CameraAnimTrigger))
			},
			{
				1618,
				new BrushData(1618, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_SwingingRope", typeof(SwingingRopeTriggerBox))
			},
			{
				1619,
				new BrushData(1619, "Base/Levels/Thief_2/Brushes/TriggerBox/Thief2_SwingingRop_Chang", typeof(SwingingRopeTriggerBox))
			},
			{
				10001,
				new BrushData(10001, "Base/Levels/Commons/Brushes/TriggerBox/Rainbow_UnpredictableDiamondAward", typeof(PathToMoveByDiamondAwardListenTrigger))
			},
			{
				10002,
				new BrushData(10002, "Base/Levels/Commons/Brushes/TriggerBox/Rainbow_UnpredictableCrownAward", typeof(PathToMoveByCrownAwardListenTrigger))
			},
			{
				10003,
				new BrushData(10003, "Base/Levels/Commons/Brushes/TriggerBox/UnpredictableDiamondAward", typeof(PathToMoveByUnpredictableDiamondAwardTrigger))
			},
			{
				10004,
				new BrushData(10004, "Base/Levels/Commons/Brushes/TriggerBox/UnpredictableCrownAward", typeof(PathToMoveByUnpredictableCrownAwardTrigger))
			},
			{
				10005,
				new BrushData(10005, "Base/Levels/Commons/Brushes/TriggerBox/Diamond_Fragment", typeof(CutListenForDiamondFragment))
			},
			{
				10006,
				new BrushData(10006, "Base/Levels/Commons/Brushes/TriggerBox/Crown_Fragment", typeof(CutListenForCrownFragment))
			},
			{
				10007,
				new BrushData(10007, "Base/Levels/Commons/Brushes/TriggerBox/ActiveDiamond", typeof(ActiveDiamond))
			},
			{
				10008,
				new BrushData(10008, "Base/Levels/Commons/Brushes/TriggerBox/Tutorial_UnpredictableDiamondAward", typeof(PathToMoveByUnpredictableDiamondAwardTrigger))
			},
			{
				10009,
				new BrushData(10009, "Base/Levels/Commons/Brushes/TriggerBox/Tutorial_UnpredictableCrownAward", typeof(PathToMoveByUnpredictableCrownAwardTrigger))
			}
		};

		public static Dictionary<int, BrushData> GetBrushDatasByType(TileObjectType tileType)
		{
			Dictionary<int, BrushData> result = null;
			switch (tileType)
			{
			case TileObjectType.Tile:
				result = m_tileBrushs;
				break;
			case TileObjectType.Enemy:
				result = m_enemyBrushs;
				break;
			case TileObjectType.Midground:
				result = m_midgroundBrushs;
				break;
			case TileObjectType.Trigger:
				result = m_triggerBoxBrushs;
				break;
			case TileObjectType.Group:
				result = m_groupBrushs;
				break;
			}
			return result;
		}

		public static BrushData GetBrushDataByTypeAddBrushID(TileObjectType tileType, int id)
		{
			Dictionary<int, BrushData> brushDatasByType = GetBrushDatasByType(tileType);
			if (brushDatasByType.ContainsKey(id))
			{
				return brushDatasByType[id];
			}
			return null;
		}
	}
}
