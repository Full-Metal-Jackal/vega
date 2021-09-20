public class Workbench : Interaction
{
	public override bool CanBeUsedBy(Mob mob) => true;  // <TODO> implement player team check later.

	public override bool OnUse(Mob mob)
	{
		if (UI.CircuitConstructor.CircuitConstructor.Instance != null)
			UI.CircuitConstructor.CircuitConstructor.Instance.Open();
		return true;
	}
}
