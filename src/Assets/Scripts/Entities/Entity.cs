using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	[field: SerializeField]
	public virtual string Name { get; set; } = "unnamed entity";
	
	public bool Initialized { get; private set; } = false;

	protected Outline outline;
	public Outline Outline
	{
		get => outline;
		private set
		{
			outline = value;
		}
	}

	void Awake()
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

		TryGetComponent(out outline);

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
