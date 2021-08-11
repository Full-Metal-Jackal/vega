using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	public bool Initialized { get; private set; } = false;

	[field: SerializeField]
	public virtual string Name { get; set; } = "unnamed entity";

	[field: SerializeField]
	public bool CanHideWalls { get; set; } = false;

	public Outline Outline { get; private set; }

	public IEnumerable<Collider> Colliders => GetComponentsInChildren<Collider>();

	/// <summary>
	/// If this entity should be transfered between levels.
	/// </summary>
	public bool Persistent { get; protected set; }

	private void Awake()
	{
		if (enabled = Initialize())
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

		TryGetComponent(out Outline outline);
		Outline = outline;

		return Initialized = true;
	}

	public void Start() =>
		Setup();

	public virtual void Setup()
	{
	}

	protected virtual void Tick(float delta)
	{
	}

	public override string ToString() => Name;
}
