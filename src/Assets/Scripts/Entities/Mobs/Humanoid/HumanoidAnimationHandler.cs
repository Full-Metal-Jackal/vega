using UnityEngine;

using static Utils;

public class HumanoidAnimationHandler : MobAnimationHandler
{
	private Humanoid humanoid;

	public bool LookAtIkEnabled { get; set; } = true;
	[SerializeField]
	private float headIkWeight = 1f;
	[SerializeField]
	private float bodyIkWeight = .75f;
	/// <summary>
	/// The value the "look at" IK weight is multiplied by when the mob is not aiming.
	/// </summary>
	[SerializeField]
	private float lookAtIkNonAimingFactor = .5f;

	/// <summary>
	/// How far from MinAimDistance IK should start to blend.
	/// </summary>
	[SerializeField]
	private float ikBlendingDistance = 2f;

	private float ikTransition = 1f;
	private float ikTransitionGoal = 1f;
	private float ikTransitionTime = .1f;

	protected Transform leftHandIkTarget;
	protected Transform rightHandIkTarget;

	protected int UpperBodyLayer { get; private set; }
	protected int ArmsLayer { get; private set; }
	private float[] previousLayersWeight;

	[SerializeField]
	private float aimPosSmoothing = .2f;
	private Vector3 aimPosSmoothingVelocity = Vector3.zero;

	protected override void Awake()
	{
		base.Awake();

		humanoid = transform.parent.GetComponent<Humanoid>();
		humanoid.OnActiveItemChanged += SetupHandsIkForItem;

		previousLayersWeight = new float[Animator.layerCount];
		UpperBodyLayer = Animator.GetLayerIndex("UpperBody");
		ArmsLayer = Animator.GetLayerIndex("Arms");
	}

	public void SetupHandsIkForItem(Inventory.Item item)
	{
		leftHandIkTarget = rightHandIkTarget = null;

		if (!(item && item.Model))
			return;

		if (!(item.HoldType && item.HoldType.UsesHandIk))
			return;

		switch (item.HoldType.Socket)
		{
		case HoldType.SocketType.RightHand:
			leftHandIkTarget = item.Model.LeftHandGrip;
			break;
		case HoldType.SocketType.LeftHand:
			rightHandIkTarget = item.Model.RightHandGrip;
			break;
		}
	}

	public void SetIkWeights(AvatarIKGoal goal, float weight)
	{
		Animator.SetIKPositionWeight(goal, weight);
		Animator.SetIKRotationWeight(goal, weight);
	}

	private void SetIkTransform(AvatarIKGoal goal, Transform transform) =>
		SetIkTransform(goal, transform.position, transform.rotation);

	private void SetIkTransform(AvatarIKGoal goal, Vector3 position, Quaternion rotation)
	{
		Animator.SetIKPosition(goal, position);
		Animator.SetIKRotation(goal, rotation);
	}

	public void OnDodgeRollBegin()
	{
		humanoid.OnDodgeRoll();

		// We set a separate trigger to cease animations like reloading, because otherwise
		// the player could press dodge and then press reload before the dodge itself took place,
		// thus reloading while dodging.
		Animator.SetTrigger("StopAdditionalAnimations");

		DisableAdditionalLayers();
		TransitIkWeightTo(0f, .1f);
	}

	public void OnDodgeRollEnd()
	{
		humanoid.OnDodgeRollEnd();

		EnableAdditionalLayers();
		TransitIkWeightTo(1f, .1f);
	}

	public void OnReloadBegin()
	{
		TransitIkWeightTo(0f, .1f);
	}

	public void OnReloadEnd()
	{
		humanoid.OnReloadEnd();
		TransitIkWeightTo(1f, .1f);
	}

	public void OnThrowBegin()
	{
		TransitIkWeightTo(0f, .1f);
	}

	public void OnThrowEnd()
	{
		humanoid.OnThrowEnd();
		TransitIkWeightTo(1f, .1f);
	}

	private void OnAnimatorIK(int layer)
	{
		if (layer == UpperBodyLayer)
		{
			if (!LookAtIkEnabled)
			{
				Animator.SetLookAtWeight(0);
				return;
			}

			float lookAtDistance = HorizontalDistance(SmoothedAimPos, Mob.transform.position);
			float weight = ikTransition;

			if (!humanoid.IsAiming)
			{
				weight *= Mathf.Clamp01((lookAtDistance - humanoid.MinAimDistance) / ikBlendingDistance);
				weight *= lookAtIkNonAimingFactor;
			}

			Animator.SetLookAtWeight(weight, bodyIkWeight, headIkWeight);
			Animator.SetLookAtPosition(SmoothedAimPos);
		}
		else if (layer == ArmsLayer)
		{
			if (leftHandIkTarget)
			{
				SetIkWeights(AvatarIKGoal.LeftHand, ikTransition);
				SetIkTransform(AvatarIKGoal.LeftHand, leftHandIkTarget);
			}
			else
			{
				SetIkWeights(AvatarIKGoal.LeftHand, 0f);
			}

			if (rightHandIkTarget)
			{
				SetIkWeights(AvatarIKGoal.RightHand, ikTransition);
				SetIkTransform(AvatarIKGoal.RightHand, rightHandIkTarget);
			}
			else
			{
				Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
			}
		}
	}

	/// <summary>
	/// Smoothly shifts the "look at" IK weight to another value.
	/// </summary>
	/// <param name="targetWeight">The weight the "look at" IK will assume at the end of transitionTime.</param>
	/// <param name="transitionTime">The time of the transition.</param>
	public void TransitIkWeightTo(float targetWeight, float transitionTime)
	{
		ikTransitionGoal = targetWeight;
		ikTransitionTime = transitionTime;
	}

	protected override void Update()
	{
		base.Update();

		UpdateSmoothedAimPos();

		if (humanoid.IsAiming)
		{
			Vector3 smoothedAimDir = humanoid.AimPos - humanoid.transform.position;
			smoothedAimDir.y = 0;

			// <TODO> this piece of code is needed only for holdtype offset tuning and should be deleted as soon as we determine them all.
			//if (humanoid.HoldType)
			//	humanoid.ItemSocket.localRotation = humanoid.HoldType.SocketRotOffset;
		}

		if (ikTransitionGoal == ikTransition)
			return;

		if (ikTransitionGoal > ikTransition)
			ikTransition = Mathf.Min(ikTransition += Time.deltaTime / ikTransitionTime, ikTransitionGoal);
		else
			ikTransition = Mathf.Max(ikTransition -= Time.deltaTime / ikTransitionTime, ikTransitionGoal);
	}

	public void DisableAdditionalLayers()
	{
		for (int i = 1; i < Animator.layerCount; i++)
		{
			previousLayersWeight[i] = Animator.GetLayerWeight(i);
			Animator.SetLayerWeight(i, 0f);
		}
	}

	public void EnableAdditionalLayers()
	{
		for (int i = 1; i < Animator.layerCount; i++)
			Animator.SetLayerWeight(i, 1f); //TODO PreviousWeight incorrect behavior
	}

	public Vector3 SmoothedAimPos { get; protected set; }
	public void UpdateSmoothedAimPos() => SmoothedAimPos = Vector3.SmoothDamp(
			SmoothedAimPos,
			humanoid.AimPos,
			ref aimPosSmoothingVelocity,
			aimPosSmoothing
		);

	private void OnDrawGizmosSelected()
	{
		if (!leftHandIkTarget)
			return;

		Gizmos.color = (Color.yellow + Color.red) * .5f;
		Gizmos.DrawWireSphere(leftHandIkTarget.position, .025f);
		Gizmos.DrawRay(leftHandIkTarget.position, leftHandIkTarget.forward * .065f);

		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(humanoid.AimPos, SmoothedAimPos - humanoid.AimPos);
		Gizmos.DrawWireSphere(SmoothedAimPos, .1f);
	}
}
