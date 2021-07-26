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

	public readonly static Texture2D defaultCursor = null;

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

		PlayerController.Instance.OnPossesed += mob =>
		{
			Hud.Instance.RegisterComponents();
			CameraController.Instance.SetTrackedMob(mob);
		};

		Initialized = true;
	}

	public static void Start()
	{
		if (!Initialized)
			throw new System.Exception("Attempted to start uninitialized Game instance.");

		// Update the input state at start.
		State = state;

		Debug.Log("The Game has been started.");
	}

	/// <summary>
	/// Clears all the references contained in this object.
	/// Has to be used on level transitions.
	/// </summary>
	private static void Cleanup()
	{
		Entities.RemoveWhere((Entity entity) => !entity.Persistent);
	}

	// Just a shortcut for quick comparison.
	public static bool Paused => state == GameState.Paused;
}
