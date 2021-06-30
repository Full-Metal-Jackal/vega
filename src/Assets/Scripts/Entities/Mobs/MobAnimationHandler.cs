using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MobAnimationHandler : MonoBehaviour
{
	protected Mob Mob { get; private set; }
	protected Animator Animator { get; private set; }

	private void Awake() => Initialize();

	protected virtual void Initialize()
	{
		Mob = transform.parent.GetComponent<Mob>();
		Animator = GetComponent<Animator>();
	}

	private void Start() => Setup();

	protected virtual void Setup()
	{
	}
}
