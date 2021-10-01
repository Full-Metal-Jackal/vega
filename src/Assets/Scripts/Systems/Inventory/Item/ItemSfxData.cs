using UnityEngine;

namespace Inventory
{
	[RequireComponent(typeof(AudioSource))]
	public class ItemSfxData : MonoBehaviour
	{
		[field: SerializeField]
		public AudioClip Equip { get; private set; }

		[field: SerializeField]
		public AudioClip Fire { get; private set; }

		private AudioSource audioSource;

		private void Awake() =>
			audioSource = GetComponent<AudioSource>();

		public void Play(AudioClip clip)
		{
			if (clip)
				audioSource.PlayOneShot(clip);
		}
	}
}
