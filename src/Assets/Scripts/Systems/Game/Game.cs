using System.Collections.Generic;
using UnityEngine;

using UI;
using UI.CircuitConstructor;

public static class Game
{
	/// <summary>
	/// The collection of all entites currently represented in the game.
	/// </summary>
	public readonly static HashSet<Entity> Entities = new HashSet<Entity>();

	// <TODO> Change to Paused as soon as we get the main menu.
	private static GameState state = GameState.Normal;
	public static GameState State
	{
		get => state;
		set
		{
			state = value;
			switch (state)
			{
			case GameState.Normal:
				Input.PlayerInput.WorldInputEnabled = true;
				Input.PlayerInput.UiInputEnabled = false;
				break;
			case GameState.Paused:
				Input.PlayerInput.WorldInputEnabled = false;
				Input.PlayerInput.UiInputEnabled = true;
				break;
			}
		}
	}

	public static bool Initialized { get; private set; } = false;
	public static void Initialize()
	{
		if (Initialized)
			throw new System.Exception("Multiple Game initialization attempts.");

		// Update the input state at start.
		State = state;

		PlayerController.Instance.OnPossesed += mob =>
		{
			Hud.Instance.RegisterComponents();
			CameraController.Instance.SetTrackedMob(mob);
		};

		Initialized = true;
		Debug.Log("Game initialization complete.");
	}

	// Just a shortcut for quick comparison.
	public static bool Paused => state == GameState.Paused;
}
