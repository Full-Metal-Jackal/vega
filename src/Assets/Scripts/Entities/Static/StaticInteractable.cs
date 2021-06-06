using UnityEngine;

public class StaticInteractable : StaticEntity, IInteractable
{
	public float radius = 3f;
	public bool active = false;

	public bool Selectable { get; set; } = true;

	public bool CanBeUsedBy(Mob mob) => !active;

	public bool OnUse(Mob mob)
	{
		return true;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, radius);
	}



}
