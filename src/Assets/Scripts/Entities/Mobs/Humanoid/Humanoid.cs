using UnityEngine;
using System.Collections.Generic;

using Inventory;

public abstract class Humanoid : Mob
{
	/// <summary>
	/// The multiplier of the mob's walking speed.
	/// </summary>
	public float walkSpeedFactor = .5f;

	/// <summary>
	/// The velocity with which this mob is forced forward when dodgeing.
	/// </summary>
	public float dodgeSpeed = 600f;
	protected float dodgeAngle = 15f;

	/// <summary>
	/// Minimal amount of seconds that should pass betwen two dodge-rolls.
	/// </summary>
	public float dodgeCooldown = 1f;

	/// <summary>
	/// How much stamina does a dodgeroll withdraw.
	/// </summary>
	public float dodgeStaminaCost = 25f;

	/// <summary>
	/// How much stamina does sprinting withdraw every frame.
	/// </summary>
	public float sprintStaminaCost = 15f;

	/// <summary>
	/// The multiplier of the mob's sprinting speed.
	/// </summary>
	public float sprintSpeedFactor = 1.5f;

	private Vector3 velocityBuffer = Vector3.zero;
	private readonly float movementSmoothing = .01f;

	// <TODO> Will do for now, may be unified or universalized later.
	[SerializeField]
	private ItemSocket leftHand;
	[SerializeField]
	private ItemSocket rightHand;
	[SerializeField]
	private ItemSocket back;
	[SerializeField]
	private ItemSocket belt;

	public bool CanDodge
	{
		get
		{
			if (!CanMoveActively)
				return false;

			switch (MovementState)
			{
			case MovementState.Standing:
			case MovementState.Walking:
				return false;
			}

			return Stamina > dodgeStaminaCost;
		}
	}

	public bool CanSprint
	{
		get
		{
			if (!CanMoveActively)
				return false;

			switch (MovementState)
			{
			case MovementState.Standing:
				return false;
			}

			return Stamina > sprintStaminaCost;
		}
	}

	public override MovementType MovementType
	{
		get => base.MovementType;
		set
		{
			if (value == MovementType.Sprinting && !CanSprint)
				return;
			base.MovementType = value;
		}
	}

	public override void DashAction() => Dodge();

	/// <summary>
	/// Performs a dodge attempt.
	/// </summary>
	public void Dodge()
	{
		if (CanDodge)
			MovementState = MovementState.Dodging;
	}

	public override void Move(
		float delta,
		Vector3 direction,
		bool affectY = false
	)
	{
		if (!CanMoveActively)
			return;

		float speed = moveSpeed;

		if (direction.magnitude <= movementHaltThreshold)
		{
			MovementState = MovementState.Standing;
		}
		else
		{
			if (direction.magnitude > 1f)
				direction.Normalize();

			switch (MovementType)
			{
			case MovementType.Walking:
				speed *= walkSpeedFactor;
				MovementState = MovementState.Walking;
				break;
			case MovementType.Sprinting:
				if (!CanSprint)
					goto default;

				speed *= sprintSpeedFactor;
				Stamina -= sprintStaminaCost * delta;
				MovementState = MovementState.Sprinting;
				break;
			default:
				MovementState = MovementState.Running;
				break;
			}
		}

		activeDirection = direction;

		Vector3 targetVelocity = speed * direction * delta;
		if (!affectY)
			targetVelocity.y = Body.velocity.y;

		Body.velocity = Vector3.SmoothDamp(
			Body.velocity,
			targetVelocity,
			ref velocityBuffer,
			movementSmoothing
		);

		if (turnsToMovementDirection)
			TurnTo(Body.velocity);
	}

	public void OnDodgeRoll()
	{
		Vector3 direction = activeDirection.magnitude > 0f ? activeDirection : transform.forward;
		direction.y = 0f;
		direction.Normalize();

		if (turnsToMovementDirection)
			transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

		Vector3 force = Quaternion.AngleAxis(-dodgeAngle, transform.right) * direction * dodgeSpeed;

		Stamina -= dodgeStaminaCost;

		Body.velocity = Vector3.zero;
		Body.AddForce(force, ForceMode.Impulse);
	}

	public void OnDodgeRollEnd()
	{
		MovementState = MovementState.Sprinting;
	}

	public override ItemSocket GunSocket => rightHand;
}
