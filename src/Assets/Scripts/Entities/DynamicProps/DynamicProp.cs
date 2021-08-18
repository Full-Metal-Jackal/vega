﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicProp : DynamicEntity
{
	[SerializeField]
	private bool startFrozen = false;

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
				Body.isKinematic = true;
				Body.constraints = RigidbodyConstraints.FreezeAll;
			}
			else
			{
				Body.isKinematic = false;
				Body.constraints = RigidbodyConstraints.None;
			}
		}
	}
}