using UnityEngine;

public class HumanoidAnimationHandler : MobAnimationHandler
{
	private Humanoid humanoid;

	private Transform rightShoulder;

	public bool LookAtIkEnabled { get; set; }
	[SerializeField]
	private float lookAtIkSmoothing = .1f;
	[SerializeField]
	private float headIkWeight = 1f;
	[SerializeField]
	private float bodyIkWeight = .75f;
	private Vector3 lookAtPosVelocity = Vector3.zero;
	private Vector3 lookAtPos;

	/// <summary>
	/// The minimum distance from the mob to its AimPos for IK to work.
	/// </summary>
	[SerializeField]
	private float ikMinDistance = 1.8f;
	private readonly float ikBlendingDistance = 2f;

	private Transform leftHandIkTarget;
	private Transform rightHandIkTarget;
	
	// Right now, only right hand is supported as the aiming hand, which is sufficient for our current goals.
	private bool aimingHand = false;

	public void UpdateLookAtPos() => lookAtPos = Vector3.SmoothDamp(
			lookAtPos,
			Mob.AimPos,
			ref lookAtPosVelocity,
			lookAtIkSmoothing
		);

	public Quaternion AimingHandRotation
	{
		get
		{
			Vector3 dir = lookAtPos - rightShoulder.position;
			return Quaternion.AngleAxis(-90f, dir) * Quaternion.LookRotation(dir, Vector3.up);
		}
	}

	public void SetupHandsIkForItem(Inventory.Item item)
	{
		leftHandIkTarget = item.Model.LeftHandGrip;
		rightHandIkTarget = item.Model.RightHandGrip;
	}

	private void SetIkWeights(AvatarIKGoal goal, float weight)
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

	protected override void Initialize()
	{
		base.Initialize();

		humanoid = transform.parent.GetComponent<Humanoid>();
		humanoid.OnItemChange += item => SetupHandsIkForItem(item);

		rightShoulder = Animator.GetBoneTransform(HumanBodyBones.RightShoulder);
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
		UpdateLookAtPos();

		if (LookAtIkEnabled)
		{
			float lookAtDistance = Vector3.Distance(lookAtPos, Mob.transform.position);
			float weight = Mathf.Clamp01((lookAtDistance - ikMinDistance) / ikBlendingDistance);
			Animator.SetLookAtWeight(weight, bodyIkWeight, headIkWeight);
			Animator.SetLookAtPosition(lookAtPos);
		}
		else
		{
			Animator.SetLookAtWeight(0);
		}

		SetIkWeights(AvatarIKGoal.LeftHand, leftHandIkTarget ? 1 : 0);
		if (leftHandIkTarget)
			SetIkTransform(AvatarIKGoal.LeftHand, leftHandIkTarget);

		if (aimingHand)
		SetIkWeights(AvatarIKGoal.RightHand, rightHandIkTarget ? 1 : 0);
		if (rightHandIkTarget)
			SetIkTransform(AvatarIKGoal.RightHand, rightHandIkTarget);
	}
}
