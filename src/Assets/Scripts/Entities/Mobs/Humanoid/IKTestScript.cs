using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTestScript : MonoBehaviour
{
	public Transform target;

	protected Animator Animator { get; private set; }

	protected virtual void Awake()
	{
		Animator = GetComponent<Animator>();
	}

	private void OnAnimatorIK()
	{
		if (target)
		{
			Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
			Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
			Animator.SetIKPosition(AvatarIKGoal.LeftHand, target.position);
			Animator.SetIKRotation(AvatarIKGoal.LeftHand, target.rotation);
		}
	}
}
