using System.Collections.Generic;
using UnityEngine;

using Inventory;

public abstract class Mob : DynamicEntity, IDamageable
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
	[field: SerializeField]
	public float MoveSpeed { get; private set; }  = 250f;

	protected readonly float movementHaltThreshold = .01f;

	[SerializeField]
	protected readonly bool turnsToMovementDirection = true;
	protected readonly float rotationThreshold = .01f;

	protected Vector3 activeDirection = Vector3.zero;

	public bool Alive { get; protected set; } = true;

	public MobController Controller { get; set; }

	public delegate void ActiveItemAction(Item item);
	public event ActiveItemAction OnItemChange;
	public virtual Item ActiveItem
	{
		get => activeItem;
		set
		{
			activeItem = value;

			OnItemChange?.Invoke(activeItem);
		}
	}
	private Item activeItem;

	/// <summary>
	/// The current state of the mob, represents mostly the animation that is being played right now.
	/// </summary>
	public virtual MobState MovementState
	{
		get => movementState;
		protected set
		{
			movementState = value;
			if (!Animator)
				return;

			const string animatorVariable = "MovementState";
			switch (movementState)
			{
			case MobState.Standing:
				Animator.SetInteger(animatorVariable, 0);
				break;
			case MobState.Walking:
				Animator.SetInteger(animatorVariable, 1);
				break;
			case MobState.Running:
				Animator.SetInteger(animatorVariable, 2);
				break;
			case MobState.Sprinting:
				Animator.SetInteger(animatorVariable, 3);
				break;
			case MobState.Dodging:
				Animator.SetTrigger("DodgeRollTrigger");
				break;
			}
		}
	}
	private MobState movementState = MobState.Standing;

	/// <summary>
	/// The way this mob moves continuously: walking, running or sprinting.
	/// </summary>
	public virtual MovementType MovementType { get; set; } = MovementType.Running;

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
	/// Attempts to make the mob dash, dodgeroll etc.
	/// </summary>
	public virtual void DashAction()
	{
	}

	/// <summary>
	/// Handles the mob's active movement.
	/// </summary>
	/// <param name="delta">Delta between two ticks.</param>
	/// <param name="direction">Vector describing the movement of the mob with magnitude between 0 and 1.</param>
	/// <param name="affectY">If the request should influence the mob's vertical movement.</param>
	public virtual void Move(
		float delta,
		Vector3 direction,
		bool affectY = false
	)
	{
		if (!CanMoveActively)
			return;

		if (direction.magnitude <= movementHaltThreshold)
		{
			MovementState = MobState.Standing;
		}
		else
		{
			if (direction.magnitude > 1f)
				direction.Normalize();
			MovementState = MobState.Running;
		}

		activeDirection = direction;

		Vector3 targetVelocity = MoveSpeed * direction * delta;
		if (!affectY)
			targetVelocity.y = Body.velocity.y;

		Body.velocity = targetVelocity;

		if (turnsToMovementDirection)
			TurnTo(Body.velocity);
	}

	/// <summary>
	/// Makes the mob's whole body face the specified direction.
	/// </summary>
	/// <param name="rotateTo">The direction to face.</param>
	public virtual void TurnTo(Vector3 rotateTo)
	{
		rotateTo.y = 0f;
		if (rotateTo.magnitude <= rotationThreshold)
			return;

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
			switch (MovementState)
			{
			case MobState.Dead:
			case MobState.Dodging:
			case MobState.Unconscious:
				return false;
			}

			return true;
		}
	}

	public virtual ItemSocket GunSocket => null;
}
