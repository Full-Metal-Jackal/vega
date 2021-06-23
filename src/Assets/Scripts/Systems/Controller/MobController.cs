using UnityEngine;

public abstract class MobController : MonoSingleton<MobController>
{
	/// <summary>
	/// Tells the controller what mob should be possessed as the level starts.
	/// Needed only in specific cases to possess mobs in the editor where the controller can't be attached to the mob itself.
	/// </summary>
	public Mob possessAtStart;
	public Mob Possessed { get; protected set; }

	protected int Id { get; private set; }

	private Vector3 direction;
	private MovementState state = MovementState.Standing;

	private void Start()
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
		if (!(mob is IPossessable possessable))
			return false;
		possessable.SetPossessed(this);
		Possessed = mob;
		Debug.Log($"Controller {Id} possessed {mob}.");
		return true;
	}

	private void Update()
	{
		if (!Possessed)
			return;

		direction = GetMovement(out state);

		OnUpdate(Time.deltaTime);
	}

	protected virtual Vector3 GetMovement(out MovementState state)
	{
		state = MovementState.Standing;
		return Vector3.zero;
	}

	private void FixedUpdate() => Possessed.Move(Time.fixedDeltaTime, direction, state);

	protected virtual void OnUpdate(float delta)
	{
	}
}
