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

	protected virtual void Awake()
	{
		Outline = GetComponent<Outline>();
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	public override string ToString() => Name;
}
