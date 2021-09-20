using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RagdollController : MonoBehaviour
{
	protected Mob Mob { get; private set; }
	protected Animator Animator { get; private set; }

	protected IEnumerable<Rigidbody> Bones { get; private set; } = new HashSet<Rigidbody>();

	protected virtual void Awake()
	{
		Mob = transform.parent.GetComponent<Mob>();
		Animator = GetComponent<Animator>();
		Bones = GetComponentsInChildren<Rigidbody>();
	}

	protected virtual void Start()
	{
		ToggleRagdoll(false);
	}

	public void ToggleRagdoll(bool enable)
	{
		Animator.enabled = !enable;

		Mob.Body.isKinematic = enable;
		Mob.Body.detectCollisions = !enable;
		foreach (Rigidbody rigidbody in Bones)
		{
			rigidbody.isKinematic = !enable;
			rigidbody.detectCollisions = enable;
		}
	}

	public void ApplyRagdollForce(Vector3 force, Vector3 point)
	{
		const float maxBoneDistance = .5f;

		Rigidbody closestBone = null;
		float closestBoneDistance = maxBoneDistance;

		foreach (Rigidbody bone in Bones)
		{
			float distance = Vector3.Distance(bone.ClosestPointOnBounds(point), point);
			if (distance >= closestBoneDistance)
				continue;
			closestBoneDistance = distance;
			closestBone = bone;
		}

		if (!closestBone)
			return;
		closestBone.AddForceAtPosition(force, point, ForceMode.Impulse);
	}
}
