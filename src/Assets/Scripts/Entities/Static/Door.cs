using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
	[field: SerializeField]
	public float Radius { get; private set; } = 3f;

	[field: SerializeField]
	public bool Opened { get; private set; } = false;

	[field: SerializeField]
	public bool Active { get; private set; } = false;

	public Animator[] Animator;
	public bool Selectable { get; set; } = true;

	private void Awake() =>
		Initialize();

	protected virtual void Initialize() =>
		Animator = GetComponentsInChildren<Animator>();

	public bool CanBeUsedBy(Mob mob) => !Active;

	public bool OnUse(Mob mob)
	{
		if (Opened)
		{
			Animator[0].SetBool("IsOpening", false);
			Animator[1].SetBool("IsOpening", false);
		}
		else
		{
			Animator[0].SetBool("IsOpening", true);
			Animator[1].SetBool("IsOpening", true);
		}

		Opened = !Opened;
		return true;
	}
}
