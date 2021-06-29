using UnityEngine;

public class HumanoidAnimationHandler : MobAnimationHandler
{
	private Humanoid humanoid;

	protected Transform leftHandHandle;
	protected Transform rightHandHandle;

	public bool lookAtIkEnabled;

	public void SetupHandsIkForItem(Inventory.Item item)
	{
		leftHandHandle = item.LeftHandHandle;
		rightHandHandle = item.RightHandHandle;
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
		Animator.SetLookAtWeight(lookAtIkEnabled ? 1 : 0); // This method can only be called from this callback for some reason.
		if (lookAtIkEnabled)
			Animator.SetLookAtPosition(CameraController.GetWorldCursorPosition());

		SetIkWeights(AvatarIKGoal.LeftHand, leftHandHandle ? 1 : 0);
		if (leftHandHandle)
			SetIkTransform(AvatarIKGoal.LeftHand, leftHandHandle);

		SetIkWeights(AvatarIKGoal.RightHand, rightHandHandle ? 1 : 0);
		if (rightHandHandle)
			SetIkTransform(AvatarIKGoal.RightHand, rightHandHandle);
	}
}
