public class Workbench : StaticEntity, IInteractable
{
	public bool Selectable { get; set; } = true;

	public bool CanBeUsedBy(Mob mob) => true;  // <TODO> implement player team check later.

	public bool OnUse(Mob mob)
	{
		Game.circuitConstructor.Open();
		return true;
	}
}
