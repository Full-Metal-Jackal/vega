using UnityEngine;

public class GameController : MonoBehaviour
{
	void Start()
	{
		Debug.Log("Starting game controller.");

		Game.UpdateObjects();

		enabled = false;
	}
}
