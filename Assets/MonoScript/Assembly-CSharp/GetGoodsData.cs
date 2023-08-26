using RS2;
using UnityEngine.Events;

public class GetGoodsData
{
	private int m_goodsId;

	private double m_goodsNum;

	private int m_goodsTeamNum;

	private UnityAction m_callBackFunc;

	public UnityAction<GetGoodsForm> closeCallback;

	private int m_goodsTeamId;

	private bool m_isBuy;

	private bool m_isGoodsTeam;

	private bool m_showGetPath;

	private bool m_showExpound;

	private int m_expoundTextId;

	private bool needCloseButton;

	private uint closeButtonDelayTime;

	private bool needActionButton;

	private UnityAction actionButtonCallback;

	private bool needCheckAds;

	private bool upgrade;

	public UnityAction moveEffectFinishedCallback;

	private UnityAction normalButtonCallback;

	private UnityAction<UnityAction> adButtonCallback;

	private bool m_IsAutoPlayEffect;

	public int GoodsId
	{
		get
		{
			return m_goodsId;
		}
		set
		{
			m_goodsId = value;
		}
	}

	public double GoodsNum
	{
		get
		{
			return m_goodsNum;
		}
		set
		{
			m_goodsNum = value;
		}
	}

	public int GoodsTeamNum
	{
		get
		{
			return m_goodsTeamNum;
		}
		set
		{
			m_goodsTeamNum = value;
		}
	}

	public UnityAction CallBackFunc
	{
		get
		{
			return m_callBackFunc;
		}
		set
		{
			m_callBackFunc = value;
		}
	}

	public int GoodsTeamId
	{
		get
		{
			return m_goodsTeamId;
		}
		set
		{
			m_goodsTeamId = value;
		}
	}

	public bool Buy
	{
		get
		{
			return m_isBuy;
		}
		set
		{
			m_isBuy = value;
		}
	}

	public bool GoodsTeam
	{
		get
		{
			return m_isGoodsTeam;
		}
		set
		{
			m_isGoodsTeam = value;
		}
	}

	public bool ShowGetPath
	{
		get
		{
			return m_showGetPath;
		}
		set
		{
			m_showGetPath = value;
		}
	}

	public bool ShowExpound
	{
		get
		{
			return m_showExpound;
		}
		set
		{
			m_showExpound = value;
		}
	}

	public int ExpoundTextId
	{
		get
		{
			return m_expoundTextId;
		}
		set
		{
			m_expoundTextId = value;
		}
	}

	public bool NeedCloseButton
	{
		get
		{
			return needCloseButton;
		}
		set
		{
			needCloseButton = value;
		}
	}

	public uint CloseButtonDelayTime
	{
		get
		{
			return closeButtonDelayTime;
		}
		set
		{
			closeButtonDelayTime = value;
		}
	}

	public bool NeedActionButton
	{
		get
		{
			return needActionButton;
		}
		set
		{
			needActionButton = value;
		}
	}

	public UnityAction ActionButtonCallback
	{
		get
		{
			return actionButtonCallback;
		}
		set
		{
			actionButtonCallback = value;
		}
	}

	public bool NeedCheckAds
	{
		get
		{
			return needCheckAds;
		}
		set
		{
			needCheckAds = value;
		}
	}

	public bool Upgrade
	{
		get
		{
			return upgrade;
		}
		set
		{
			upgrade = value;
		}
	}

	public bool NeedMoveEffect { get; set; }

	public UnityAction NormalButtonCallback
	{
		get
		{
			return normalButtonCallback;
		}
		set
		{
			normalButtonCallback = value;
		}
	}

	public UnityAction<UnityAction> ADButtonCallback
	{
		get
		{
			return adButtonCallback;
		}
		set
		{
			adButtonCallback = value;
		}
	}

	public bool IsAutoPlayEffect
	{
		get
		{
			return m_IsAutoPlayEffect;
		}
		set
		{
			m_IsAutoPlayEffect = value;
		}
	}
}
