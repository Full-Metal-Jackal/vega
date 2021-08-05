public enum GameState
{
	/// <summary>
	/// Used to interact with the world, UI elements shouldn't be interacted in general.
	/// </summary>
	Normal,

	/// <summary>
	/// Allows to interact with UI components only, locks all the entities' movement.
	/// </summary>
	Paused
}
