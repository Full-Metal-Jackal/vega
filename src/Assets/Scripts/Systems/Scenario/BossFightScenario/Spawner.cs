using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	private GameObject target;
	private Vector3 spawnPosition;

	[field: SerializeField]
	public GameObject objectToSpawn;

	public void Start()
	{
		//print(spawnableGameObjects);
	}


	public void Spawn()
	{
		Instantiate(objectToSpawn, transform);
	}

	public void SpawnOn(Transform parent)
	{
		Instantiate(objectToSpawn, parent.position, parent.rotation);
	}
}
