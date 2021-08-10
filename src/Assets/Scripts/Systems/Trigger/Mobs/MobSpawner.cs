using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriggerSystem
{
	/// <summary>
	/// Spawners are used to describe types of mobs to be spawned.
	/// This way we can easily create a set of prefabs to spawn randomized mobs.
	/// </summary>
	public class MobSpawner : MonoBehaviour
	{
		[field: SerializeField]
		public int Amount { get; private set; } = 1;
		public int MobsLeftToSpawn { get; private set; }

		[SerializeField]
		private List<Mob> mobPrefabs;

		private void Awake() =>
			MobsLeftToSpawn = Amount;

		public Mob Spawn()
		{
			Mob mob = Instantiate(
				Utils.Pick(mobPrefabs),
				Containers.Instance.Mobs,
				false
			);

			MobsLeftToSpawn--;
			
			Debug.Log($"Spawned {mob}");

			return mob;
		}

		public Mob Spawn(Transform spawnerPosition)
		{
			Mob mob = Spawn();

			mob.transform.position = spawnerPosition.position;
			mob.transform.rotation = spawnerPosition.rotation;

			return mob;
		}
	}
}
