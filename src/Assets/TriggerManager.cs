using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : Room
{
	Transform room;
	private void Start()
	{
		room = transform.parent;
	}
	private void OnTriggerEnter(Collider other)
	{
		print("Enter: " + other.gameObject.name + " in " + room.name);
		print("Contains: ");
	}

	private void OnTriggerExit(Collider other)
	{
		print("Exit: " + other.gameObject.name);
	}
}
