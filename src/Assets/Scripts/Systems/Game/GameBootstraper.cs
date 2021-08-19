using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBootstraper : MonoBehaviour
{
	private void Awake()
	{
		if (!Game.Initialized)
			Game.Initialize();
		// Game.LoadScene();
	}

	private void Start() =>
		Game.Start();

	/// <summary>
	/// Since Unity cannot handle properties, there's a setter for this one.
	/// Must not be used anywhere in code.
	/// </summary>
	/// <param name="isPlaying">New state of PlayingScene.</param>
	public void SetPlayingScene(bool isPlaying) =>
		Game.PlayingScene = isPlaying;
}
