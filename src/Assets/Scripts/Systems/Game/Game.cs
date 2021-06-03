using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
	/// <summary>
	/// The collection of all entites currently represented in the game.
	/// </summary>
	public readonly static List<Entity> Entities = new List<Entity>();
	/// <summary>
	/// The game's player controller.
	/// </summary>
	public static PlayerController playerController;
	/// <summary>
	/// The game's camera controller.
	/// </summary>
	public static CameraController cameraController;
	public static bool Initialized { get; private set; } = false;
	public static void Initialize()
	{
		if (Initialized)
			throw new System.Exception("Multiple Game initialization attempts.");

		Initialized = true;
		Debug.Log("Game initialization complete.");
	}
}
