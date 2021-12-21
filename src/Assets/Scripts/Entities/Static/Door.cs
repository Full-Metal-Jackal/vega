using UnityEngine;

public class Door : Interaction
{
	[field: SerializeField]
	public float Radius { get; private set; } = 3f;

	[field: SerializeField]
	public bool Opened { get; private set; } = false;

	[field: SerializeField]
	public bool Locked { get; private set; } = false;

	[SerializeField]
	private Animator animator;

	public override bool CanBeUsedBy(Mob mob) => !Locked;

	public override bool OnUse(Mob mob)
	{
		if (Opened)
		{
			animator.SetBool("IsOpening", false);

		}
		else
		{
			animator.SetBool("IsOpening", true);
		}

		Opened = !Opened;
		return true;
	}
}
