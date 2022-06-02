using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriggerSystem
{
	public class MobDeathTrigger : Trigger
	{
		[SerializeField]
		private List<Mob> mobs = new List<Mob>();

		private void Awake()
		{
			mobs.ForEach(
				(Mob mob) => mob.OnDefeated.AddListener(RegisterDeath)
			);
		}

		private void RegisterDeath(Mob mob)
		{
			mob.OnDefeated.RemoveListener(RegisterDeath);
			mobs.Remove(mob);

			if (mobs.Count <= 0)
				Activate();
		}
	}
}
