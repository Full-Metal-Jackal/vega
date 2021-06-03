using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
	public string Name = "unnamed entity";
	public Rigidbody Body { get; protected set; }
	public bool Initialized { get; private set; } = false;

	protected Outline outerOutline;
	public Outline OuterOutline
	{
		get => outerOutline;
		private set
		{
			outerOutline = value;
		}
	}

	void Start()
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

	private void FixedUpdate()
	{
		MovePhysics(Time.fixedDeltaTime);
	}

	protected virtual bool Initialize()
	{
		if (Initialized)
		{
			Debug.LogWarning($"Multiple initialization attempts of {this}!");
			return false;
		}

		Body = GetComponent<Rigidbody>();
		TryGetComponent(out outerOutline);

		Initialized = true;
		enabled = true;
		return true;
	}

	public virtual bool Spawn(Vector3 pos)
	{
		if (!(Initialized || Initialize()))
			return false;

		Body.MovePosition(pos);
		return true;
	}

	protected virtual void Tick(float delta)
	{
	}

	/// <summary>
	/// Handles the entity's movement.
	/// </summary>
	/// <param name="delta">delta between two ticks</param>
	public virtual void MovePhysics(float delta)
	{
	}

	public override string ToString() => Name;
}
