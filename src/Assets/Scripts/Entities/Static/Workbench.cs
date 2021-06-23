public class Workbench : StaticEntity, IInteractable
{
	public bool Selectable { get; set; } = true;

	public bool CanBeUsedBy(Mob mob) => true;  // <TODO> implement player team check later.

	public bool OnUse(Mob mob)
	{
		if (Game.CircuitConstructor != null)
			Game.CircuitConstructor.Open();
		return true;
	}
}
