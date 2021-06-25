using UnityEngine;

public class Projectile : DynamicEntity
{
	[field: SerializeField]
	public float Damage { get; protected set; } = 10f;

	[field: SerializeField]
	public ProjecitleType Type { get; private set; } = ProjecitleType.Kinetic;

	public virtual void OnHit()
	{
	}
}
