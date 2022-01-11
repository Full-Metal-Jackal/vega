using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveZone : MonoBehaviour
{
	[field: SerializeField]
	private float radius;
	[field: SerializeField]
	private float timeToLive = 3;
	[field: SerializeField]
	private Damage damage;
	private float counter;
	private List<Mob> targetsToDamage = new List<Mob>();

    private void Awake()
    {
		transform.GetComponent<SphereCollider>().radius = radius;
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
			Debug.Log(this + " Explode!!!");
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
