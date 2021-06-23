public class Workbench : Interaction
{
	public override bool CanBeUsedBy(Mob mob) => true;  // <TODO> implement player team check later.

	public override bool OnUse(Mob mob)
	{
		if (Game.CircuitConstructor != null)
			Game.CircuitConstructor.Open();
		return true;
	}
}
