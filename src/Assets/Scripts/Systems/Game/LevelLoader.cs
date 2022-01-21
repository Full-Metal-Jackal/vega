using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoSingleton<LevelLoader>
{
	public static readonly float neededProgress = .9f;
	
	[SerializeField]
	private List<GameObject> instantiateAtStart;

	[SerializeField]
	private Mob possessAtStart;

	protected override void Awake()
	{
		if (!Game.Initialized)
			Game.Initialize();

		foreach (GameObject prefab in instantiateAtStart)
			Instantiate(prefab, Containers.Instance.Interscene);

		PlayerController.Instance.possessAtStart = possessAtStart;
	}

	private void Start() =>
		Game.StartLevel();

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
	}
}
