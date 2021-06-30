using UnityEngine;

public class HumanoidAnimationHandler : MobAnimationHandler
{
	private Humanoid humanoid;

	protected Transform leftHandIkTarget;
	protected Transform rightHandIkTarget;

	public bool LookAtIkEnabled { get; set; }
	[SerializeField]
	private float lookAtIkSmoothing = .1f;
	[SerializeField]
	private float headIkWeight = .8f;
	[SerializeField]
	private float bodyIkWeight = .6f;
	private Vector3 lookAtPosVelocity = Vector3.zero;
	private Vector3 lookAtPos = Vector3.zero;

	/// <summary>
	/// The minimum distance from the mob to its AimPos for IK to work.
	/// </summary>
	[SerializeField]
	private float lookAtIkThreshold = 1.8f;
	private readonly float thresholdSmoothingDistance = 5f;

	public Vector3 SmoothLookAtPos() =>
		lookAtPos = Vector3.SmoothDamp(lookAtPos, Mob.AimPos, ref lookAtPosVelocity, lookAtIkSmoothing);

	public void SetupHandsIkForItem(Inventory.Item item)
	{
		// <TODO> There will be a more sophisticated determination of what hand is aiming and what is holding later.
		leftHandIkTarget = item.Model.LeftHandHandle;
		// rightHandIkTarget = Mob.aim;
	}

	private void SetIkWeights(AvatarIKGoal goal, float weight)
	{
		Animator.SetIKPositionWeight(goal, weight);
		Animator.SetIKRotationWeight(goal, weight);
	}

	private void SetIkTransform(AvatarIKGoal goal, Transform transform)
	{
		Animator.SetIKPosition(goal, transform.position);
		Animator.SetIKRotation(goal, transform.rotation);
	}

	protected override void Initialize()
	{
		base.Initialize();
		humanoid = transform.parent.GetComponent<Humanoid>();
		humanoid.OnItemChange += item => SetupHandsIkForItem(item);
	}

	protected override void Setup()
	{
		base.Setup();
		LookAtIkEnabled = true;
	}

	public void OnDodgeRollBegin() => humanoid.OnDodgeRoll();

	public void OnDodgeRollEnd() => humanoid.OnDodgeRollEnd();

	private void OnAnimatorIK()
	{
		if (LookAtIkEnabled)
		{
		float lookAtDistance = Vector3.Distance(lookAtPos, Mob.transform.position);
			Animator.SetLookAtWeight(
				Mathf.Clamp01((lookAtDistance - lookAtIkThreshold)/thresholdSmoothingDistance),
				bodyIkWeight,
				headIkWeight
			);
			Animator.SetLookAtPosition(SmoothLookAtPos());
		}
		else
		{
			Animator.SetLookAtWeight(0);
		}

		SetIkWeights(AvatarIKGoal.LeftHand, leftHandIkTarget ? 1 : 0);
		if (leftHandIkTarget)
			SetIkTransform(AvatarIKGoal.LeftHand, leftHandIkTarget);

		SetIkWeights(AvatarIKGoal.RightHand, rightHandIkTarget ? 1 : 0);
		if (rightHandIkTarget)
			SetIkTransform(AvatarIKGoal.RightHand, rightHandIkTarget);
	}
}
