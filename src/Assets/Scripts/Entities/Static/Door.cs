using UnityEngine;

public class Door : StaticEntity, IInteractable
{
	public float radius = 3f;
	public bool opened = false;
	public bool active = false;

	private string tagName = "TempCollision"; //Changable if needed 

	public Animator[] Anim;
	public bool Selectable { get; set; } = true;

	protected override bool Initialize()
	{
		Anim = GetComponentsInChildren<Animator>();
		return base.Initialize();
	}

	public bool CanBeUsedBy(Mob mob) => !active;

	public bool OnUse(Mob mob)
	{
		if (opened)
		{
			Anim[0].SetBool("IsOpening", false);
			Anim[1].SetBool("IsOpening", false);
		}
		else
		{
			Anim[0].SetBool("IsOpening", true);
			Anim[1].SetBool("IsOpening", true);
		}
		opened = !opened;
		return true;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
