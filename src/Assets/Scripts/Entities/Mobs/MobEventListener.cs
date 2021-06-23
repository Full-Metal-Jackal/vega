using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEventListener : MonoBehaviour
{
	private Mob mob; // Might be used later.

	private void Awake()
	{
		Initialize();
	}

	protected virtual void Initialize()
	{
		mob = transform.parent.GetComponent<Mob>();
	}
}
