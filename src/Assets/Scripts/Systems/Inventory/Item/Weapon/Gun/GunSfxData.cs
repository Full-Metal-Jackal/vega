using UnityEngine;

namespace Inventory
{
	public class GunSfxData : ItemSfxData
	{
		[field: SerializeField]
		public AudioClip DryFire { get; private set; }

		[field: SerializeField]
		public AudioClip Reload { get; private set; }
	}
}
