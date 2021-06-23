using UnityEngine;

public class Door : Interaction
{
	[field: SerializeField]
	public float Radius { get; private set; } = 3f;

	[field: SerializeField]
	public bool Opened { get; private set; } = false;

	[field: SerializeField]
	public bool Active { get; private set; } = false;

	[SerializeField]
	private Animator[] animators;

	protected override void Initialzie()
	{
		base.Initialzie();
	}

	public override bool CanBeUsedBy(Mob mob) => !Active;

	public override bool OnUse(Mob mob)
	{
		if (Opened)
		{
			animators[0].SetBool("IsOpening", false);
			animators[1].SetBool("IsOpening", false);
		}
		else
		{
			animators[0].SetBool("IsOpening", true);
			animators[1].SetBool("IsOpening", true);
		}

		Opened = !Opened;
		return true;
	}
}
