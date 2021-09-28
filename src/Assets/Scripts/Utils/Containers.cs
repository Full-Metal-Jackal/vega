using UnityEngine;

/// <summary>
/// This script contains quick references to container transforms.
/// Used for spawning things such as mobs and items on runtime while keeping the hierarchy clean.
/// </summary>
public class Containers : MonoSingleton<Containers>
{
	[field: SerializeField]
	public Transform Items { get; private set; }

	[field: SerializeField]
	public Transform Mobs { get; private set; }

	[field: SerializeField]
	public Transform Interscene { get; private set; }
}
