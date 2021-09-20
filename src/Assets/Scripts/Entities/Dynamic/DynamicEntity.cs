using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DynamicEntity : Entity
{
	public Rigidbody Body { get; protected set; }

	protected override void Awake()
	{
		base.Awake();

		Body = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() =>
		MovePhysics(Time.fixedDeltaTime);

	/// <summary>
	/// Handles the entity's movement.
	/// </summary>
	/// <param name="delta">delta between two ticks</param>
	public virtual void MovePhysics(float delta)
	{
	}
}
