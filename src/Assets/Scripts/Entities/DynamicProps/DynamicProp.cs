using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicProp : Entity
{
	public bool startFrozen = false;

	protected override bool Initialize()
	{
		if (!base.Initialize())
			return false;
		SetFrozen(startFrozen);
		return true;
	}

	protected virtual void SetFrozen(bool freeze = true)
	{
		if (freeze)
		{
			Body.velocity = Vector3.zero;
			Body.angularVelocity = Vector3.zero;
			Body.constraints = RigidbodyConstraints.FreezeAll;
			Body.isKinematic = true;
		}
		else
		{
			Body.constraints = RigidbodyConstraints.None;
			Body.isKinematic = false;
		}
	}
}
