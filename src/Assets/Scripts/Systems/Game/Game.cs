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
			Hud.Instance.Toggle(!__playingScene);

			PlayerInput.UpdateInput();
		}
	}

	public static bool Initialized { get; private set; } = false;
	public static void Initialize()
	{
		if (Initialized)
			throw new System.Exception("Multiple Game initialization attempts.");

		Initialized = true;
		PlayerInput.Initialize();
	}

	public static void LoadScene()
	{
		if (!Initialized)
			throw new System.Exception("Attempted to start uninitialized Game instance.");

		PlayerInput.UpdateInput();

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

	public static void ChangeLevel(string sceneName)
	{
		Cleanup();

		var loading = SceneManager.LoadSceneAsync(sceneName);
		// loading.allowSceneActivation = false;

		//loaderCanvas.SetActive(true);
		//do
		//{
		//	await Task.Delay(100);  //only for test purpose
		//	progressBar.value = loading.progress;

		//} while (loading.progress < 0.9f);

		loading.allowSceneActivation = true;

		// loaderCanvas.SetActive(false);
	}
}
