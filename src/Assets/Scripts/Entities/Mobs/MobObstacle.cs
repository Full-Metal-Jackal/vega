using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MobObstacle : MonoBehaviour
{
	private Collider stoppingCollider;
	private Collider mobCollider;

	private void Awake()
	{
		stoppingCollider = GetComponent<Collider>();

		mobCollider = transform.parent.GetComponent<Collider>();
	}

	public virtual void Start()
	{
		Physics.IgnoreCollision(stoppingCollider, mobCollider, true);
	}
}
