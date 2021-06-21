using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBootstraper : MonoBehaviour
{
	private void Awake()
	{
		Game.Initialize();
	}
}
