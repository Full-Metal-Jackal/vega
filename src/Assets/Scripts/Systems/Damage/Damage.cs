using UnityEngine;

[System.Serializable]
public struct Damage
{
	public float amount;
	public DamageType type;

	// If this damage can cause the mob to drop incapacitated if they have to stamina, e.g. taser or tear gas or EMP for robots.
	public bool incapacitating;

	[HideInInspector]
	public Entity inflictor;
	// The force applied to the ragdoll on impact.
	
	public float force;
	// The direction of the damage, used along with the force.
	[HideInInspector]
	public Vector3 direction;
	
	// Position of impact assigned at the very moment of impact.
	[HideInInspector]
	public Vector3 hitPoint;

	// <TODO> Not implemented in Mob.cs yet!!
	public bool ignoresShield;

	public bool AppliesForce => type is DamageType.Kinetic;

	public Damage(
		float amount,
		DamageType type,

		bool incapacitating = false,
		bool ignoresShield = false,

		float force = 0f,
		Vector3 direction = new Vector3(),
		Vector3 hitPoint = new Vector3(),

		Entity inflictor = null
		)
	{
		this.amount = amount;
		this.type = type;
		
		this.incapacitating = incapacitating;
		this.ignoresShield = ignoresShield;

		this.inflictor = inflictor;

		this.force = force;
		this.direction = direction;
		this.hitPoint = hitPoint;
	}
}
