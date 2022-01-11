using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
	private GameObject target;
	private Vector3 spawnPosition;
	private float range = 20f;
	[field: SerializeField]
	public GameObject objectToSpawn;

	public void Spawn()
	{
		bool notSpawned = true;
		while (notSpawned)
		{
			if (RandomPoint(transform.position, range, out spawnPosition))
			{
				Instantiate(objectToSpawn, spawnPosition, transform.rotation);
				notSpawned = false;
			}
		}
	}

	public void SpawnOn(Transform parent)
	{
		Instantiate(objectToSpawn, parent.position, parent.rotation);
	}

	private bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		Vector3 randomPoint = center + Random.insideUnitSphere * range;
		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
		{
			result = hit.position;
			return true;
		}
		result = Vector3.zero;
		return false;
	}
}
