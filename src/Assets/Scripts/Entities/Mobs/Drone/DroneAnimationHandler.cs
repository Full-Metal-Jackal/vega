using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DroneAnimationHandler : MobAnimationHandler
{
	[SerializeField]
	private List<LookAtConstraint> constraints = new List<LookAtConstraint>();

	public bool AnimationsEnabled
	{
		set
		{
			foreach (LookAtConstraint constraint in constraints)
			{
				constraint.weight = value ? 1f : 0f;
				constraint.enabled = value;
			}
		}
	}
}
