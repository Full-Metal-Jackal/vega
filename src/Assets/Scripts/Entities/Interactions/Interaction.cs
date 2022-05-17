using UnityEngine;

[RequireComponent(typeof(Entity))]
public abstract class Interaction : MonoBehaviour
{
	public bool Initialized { get; private set; } = false;

	public Entity Entity { get; private set; }

	[SerializeField]
	private EntityOutlineFX outline;

	public bool OutlineEnabled
	{
		set
		{
			if (outline != null)
				outline.enabled = value;
		}
	}

	public virtual bool Selectable => enabled;

	protected virtual void Awake()
	{
		if (Initialized)
			throw new System.Exception($"Multiple initialization attempts of {this}!");
		
		if (!Entity)
			Entity = GetComponent<Entity>();

		Initialized = true;
	}

	/// <summary>
	/// Called when the mob uses the object.
	/// </summary>
	/// <param name="mob">The user of the object.</param>
	/// <returns>true if the object was used successfully, false otherwise.</returns>
	public abstract bool OnUse(Mob mob);

	/// <summary>
	/// Checks if the mob can use this object.
	/// </summary>
	/// <param name="mob">The mob to check the usage for.</param>
	/// <returns>true if the object can be used by the mob, false otherwise.</returns>
	public virtual bool CanBeUsedBy(Mob mob) => true;

	private void Update()
	{
		Tick(Time.deltaTime);
	}

	protected virtual void Tick(float delta)
	{
	}
}
