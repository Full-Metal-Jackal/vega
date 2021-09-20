using System;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Data/MobTrait"), Serializable]
public class MobTraits : ScriptableObject
{
	[field: SerializeField]
	public Sprite Avatar { get; private set; }
	
	[field: SerializeField]
	public AudioClip TypingSound { get; private set; }

	[field: SerializeField]
	public string Name { get; private set; } = "Mob Name";

	/// <summary>
	/// Used to calculate the delay between the appearence of characters in a dialogue.
	/// Delay is calculated as follows: `delay = 1 / (typingSpeed * 10)`, in seconds.
	/// </summary>
	[field: SerializeField, Min(0.001f)]
	public float TypingSpeed { get; private set; } = 1.0f;
}
