using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MobSfxHandler : MonoBehaviour
{
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip hitSound;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();

		Mob mob = transform.parent.GetComponentInParent<Mob>();
		if (!mob)
		{
			Debug.LogError($"No mob found for {this}. Mob's SFX should be two levels deeper than the mob itself.");
			return;
		}

		if (hitSound)
			mob.OnDamaged += (Mob mob) => audioSource.PlayOneShot(hitSound);
	}
}
