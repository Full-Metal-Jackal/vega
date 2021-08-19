using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

	[SerializeField]
	private GameObject loaderCanvas;
	[SerializeField]
	private Slider progressBar;
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public async void LoadScene(string sceneName)
	{
		var scene = SceneManager.LoadSceneAsync(sceneName);
		scene.allowSceneActivation = false;

		loaderCanvas.SetActive(true);
		do
		{
			await Task.Delay(100);  //only for test purpose
			progressBar.value = scene.progress;

		} while (scene.progress < 0.9f);

		scene.allowSceneActivation = true;

		loaderCanvas.SetActive(false);
	}

}
