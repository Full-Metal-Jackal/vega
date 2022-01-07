using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveZone : MonoBehaviour
{
	private float timeToLive = 3;
    private float counter;
    private void Awake()
    {
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
			Destroy(gameObject);
		}
    }
}
