/// <summary>
/// Represents everying that can be picked up.
/// </summary>
public abstract class Item : Interaction
{
	public override bool OnUse(Mob mob)
	{
		if (OnPickedUp())
		{
			Suicide();
			return true;
		}

		return false;
	}

	protected virtual bool OnPickedUp() => true;

	protected void Suicide() =>
		Destroy(gameObject);
}
