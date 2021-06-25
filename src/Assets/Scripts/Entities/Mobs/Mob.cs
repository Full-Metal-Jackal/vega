using System.Collections.Generic;
using UnityEngine;

using Inventory;

public abstract class Mob : DynamicEntity, IDamageable, IPossessable
{
	[field: SerializeField]
	public virtual float MaxHealth { get; set; } = 100;
	[field: SerializeField]
	public float Health { get; protected set; }
	
	[field: SerializeField]
	public float Stamina { get; set; }
	[field: SerializeField]
	public virtual float MaxStamina { get; set; } = 100;
	
	[field: SerializeField]
	protected Animator Animator { get; private set; }

	[field: SerializeField]
	public MobInventory Inventory { get; private set; }

	/// <summary>
	/// The mob's running speed.
	/// </summary>
	public float moveSpeed = 250f;

	protected float movementHaltThreshold = .01f;
	protected bool turnsToMovementDirection = true;
	protected Vector3 activeDirection = Vector3.zero;

	public bool Alive { get; protected set; } = true;

	public MobController Controller { get; set; }

	private MovementState movementState = MovementState.Standing;
	public virtual MovementState MobMovementState
	{
		get => movementState;
		set
		{
			movementState = value;
			if (!Animator)
				return;

			const string animatorVariable = "MovementState";
			switch (movementState)
			{
			case MovementState.Standing:
				Animator.SetInteger(animatorVariable, 0);
				break;
			case MovementState.Walking:
				Animator.SetInteger(animatorVariable, 1);
				break;
			case MovementState.Running:
				Animator.SetInteger(animatorVariable, 2);
				break;
			case MovementState.Sprinting:
				Animator.SetInteger(animatorVariable, 3);
				break;
			case MovementState.Dodging:
				Animator.SetTrigger("DodgeRollTrigger");
				break;
			default:
				break;
			}
		}
	}

	protected override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		Health = MaxHealth;
		Stamina = MaxStamina;

		return true;
	}

	public void TakeDamage(float damage)
	{
		Health -= damage;
	}

	/// <summary>
	/// Handles the mob's active movement.
	/// </summary>
	/// <param name="delta">Delta between two ticks.</param>
	/// <param name="direction">Vector describing the movement of the mob with magnitude between 0 and 1.</param>
	/// <param name="requestedState">The movement state that is trying to be enforced on the mob.</param>
	/// <param name="affectY">If the request should influence the mob's vertical movement.</param>
	public virtual void Move(
		float delta,
		Vector3 direction,
		MovementState requestedState = MovementState.Running,
		bool affectY = false
	)
	{
		if (!CanMoveActively)
			return;

		if (direction.magnitude > 1f)
			direction.Normalize();
		else if (direction.magnitude <= movementHaltThreshold)
			requestedState = MovementState.Standing;
		activeDirection = direction;

		float speed = moveSpeed;

		MobMovementState = requestedState;

		Vector3 targetVelocity = speed * direction * delta;
		if (!affectY)
			targetVelocity.y = Body.velocity.y;

		Body.velocity = targetVelocity;

		Vector3 rotateTo = Body.velocity;
		rotateTo.y = 0f;
		if (turnsToMovementDirection && rotateTo.magnitude > movementHaltThreshold)
			transform.rotation = Quaternion.LookRotation(rotateTo, Vector3.up);
	}

	/// <summary>
	/// Makes the mob possessed by the provided controller.
	/// </summary>
	/// <param name="controller">The controller that should possess the mob.</param>
	/// <returns>true if the mob has been possessed succesfully, false otherwise.</returns>
	public bool SetPossessed(MobController controller)
	{
		if (Controller)
			return false;
		Controller = controller;
		return true;
	}

	public virtual bool Use(Interaction interaction) => interaction.CanBeUsedBy(this) && interaction.OnUse(this);

	public bool CanMoveActively
	{
		get
		{
			switch (MobMovementState)
			{
			case MovementState.Dead:
			case MovementState.Dodging:
			case MovementState.Unconscious:
				return false;
			default:
				break;
			}

			return true;
		}
	}

	public virtual ItemSocket GunSocket => null;
}
