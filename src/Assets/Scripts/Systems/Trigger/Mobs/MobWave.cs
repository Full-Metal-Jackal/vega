using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TriggerSystem
{
	public class MobWave : Trigger
	{
		private bool activated = false;
		
		private readonly HashSet<Mob> trackedMobs = new HashSet<Mob>();

		[SerializeField]
		private float spawnInterval = 0;
		private float lastSpawnTime = 0;

		[SerializeField]
		private bool randomSpawnPoints = false;
		private int spawnPointIndex = 0;
		[SerializeField]
		private List<Transform> spawnPoints = new List<Transform>();

		[SerializeField]
		private bool randomSpawners = false;
		// Since Unity cannot serialize dictionaries, we combine spawners into the MobSpawner script.
		[SerializeField]
		private List<MobSpawner> toSpawn = new List<MobSpawner>();
		private int spawnerIndex = -1;

		public void BeginSpawning()
		{
			if (spawnInterval > 0)
			{
				activated = true;
				return;
			}

			foreach (MobSpawner spawner in toSpawn)
				while (spawner.MobsLeftToSpawn > 0)
					CreateMob(spawner);
		}

		public void CreateMob(MobSpawner spawner) =>
			AddTrackedMob(spawner.Spawn(GetSpawnPoint()));

		public void AddTrackedMob(Mob mob)
		{
			mob.OnDefeated.AddListener(OnMobDefeated);

			trackedMobs.Add(mob);
		}

		public void OnMobDefeated(Mob mob)
		{
			mob.OnDefeated.RemoveListener(OnMobDefeated);

			trackedMobs.Remove(mob);

			if (!trackedMobs.Any())
				Activate();
		}

		public Transform GetSpawnPoint()
		{
			if (randomSpawnPoints)
				return Utils.Pick(spawnPoints);

			Transform point = spawnPoints[spawnPointIndex];
			spawnPointIndex = ++spawnPointIndex % spawnPoints.Count;
			return point;
		}

		public void Update()
		{
			if (!activated)
				return;

			if (lastSpawnTime + spawnInterval > Time.time)
				return;

			List<MobSpawner> notEmptySpawners = toSpawn.Where((MobSpawner s) => s.MobsLeftToSpawn > 0).ToList();
			if (!notEmptySpawners.Any())
			{
				activated = false;
				return;
			}

			MobSpawner spawner;
			if (randomSpawners)
			{
				spawner = Utils.Pick(notEmptySpawners);
			}
			else
			{
				spawnerIndex = ++spawnerIndex % notEmptySpawners.Count;
				spawner = notEmptySpawners[spawnerIndex];
			}

			lastSpawnTime = Time.time;
			CreateMob(spawner);
		}
	}
}
