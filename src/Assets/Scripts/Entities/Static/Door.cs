using UnityEngine;

public class Door : StaticEntity, IInteractable
{
	public float radius = 3f;
	public bool opened = false;
	public bool active = false;

	public Animator Anim;
	public bool Selectable { get; set; } = true;

	protected override bool Initialize()
	{
		Anim = GetComponent<Animator>();
		return base.Initialize();
	}

	public bool CanBeUsedBy(Mob mob) => !active;

		public bool OnUse(Mob mob)
	{
		Debug.Log(this + " Has been used");
		if (opened)
		{
			Anim.SetBool("IsOpening", false);
		}
		else
		{
			Anim.SetBool("IsOpening", true);
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
