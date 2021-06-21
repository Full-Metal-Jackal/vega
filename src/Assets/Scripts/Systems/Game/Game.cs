using System.Collections.Generic;
using UnityEngine;

public static class Game
{
	/// <summary>
	/// The collection of all entites currently represented in the game.
	/// </summary>
	public readonly static List<Entity> Entities = new List<Entity>();

	/// <summary>
	/// The player controller singleton.
	/// </summary>
	public static PlayerController playerController;

	/// <summary>
	/// The camera controller singleton.
	/// </summary>
	public static CameraController cameraController;
	
	// <TODO> May be we should move UI related stuff to another static class later?
	/// <summary>
	/// The circuit constructor UI singleton.
	/// </summary>
	public static UI.CircuitConstructor.CircuitConstructor circuitConstructor;

	// <TODO> Change to UiOnly as soon as we get the main menu.
	public static InputState inputState = InputState.WorldOnly;

	public static bool Initialized { get; private set; } = false;
	public static void Initialize()
	{
		if (Initialized)
			throw new System.Exception("Multiple Game initialization attempts.");

		Initialized = true;
		Debug.Log("Game initialization complete.");
	}

	public static bool IsWorldInputAllowed => inputState == InputState.WorldOnly;
}
