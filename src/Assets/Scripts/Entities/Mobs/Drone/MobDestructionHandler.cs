using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MobDestructionHandler : MonoBehaviour
{
	[SerializeField]
	private Mob mob;

	/// <summary>
	/// List of game objects that have to be detached after the mob's death so that they stay on the scene.
	/// Useful for VFX, SFX etc.
	/// </summary>
	[SerializeField]
	private List<Transform> detachables = new List<Transform>();

	[SerializeField]
	private float detachablesLifeTime = 5f;

	public UnityEvent<Mob> OnDestroy;

	public void Perform()
	{
		detachables.ForEach((Transform t) => t.SetParent(Containers.Instance.Mobs));

		OnDestroy?.Invoke(mob);
		Destroy(mob.gameObject);

		detachables.ForEach((Transform t) => Destroy(t.gameObject, detachablesLifeTime));
	}
}
