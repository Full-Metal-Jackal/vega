using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicProp : DynamicEntity
{
	public bool startFrozen = false;

	protected override bool Initialize()
	{
		if (!base.Initialize())
			return false;
		Frozen = startFrozen;
		return true;
	}

	public virtual bool Frozen
	{
		get => Body.constraints == RigidbodyConstraints.FreezeAll;
		set
		{
			if (value)
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
}
