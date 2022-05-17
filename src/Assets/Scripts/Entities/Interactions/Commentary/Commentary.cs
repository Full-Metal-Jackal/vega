using UnityEngine;

[CreateAssetMenu(fileName = "CommentaryData", menuName = "Scene Data/Commentary", order = 1)]
public class Commentary : ScriptableObject
{
	[field: SerializeField]
	public MobTraits Character { get; private set; }

	[field: SerializeField]
	public string Text { get; private set; }

	[field: SerializeField]
	public float AdditiveDelay { get; private set; }
}
