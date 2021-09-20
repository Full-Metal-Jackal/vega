using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MobAnimationHandler : MonoBehaviour
{
	protected Mob Mob { get; private set; }
	protected Animator Animator { get; private set; }

	protected virtual void Awake()
	{
		Mob = transform.parent.GetComponent<Mob>();
		Animator = GetComponent<Animator>();
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}
}
