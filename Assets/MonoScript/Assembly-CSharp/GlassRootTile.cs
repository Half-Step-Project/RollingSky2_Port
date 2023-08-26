using UnityEngine;

public class GlassRootTile : BaseGlassTile
{
	public override bool IfRebirthRecord
	{
		get
		{
			return true;
		}
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (currentState == GlassState.Wait)
		{
			currentState = GlassState.Active;
		}
	}
}
