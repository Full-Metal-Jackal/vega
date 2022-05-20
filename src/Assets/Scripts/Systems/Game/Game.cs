using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
			PlayerInput.UpdateInput();
		}
	}

	private static bool __playingScene = false;
	public static bool PlayingScene
	{
		get => __playingScene;
		set
		{
			__playingScene = value;

			CameraController.Instance.InScene = __playingScene;
			UI.HUD.Hud.Instance.Toggle(!__playingScene);

			PlayerInput.UpdateInput();
		}
	}

	public static void StartLevel()
	{
		Debug.Log("The game has been started.");
		PlayerInput.UpdateInput();
	}

	/// <summary>
	/// Clears all the references contained in this object.
	/// Has to be used on level transitions.
	/// </summary>
	public static void Cleanup()
	{
		Entities.RemoveWhere((Entity entity) => !entity.Persistent);
	}
}
