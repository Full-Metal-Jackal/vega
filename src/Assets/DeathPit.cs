using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeathPit : MonoSingleton<DeathPit>
{
	[SerializeField]
	private float damage = 20f;

	protected override void Awake()
	{
		base.Awake();

	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Vector4(1f, 0f, 0f, .25f);

		BoxCollider boxCollider = GetComponent<BoxCollider>();
		Gizmos.DrawCube(boxCollider.center, boxCollider.size);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.transform.parent.TryGetComponent(out Mob mob))
			return;

		Damage fallDamage = new Damage(
			damage,
			DamageType.Fall,
			incapacitating: false
			);

		if (!mob.IsPlayer)
		{
			mob.Die(fallDamage);
			return;
		}

		mob.TakeDamage(fallDamage);
		mob.RecoverAfterFall();
	}
}
