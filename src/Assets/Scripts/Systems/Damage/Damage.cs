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

	public bool AppliesForce => type is DamageType.Kinetic;
}
