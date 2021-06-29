using UnityEngine;

public class HumanoidAnimationHandler : MobAnimationHandler
{
	private Humanoid humanoid;

	protected Transform leftHandIkTarget;
	protected Transform rightHandIkTarget;

	[SerializeField]
	private float headIkWeight = .8f;
	[SerializeField]
	private float bodyIkWeight = .6f;

	public bool lookAtIkEnabled;

	public void SetupHandsIkForItem(Inventory.Item item)
	{
		// <TODO> There will be a more sophisticated determination of what hand is aiming and what is holding later.
		leftHandIkTarget = item.Model.LeftHandHandle;
		rightHandIkTarget = Mob.AimTransform;
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
		lookAtIkEnabled = true;
	}

	public void OnDodgeRollBegin() => humanoid.OnDodgeRoll();

	public void OnDodgeRollEnd() => humanoid.OnDodgeRollEnd();

	private void OnAnimatorIK()
	{
		if (lookAtIkEnabled)
		{
			Animator.SetLookAtWeight(1, bodyIkWeight, headIkWeight);
			Animator.SetLookAtPosition(Mob.AimPos);
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
