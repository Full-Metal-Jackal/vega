using UnityEngine;

public class Commentary : MonoBehaviour
{
	[field: SerializeField]
	public Mob Character { get; private set; }

	[field: SerializeField]
	public string Text { get; private set; }

	[field: SerializeField]
	public float AdditiveDelay { get; private set; }
}
