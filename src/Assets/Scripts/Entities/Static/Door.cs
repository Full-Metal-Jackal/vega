using UnityEngine;

public class Door : StaticEntity, IInteractable
{
	public float radius = 3f;
	public bool opened = false;
	public bool active = false;

	private string tagName = "TempCollision"; //Changable if needed 

	public Animator Anim;
	public bool Selectable { get; set; } = true;

	protected override bool Initialize()
	{
		Anim = GetComponentInChildren<Animator>();
		return base.Initialize();
	}

	public bool CanBeUsedBy(Mob mob) => !active;

	public bool OnUse(Mob mob)
	{
		if (opened)
		{
			Anim.SetBool("IsOpening", false);
			foreach (Transform elem in GetComponentInChildren<Transform>())
			{
				if (elem.CompareTag(tagName))
				{
					elem.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			Anim.SetBool("IsOpening", true);
			foreach (Transform elem in GetComponentInChildren<Transform>())
			{
				if (elem.CompareTag(tagName))
				{
					elem.gameObject.SetActive(false);
				}
			}
		}
		opened = !opened;
		return true;
	}
}
