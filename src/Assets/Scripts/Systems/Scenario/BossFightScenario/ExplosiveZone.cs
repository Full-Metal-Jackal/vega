using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveZone : MonoBehaviour
{
	[field: SerializeField]
	private float radius;
	[field: SerializeField]
	private float timeToLive = 3;
    private float counter;
	private SphereCollider collider;
	private List<Mob> targetsToDamage = new List<Mob>();
	[field: SerializeField]
	private Damage damage;
    private void Awake()
    {
		collider = transform.GetComponent<SphereCollider>();
		collider.radius = radius;
		counter = timeToLive;
    }

    private void Update()
    {
        if (counter > 0)
		{
			counter -= Time.deltaTime;
		}
		else
		{
			print(this + " Explode!!!");
			foreach (Mob mob in targetsToDamage)
			{
				mob.TakeDamage(damage);
			}
			Destroy(gameObject);
		}
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob))
		{
			targetsToDamage.Add(mob);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob))
		{
			targetsToDamage.Remove(mob);
		}
	}
}
