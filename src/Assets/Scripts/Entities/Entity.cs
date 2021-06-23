﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	public string Name = "unnamed entity";
	public bool Initialized { get; protected set; } = false;

	protected Outline outerOutline;
	public Outline OuterOutline
	{
		get => outerOutline;
		private set
		{
			outerOutline = value;
		}
	}

	void Awake()
	{
		enabled = false;
		if (Initialize())
			Game.Entities.Add(this);
	}

	private void Update()
	{
		float delta = Time.deltaTime;
		Tick(delta);
	}

	protected virtual bool Initialize()
	{
		if (Initialized)
		{
			Debug.LogWarning($"Multiple initialization attempts of {this}!");
			return false;
		}

		TryGetComponent(out outerOutline);

		return Initialized = true;
	}

	public virtual bool Spawn(Vector3 pos)
	{
		if (!(Initialized || Initialize()))
			return false;

		transform.position = pos;
		return true;
	}

	protected virtual void Tick(float delta)
	{
	}

	public override string ToString() => Name;
}
