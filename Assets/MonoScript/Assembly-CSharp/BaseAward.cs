using RS2;
using UnityEngine;

public class BaseAward : BaseEnemy
{
	public const string NodeShowModelPath = "model/showNode";

	public const string NodeHideModelPath = "model/hideNode";

	protected GameObject modelShowNode;

	protected GameObject modelHideNode;

	protected Animation animShowNode;

	protected Animation animHideNode;

	public override void Initialize()
	{
		base.Initialize();
		Transform transform = base.transform.Find("model/showNode");
		Transform transform2 = base.transform.Find("model/hideNode");
		modelShowNode = (transform ? transform.gameObject : null);
		modelHideNode = (transform2 ? transform2.gameObject : null);
		animShowNode = (modelShowNode ? modelShowNode.GetComponent<Animation>() : null);
		animHideNode = (modelHideNode ? modelHideNode.GetComponent<Animation>() : null);
	}

	public override void ResetElement()
	{
		base.ResetElement();
	}

	public override void TriggerEnter(BaseRole ball)
	{
		OnCollideBall(ball);
	}

	protected virtual void OnCollideBall(BaseRole ball)
	{
	}

	public override void CoupleTriggerEnter(BaseCouple couple, Collider collider)
	{
		OnCollideCouple(couple, collider);
	}

	protected virtual void OnCollideCouple(BaseCouple couple, Collider collider)
	{
	}
}
