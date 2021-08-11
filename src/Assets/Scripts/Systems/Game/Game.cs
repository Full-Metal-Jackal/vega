using System.Collections.Generic;
using UnityEngine;

using UI;
using Input;

public static class Game
{
	/// <summary>
	/// The collection of all entites currently represented in the game.
	/// </summary>
	public readonly static HashSet<Entity> Entities = new HashSet<Entity>();

	public readonly static Texture2D defaultCursor = null;

	private static bool __paused = false;
	public static bool Paused
	{
		get => __paused;
		set
		{
			__paused = value;
			PlayerInput.Instance.UpdateInput();
		}
	}

	private static bool __playingScene = false;
	public static bool PlayingScene
	{
		get => __playingScene;
		set
		{
			__playingScene = value;
			PlayerInput.Instance.UpdateInput();
		}
	}

	public static bool Initialized { get; private set; } = false;
	public static void Initialize()
	{
		if (Initialized)
			throw new System.Exception("Multiple Game initialization attempts.");

		PlayerController.Instance.OnPossesed += mob =>
		{
			CameraController.Instance.SetTrackedMob(mob);
		};

		Initialized = true;
	}

	public static void Start()
	{
		if (!Initialized)
			throw new System.Exception("Attempted to start uninitialized Game instance.");

		PlayerInput.Instance.UpdateInput();

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
}
