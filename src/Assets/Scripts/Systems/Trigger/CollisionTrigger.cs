﻿using UnityEngine;

namespace TriggerSystem
{
	[RequireComponent(typeof(Collider))]
	public class CollisionTrigger : Trigger
	{
		[SerializeField]
		private bool requiresPlayer = true;

		protected virtual void OnTriggerEnter(Collider other)
		{
			if (requiresPlayer
				&& !(other.transform.parent.TryGetComponent(out Mob mob) && mob.IsPlayer)
			)
				return;

			Activate();
		}
	}
}
