using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
	Mob player;
	private void Start()
	{
		player = GameObject.Find("Entity.Dynamic.Mob").GetComponent<Mob>();
	}

	public void EventRoll()
	{
		player.DodgeRollOff();
	}
}
