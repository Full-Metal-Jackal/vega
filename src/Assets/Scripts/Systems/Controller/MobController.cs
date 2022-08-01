using UnityEngine;

public abstract class MobController : MonoBehaviour
{
	/// <summary>
	/// Tells the controller what mob should be possessed as the level starts.
	/// Needed only in specific cases to possess mobs in the editor where the controller can't be attached to the mob itself.
	/// </summary>
	public Mob possessAtStart;
	public Mob Possessed { get; protected set; }

	protected static int TotalControllers { get; private set; } = 0;
	public int Id { get; private set; }

	private Vector3 movement;

	protected virtual void Awake()
	{
		Id = TotalControllers++;
	}

	protected virtual void Start()
	{
		if ((possessAtStart is Mob mob) || TryGetComponent(out mob))
			PossessMob(mob);
	}

	/// <summary>
	/// Assumes control of the mob.
	/// </summary>
	/// <param name="mob">The mob to take control of.</param>
	public virtual void PossessMob(Mob mob)
	{
		mob.SetPossessed(this);
		Possessed = mob;
		Debug.Log($"Controller {Id} possessed {mob}.");
	}

	protected virtual void Update()
	{
	}

	protected virtual void InputUse()
	{
	}

	protected virtual void InputTrigger(bool held) => Possessed.UseItem(held);
	protected virtual void InputReload() => Possessed.Reload();
	protected virtual void InputThrow() => Possessed.Throw();
	protected virtual void InputDodge() => Possessed.DashAction();
	protected virtual void InputDrop() => Possessed.DropItem();

	protected virtual void InputMove(Vector3 inputMovement) =>
		movement = inputMovement;

	protected virtual void InputSpecialAbility()
	{
		if (Possessed.TryGetComponent(out SpecialAbility ability))
			ability.Activate();
	}

	private void FixedUpdate()
	{
		if (Possessed)
			Possessed.Move(Time.fixedDeltaTime, movement);
	}

	public override string ToString()
	{
		string result = $"mob controller #{Id}";

		if (Possessed)
			result += $" ({Possessed})";

		return result;
	}
}
