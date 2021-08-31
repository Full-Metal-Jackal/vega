﻿using UnityEngine;

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

	public void SetupHandsIkForItem()
	{
		Inventory.Item item = humanoid.ActiveItem;
		
		if (item && item.Model)
		{
			leftHandIkTarget = item.Model.LeftHandGrip;
			rightHandIkTarget = item.Model.RightHandGrip;
		}
		else
		{
			leftHandIkTarget = rightHandIkTarget = null;
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

	protected override void Awake()
	{
		base.Awake();

		humanoid = transform.parent.GetComponent<Humanoid>();
		humanoid.OnItemChanged += SetupHandsIkForItem;
	}

	public void OnDodgeRollBegin()
	{
		humanoid.OnDodgeRoll();

		// We set a separate trigger to cease animations like reloading, because otherwise
		// the player could press dodge and then press reload before the dodge itself took place,
		// thus reloading while dodging.
		Animator.SetTrigger("StopAdditionalAnimations");
		
		AdditionalLayersEnabled = false;
		TransitIkWeightTo(0f, .1f);
	}

	public void OnDodgeRollEnd()
	{
		humanoid.OnDodgeRollEnd();

		AdditionalLayersEnabled = true;
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

	private void OnAnimatorIK()
	{
		if (LookAtIkEnabled)
		{
			float lookAtDistance = HorizontalDistance(humanoid.SmoothedAimPos, Mob.transform.position);
			float weight = ikTransition;
			if (!humanoid.IsAiming)
			{
				weight *= Mathf.Clamp01((lookAtDistance - humanoid.MinAimDistance) / ikBlendingDistance);
				weight *= lookAtIkNonAimingFactor;
			}
			Animator.SetLookAtWeight(weight, bodyIkWeight, headIkWeight);
			Animator.SetLookAtPosition(humanoid.SmoothedAimPos);
		}
		else
		{
			Animator.SetLookAtWeight(0);
		}

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
			SetIkWeights(AvatarIKGoal.RightHand, 0f);
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

		if (ikTransitionGoal == ikTransition)
			return;

		if (ikTransitionGoal > ikTransition)
			ikTransition = Mathf.Min(ikTransition += Time.deltaTime / ikTransitionTime, ikTransitionGoal);
		else
			ikTransition = Mathf.Max(ikTransition -= Time.deltaTime / ikTransitionTime, ikTransitionGoal);
	}

	public bool AdditionalLayersEnabled
	{
		set
		{
			float weight = value ? 1f : 0f;
			for (int i = 1; i < Animator.layerCount; i++)
				Animator.SetLayerWeight(i, weight);
		}
	}

	public void OnRagdoll()
	{
		if (Animator.TryGetComponent(out RagdollController ragdollController))
			ragdollController.ToggleRagdoll(true);
	}
}
