public class Workbench : Interaction
{
	public override bool CanBeUsedBy(Mob mob) => true;  // <TODO> implement player team check later.

	public override bool OnUse(Mob mob)
	{
		if (Game.circuitConstructor.Initialized)
			Game.circuitConstructor.Open();
		return true;
	}
}
