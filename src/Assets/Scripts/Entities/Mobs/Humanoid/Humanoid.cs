using UnityEngine;

using Inventory;

public abstract class Humanoid : Mob
{
	/// <summary>
	/// The multiplier of the mob's walking speed.
	/// </summary>
	[field: SerializeField]
	public float WalkSpeedFactor { get; private set; } = .5f;

	/// <summary>
	/// The velocity with which this mob is forced forward when dodgeing.
	/// </summary>
	public float DodgeSpeed { get; private set; } = 600f;
	protected float dodgeAngle = 10f;

	/// <summary>
	/// Minimal amount of seconds that should pass betwen two dodge-rolls.
	/// </summary>
	[field: SerializeField]
	public float DodgeCooldown { get; private set; } = 1f;

	/// <summary>
	/// How much stamina does a dodgeroll withdraw.
	/// </summary>
	[field: SerializeField]
	public float DodgeStaminaCost { get; private set; } = 25f;

	/// <summary>
	/// How much stamina does sprinting withdraw every frame.
	/// </summary>
	[field: SerializeField]
	public float SprintStaminaCost { get; private set; } = 15f;

	/// <summary>
	/// The multiplier of the mob's sprinting speed.
	/// </summary>
	[field: SerializeField]
	public float SprintSpeedFactor { get; private set; } = 1.5f;

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

	private HoldType holdState = HoldType.None;
	/// <summary>
	/// Represents the aiming animation that is being played right now.
	/// </summary>
	public virtual HoldType HoldState
	{
		get => holdState;
		protected set
		{
			holdState = value;
			if (!Animator)
				return;

			const string animatorIsDevice = "HoldingDevice";
			bool isDevice = false;
			switch (holdState)
			{
			case HoldType.SingleHandDevice:
			case HoldType.TwoHandsDevice:
			case HoldType.Cyberdeck:
				break;
			}
			Animator.SetBool(animatorIsDevice, isDevice);

			const string animatorVariable = "HoldType";
			int animatorValue = 0;
			switch (holdState)
			{
			case HoldType.SingleHandPistol:
			case HoldType.SingleHandDevice:
				isDevice = true;
				animatorValue = 1;
				break;
			case HoldType.TwoHandsPistol:
			case HoldType.TwoHandsDevice:
				animatorValue = 2;
				break;
			case HoldType.AssaultRifle:
				animatorValue = 3;
				break;
			case HoldType.Shotgun:
				animatorValue = 4;
				break;
			}
			Animator.SetInteger(animatorVariable, animatorValue);
			Debug.Log($"{holdState}: {animatorValue}, {isDevice}");
		}
	}

	public bool CanDodge
	{
		get
		{
			if (!CanMoveActively)
				return false;

			switch (MovementState)
			{
			case MobState.Standing:
			case MobState.Walking:
				return false;
			}

			return Stamina > DodgeStaminaCost;
		}
	}

	public bool CanSprint
	{
		get
		{
			if (!CanMoveActively)
				return false;

			if (MovementState == MobState.Standing)
				return false;

			return Stamina > SprintStaminaCost;
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

	public override Item ActiveItem
	{
		get => base.ActiveItem;
		set
		{
			HoldState = (base.ActiveItem = value) ? value.HoldType : HoldType.None;
		}
	}

	public override void Move(
		float delta,
		Vector3 direction,
		bool affectY = false
	)
	{
		if (!CanMoveActively)
			return;

		float speed = MoveSpeed;

		if (direction.magnitude <= movementHaltThreshold)
		{
			MovementState = MobState.Standing;
		}
		else
		{
			if (direction.magnitude > 1f)
				direction.Normalize();

			switch (MovementType)
			{
			case MovementType.Walking:
				speed *= WalkSpeedFactor;
				MovementState = MobState.Walking;
				break;
			case MovementType.Sprinting:
				if (!CanSprint)
					goto default;

				speed *= SprintSpeedFactor;
				Stamina -= SprintStaminaCost * delta;
				MovementState = MobState.Sprinting;
				break;
			default:
				MovementState = MobState.Running;
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

	public override void DashAction() => Dodge();

	/// <summary>
	/// Performs a dodge attempt.
	/// </summary>
	public void Dodge()
	{
		if (CanDodge)
			MovementState = MobState.Dodging;
	}

	public void OnDodgeRoll()
	{
		Vector3 direction = activeDirection.magnitude > 0f ? activeDirection : transform.forward;
		direction.y = 0f;
		direction.Normalize();

		if (turnsToMovementDirection)
			transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

		Vector3 force = Quaternion.AngleAxis(-dodgeAngle, transform.right) * direction * DodgeSpeed;

		Stamina -= DodgeStaminaCost;

		Body.velocity = Vector3.zero;
		Body.AddForce(force, ForceMode.Impulse);
	}

	public void OnDodgeRollEnd()
	{
		MovementState = MobState.Sprinting;
	}

	public override ItemSocket GunSocket => rightHand;
}
