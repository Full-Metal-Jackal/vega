using UnityEngine;
using UnityEngine.Events;

public class Door : Interaction
{
	[field: SerializeField]
	public float Radius { get; private set; } = 3f;

	[SerializeField]
	private bool startOpened;

	private bool __opened = false;
	public bool Opened
	{
		get => __opened;
		private set
		{
			__opened = value;
			animator.SetBool("IsOpening", __opened);
		}
	}

	[field: SerializeField]
	public bool Locked { get; private set; } = false;

	// Event used to make noises and flash lights to indicate that the door is locked.
	[SerializeField]
	private UnityEvent lockEvent;

	[SerializeField]
	private Animator animator;

	protected void Start() =>
		Opened = startOpened;

	public override bool CanBeUsedBy(Mob mob) => !Locked;

	public override bool OnUse(Mob mob)
	{
		if (Locked)
		{
			lockEvent?.Invoke();
			return false;
		}

		Opened = !Opened;

		return true;
	}
}
