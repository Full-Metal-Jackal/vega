﻿using System;
using UnityEngine;

public class Mob : DynamicEntity, IDamageable, IPossessable
{
	public virtual float MaxHealth { get; set; } = 100;
	public float Health { get; protected set; }

	/// <summary>
	/// The mob's running speed.
	/// </summary>
	public float moveSpeed = 250f;

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
	/// The multiplier of the mob's sprinting speed.
	/// </summary>
	public float sprintSpeedFactor = 1.5f;

	public readonly float movementHaltThreshold = .01f;
	public readonly bool turnsToMovementDirection = true;
	protected Vector3 activeDirection = Vector3.zero;

	private Vector3 velocityBuffer = Vector3.zero;
	private readonly float movementSmoothing = .01f;

	public bool Alive { get; protected set; } = true;

	public MobController Controller { get; set; }

	private MovementState movementState = MovementState.Standing;

	public MovementState MobMovementState
	{
		get => movementState;
		set
		{
			movementState = value;
			if (!animator)
				return;

			string animatorVariable = "MovementState";
			switch (movementState)
			{
			case MovementState.Standing:
				animator.SetInteger(animatorVariable, 0);
				break;
			case MovementState.Walking:
				animator.SetInteger(animatorVariable, 1);
				break;
			case MovementState.Running:
				animator.SetInteger(animatorVariable, 2);
				break;
			case MovementState.Sprinting:
				animator.SetInteger(animatorVariable, 3);
				break;
			case MovementState.Dodging:
				animator.SetTrigger("DodgeRollTrigger");
				break;
			default:
				break;
			}
		}
	}

	protected Animator animator;

	public Mob()
	{
		Name = "unnamed mob";
		Health = MaxHealth;
	}

	protected override bool Initialize()
	{
		animator = GetComponentInChildren<Animator>();
		return base.Initialize();
	}

	public void TakeDamage()
	{
		throw new NotImplementedException();
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

		switch (requestedState)
		{
		case MovementState.Walking:
			speed *= walkSpeedFactor;
			break;
		case MovementState.Sprinting:
			speed *= sprintSpeedFactor;
			break;
		case MovementState.Dodging:
			MobMovementState = requestedState;
			return;
		default:
			break;
		}
		MobMovementState = requestedState;

		Vector3 targetVelocity = speed * direction * delta;
		if (!affectY)
			targetVelocity.y = Body.velocity.y;
		
		Body.velocity = Vector3.SmoothDamp(
			Body.velocity,
			targetVelocity,
			ref velocityBuffer,
			movementSmoothing
		);

		Vector3 rotateTo = Body.velocity;
		rotateTo.y = 0f;
		if (turnsToMovementDirection && rotateTo.magnitude > movementHaltThreshold)
			transform.rotation = Quaternion.LookRotation(rotateTo, Vector3.up);
		
	}

	/// <summary>
	/// Makes the mob possessed by the provided controller.
	/// </summary>
	/// <param name="controller">The controller that should possess the mob.</param>
	/// <returns></returns>
	public bool SetPossessed(MobController controller)
	{
		if (Controller)
			return false;
		Controller = controller;
		return true;
	}

	public bool Use(IInteractable interactable) => interactable.CanBeUsedBy(this) && interactable.OnUse(this);

	public void OnDodgeRoll() 
	{
		Vector3 direction = activeDirection;
		direction.y = 0f;
		direction.Normalize();

		if (turnsToMovementDirection)
			transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

		Vector3 force = Quaternion.AngleAxis(-dodgeAngle, transform.right) * direction * dodgeSpeed;

		Body.velocity = Vector3.zero;
		Body.AddForce(force, ForceMode.Impulse);
	}

	public void OnDodgeRollEnd() => MobMovementState = MovementState.Sprinting;

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
}
