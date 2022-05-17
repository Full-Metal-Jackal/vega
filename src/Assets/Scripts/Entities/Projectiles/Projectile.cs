using System;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Projectile : DynamicEntity
{
	public event Action<Collision> OnImpact;

	protected Damage damage;

	[SerializeField]
	private VisualEffect impactEffect;
	private const float impactEffectLife = 1.0f;

	private bool __ignoreHostiles = false;
	public bool IgnoreHostiles
	{
		get => __ignoreHostiles;
		set
		{
			if (__ignoreHostiles == value)
				return;

			int layer = (int)(
				(__ignoreHostiles = value) ? Layer.ProjectilesIgnoreHostile : Layer.Projectiles
			);

			foreach (Collider collider in Colliders)
				collider.gameObject.layer = layer;
		}
	}

	[SerializeField]
	private Transform projectileTip;

	/// <summary>
	/// Who (or what) shot Kennedy.
	/// </summary>
	public Entity Source { get; protected set; }

	[SerializeField]
	private ImpactType impactType;

	public virtual void Setup(Entity source, Damage damage, bool ignoreHostiles = false)
	{
		foreach (Collider sourceCollider in source.Colliders)
			foreach (Collider collider in Colliders)
				Physics.IgnoreCollision(collider, sourceCollider);

		Source = source;

		damage.inflictor = source;
		this.damage = damage;

		IgnoreHostiles = ignoreHostiles;
	}

	private void OnCollisionEnter(Collision collision)
	{
		Impact(collision.gameObject);

		ContactPoint cp = collision.GetContact(0);
		Vector3 pos = projectileTip ? projectileTip.position : cp.point;

		ImpactController.Instance.SpawnDecal(pos, cp, impactType, scale: 0.2f);
		if (impactEffect)
			ImpactController.Instance.SpawnImpactEffect(pos, cp, impactEffect, impactEffectLife);

		OnImpact?.Invoke(collision);
	}

	public virtual void Impact(GameObject other)
	{
	}

	public void Suicide() =>
		Destroy(gameObject);
}
