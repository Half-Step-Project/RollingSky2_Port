using System;

public class CMPBUAdsEventArgs : EventArgs
{
	public string adUnitId { get; set; }

	public string extraMessage { get; set; }

	public CMPBUAdsEventArgs()
	{
	}

	public CMPBUAdsEventArgs(string adUnitId)
	{
		this.adUnitId = adUnitId;
	}

	public CMPBUAdsEventArgs(string adUnitId, string extraMessage)
	{
		this.adUnitId = adUnitId;
		this.extraMessage = extraMessage;
	}
}
