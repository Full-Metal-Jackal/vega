using System.Collections.Generic;
using UnityEngine;

using UI;
using UI.CircuitConstructor;

public static class Game
{
	/// <summary>
	/// The collection of all entites currently represented in the game.
	/// </summary>
	public readonly static List<Entity> Entities = new List<Entity>();

	// <TODO> Change to UiOnly as soon as we get the main menu.
	public static InputState inputState = InputState.WorldOnly;

	public static bool Initialized { get; private set; } = false;
	public static void Initialize()
	{
		if (Initialized)
			throw new System.Exception("Multiple Game initialization attempts.");

		PlayerController.Instance.OnPossesed += () => Hud.Instance.RegisterComponents();

		Initialized = true;
		Debug.Log("Game initialization complete.");
	}

	public static bool IsWorldInputAllowed => inputState == InputState.WorldOnly;
}
