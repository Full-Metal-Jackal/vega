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
	public float DodgeSpeed { get; private set; } = 8f;
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


	[field: SerializeField]
	public Transform LeftHandSocket { get; private set; }

	[field: SerializeField]
	public Transform RightHandSocket { get; private set; }
	public override Transform ItemSocket => RightHandSocket;

	private bool __isAiming = false;
	/// <summary>
	/// Is the mob should currently aim, used for animation.
	/// </summary>
	public bool IsAiming
	{
		get => __isAiming;
		protected set
		{
			if (__isAiming == value)
				return;

			__isAiming = value;

			if (ActiveItem && ActiveItem.Automatic)
				UpdateItemTrigger();

			Animator.SetBool("IsAiming", __isAiming);
		}
	}

	private bool __isBusy;
	/// <summary>
	/// Is the mob is currently playing item usage animation: reloading a gun, throwing a grenade or deploying a drone.
	/// </summary>
	public bool IsBusy
	{
		get => __isBusy;
		protected set
		{
			__isBusy = value;
			Animator.SetBool("IsBusy", __isBusy);

			UpdateItemTrigger();
		}
	}

	public override bool CanUseItems => base.CanUseItems && !IsBusy;
	public override bool CanFire => base.CanFire && IsAiming;

	/// <summary>
	/// The minimum AimDistance required to aim.
	/// </summary>
	public virtual float MinAimDistance => .75f;
	/// <summary>
	/// Additive distance for MinAimDistance to start aiming.
	/// </summary>
	public virtual float AimEnableDistance => .1f;

	private HoldType __holdType = null;
	/// <summary>
	/// Represents the aiming animation that is being played right now.
	/// </summary>
	public virtual HoldType HoldType
	{
		get => __holdType;
		protected set
		{
			__holdType = value;
			if (!Animator)
				return;

			int animatorValue = 0;
			if (__holdType)
			{
				// Works fine w/o position offset for now.
				// ItemSocket.localRotation = __holdType.SocketRotOffset;
				animatorValue = __holdType.AnimatorValue;
			}
			Animator.SetInteger("HoldType", animatorValue);
		}
	}

	public bool CanDodge
	{
		get
		{
			if (!CanMoveActively)
				return false;

			switch (State) 
			{
			case MobState.Standing:
			case MobState.Walking:
				return false;
			}

			return !IsBusy && Stamina > DodgeStaminaCost;
		}
	}

	public bool CanSprint
	{
		get
		{
			if (!CanMoveActively)
				return false;

			if (State == MobState.Standing)
				return false;

			return Stamina > 0f;
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
			HoldType = (base.ActiveItem = value) ? value.HoldType : null;
		}
	}
	public bool HasAimableItem => ActiveItem && ActiveItem.IsAimable;

	protected override void Awake()
	{
		base.Awake();

		SocketFallback();
	}

	// A fallback for sockets assignement.
	private void SocketFallback()
	{
		if (!RightHandSocket)
		{
			Debug.LogWarning($"{this} has no RightHandSocket assigned!");
			RightHandSocket = Utils.FindChildRecursively(Animator.transform, "socket.hand.R");
		}
		if (!LeftHandSocket)
		{
			Debug.LogWarning($"{this} has no LeftHandSocket assigned!");
			LeftHandSocket = Utils.FindChildRecursively(Animator.transform, "socket.hand.L");
		}
	}

	public override void Move(float delta, Vector3 direction, bool affectY = false)
	{
		if (!CanMoveActively)
			return;

		float speed = MoveSpeed;

		if (direction.magnitude <= movementHaltThreshold)
		{
			State = MobState.Standing;
		}
		else
		{
			if (direction.magnitude > 1f)
				direction.Normalize();

			switch (MovementType)
			{
			case MovementType.Walking:
				speed *= WalkSpeedFactor;
				State = MobState.Walking;
				break;
			case MovementType.Sprinting:
				if (!CanSprint)
					goto default;

				speed *= SprintSpeedFactor;
				Stamina -= SprintStaminaCost * delta;
				State = MobState.Sprinting;
				break;
			default:
				State = MobState.Running;
				break;
			}
		}

		activeDirection = direction;
		if (HasAimableItem)
			UpdateAiming(delta);
		else
			TurnTo(delta, direction);
		UpdateLegsAnimation();

		Vector3 targetVelocity = speed * direction;
		if (!affectY)
			targetVelocity.y = Body.velocity.y;
		Body.velocity = targetVelocity;
	}

	/// <summary>
	/// Tells the legs whether they should run normally, strafe or run backwards.
	/// </summary>
	protected virtual void UpdateLegsAnimation()
	{
		Vector3 horDir = transform.forward;
		horDir.y = 0;

		Vector3 legsMovementVector = HasAimableItem
			? Quaternion.AngleAxis(
				Vector3.SignedAngle(horDir, activeDirection, Vector3.up),
				Vector3.up
			) * Vector3.forward
			: Vector3.forward;

		legsMovementVector *= Body.velocity.magnitude / MoveSpeed;

		Animator.SetFloat("MovementSide", legsMovementVector.x);
		Animator.SetFloat("MovementForward", legsMovementVector.z);
	}

	/// <summary>
	/// Updates the mob's aiming status and rotation.
	/// </summary>
	/// <param name="delta"></param>
	protected virtual void UpdateAiming(float delta)
	{
		Vector3 aimOrigin = transform.position + Vector3.up * AimHeight;
		Vector3 horAimDir = AimPos - transform.position;
		horAimDir.y = 0;
		Ray ray = new Ray(aimOrigin, horAimDir);

		float distance;
		if (Physics.Raycast(ray, out RaycastHit hit, AimEnableDistance + MinAimDistance))
			distance = hit.distance;
		else
			distance = horAimDir.magnitude;

		print(distance);

		if (IsAiming &= distance >= MinAimDistance)
			TurnTo(delta, AimDir);
		else
			IsAiming = distance >= AimEnableDistance + MinAimDistance;
	}

	public override void TurnTo(float delta, Vector3 rotateTo)
	{
		if (HoldType)
			rotateTo = HoldType.MobAngleOffset * rotateTo;

		base.TurnTo(delta, rotateTo);
	}

	public override void DashAction() => DodgeRoll();

	/// <summary>
	/// Performs a dodge roll attempt.
	/// </summary>
	public void DodgeRoll()
	{
		if (CanDodge)
			State = MobState.Dodging;
	}

	public void OnDodgeRoll()
	{
		UpdateItemTrigger();

		Vector3 direction = activeDirection.magnitude > 0f ? activeDirection : transform.forward;
		direction.y = 0f;
		direction.Normalize();

		SnapTurnTo(direction);

		Vector3 force = Quaternion.AngleAxis(-dodgeAngle, transform.right) * direction * DodgeSpeed;

		Stamina -= DodgeStaminaCost;

		Body.velocity = Vector3.zero;
		Body.AddForce(force, ForceMode.VelocityChange);
	}

	public void OnDodgeRollEnd()
	{
		State = MobState.Sprinting;

		lastStaminaDrain = Time.time;

		UpdateItemTrigger();
	}

	public override void Reload()
	{
		if (!(ActiveItem && ActiveItem.CanReload && CanReload))
			return;

		IsBusy = true;
		Animator.SetTrigger("ReloadTrigger");
	}

	public void OnReloadEnd()
	{
		ActiveItem.Reload();
		IsBusy = false;
	}

	public override void Throw()
	{
		if (!(ThrowableItem && ThrowableItem.CanFire && CanThrow))
			return;

		if (ActiveItem && ActiveItem.Model)
			ActiveItem.Model.gameObject.SetActive(false);
		ThrowableItem.SetupModel(ItemSocket);

		IsBusy = true;

		Animator.SetTrigger("ThrowTrigger");
	}

	public void OnThrowEnd()
	{
		ThrowableItem.SetTrigger(AimPos);
		IsBusy = false;
		if (ActiveItem && ActiveItem.Model)
			ActiveItem.Model.gameObject.SetActive(true);
	}

	public override void Die(Damage damage)
	{
		base.Die(damage);

		const float deathAnimationChance = .2f;
		const float deathAnimationVelocityThreshold = .1f;

		if (Random.value <= deathAnimationChance
			&& Body.velocity.magnitude < deathAnimationVelocityThreshold
			)
		{
			if (Animator.TryGetComponent(out HumanoidAnimationHandler animationHandler))
			{
				animationHandler.TransitIkWeightTo(0, .1f);
				animationHandler.DisableAdditionalLayers();
			}

			const int deathAnimationsVariety = 1;
			Animator.SetInteger("RandomAnimationIndex", Random.Range(0, deathAnimationsVariety - 1));
			Animator.SetTrigger("DeathTrigger");
		}
		else if (Animator.TryGetComponent(out RagdollController ragdollController))
		{
			ragdollController.ToggleRagdoll(true);
			if (damage.AppliesForce)
				ragdollController.ApplyRagdollForce(damage.direction * damage.force, damage.hitPoint);
		}
	}
}
