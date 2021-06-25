using UnityEngine;

public class HumanoidEventListener : MobEventListener
{
	private Humanoid humanoid;

	protected override void Initialize()
	{
		base.Initialize();
		humanoid = transform.parent.GetComponent<Humanoid>();
	}

	public void OnDodgeRollBegin() => humanoid.OnDodgeRoll();

	public void OnDodgeRollEnd() => humanoid.OnDodgeRollEnd();
}
