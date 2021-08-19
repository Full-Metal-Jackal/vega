using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoSingleton<LevelLoader>
{
	public static readonly float neededProgress = .9f;

	protected override void Awake()
	{
		if (!Game.Initialized)
			Game.Initialize();
	}

	private void Start() => Game.StartLevel();

	public void ChangeLevel(string sceneName)
	{
		Game.Cleanup();
		StartCoroutine(LoadLevel(sceneName));
	}

	private IEnumerator LoadLevel(string sceneName)
	{
		AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
		loading.allowSceneActivation = false;

		UI.Loading.LoadingScreen.Instance.Open(loading);

		while (!loading.isDone)
			yield return null;

		UI.Loading.LoadingScreen.Instance.Close();
	}

	// <TODO> Must be relocated somewhere else
	/// <summary>
	/// Since Unity cannot handle properties, there's a setter for this one.
	/// Must not be used anywhere in code.
	/// </summary>
	/// <param name="isPlaying">New state of PlayingScene.</param>
	public void SetPlayingScene(bool isPlaying) =>
		Game.PlayingScene = isPlaying;
}
