using UnityEngine;

public abstract class MobController : MonoBehaviour
{
	/// <summary>
	/// Tells the controller what mob should be possessed as the level starts.
	/// Needed only in specific cases to possess mobs in the editor where the controller can't be attached to the mob itself.
	/// </summary>
	public Mob possessAtStart;
	public Mob Possessed { get; protected set; }

	protected int Id { get; private set; }

	protected Vector3 movement;

	public bool Initialized { get; private set; }

	private void Awake() => Initialize();

	protected virtual void Initialize()
	{
		if (Initialized)
			throw new System.Exception($"Multiple initialization attempts of {this}.");

		Initialized = true;
	}

	private void Start() => Setup();

	protected virtual void Setup()
	{
		if ((possessAtStart is Mob mob) || TryGetComponent(out mob))
			PossessMob(mob);
	}

	/// <summary>
	/// Assumes control of the mob.
	/// </summary>
	/// <param name="mob">The mob to take control of.</param>
	/// <returns>true in case of successful possession, false otherwise.</returns>
	public virtual bool PossessMob(Mob mob)
	{
		mob.SetPossessed(this);
		Possessed = mob;
		Possessed.tag = "Player";
		Debug.Log($"Controller {Id} possessed {mob}.");
		return true;
	}

	private void Update()
	{
		if (!Possessed)
			return;

		OnUpdate(Time.deltaTime);
	}

	protected virtual void OnUpdate(float delta)
	{
	}

	protected virtual Vector3 UpdateMovementInput() => Vector3.zero;

	private void FixedUpdate()
	{
		if (Possessed)
			Possessed.Move(Time.fixedDeltaTime, movement);
	}
}
