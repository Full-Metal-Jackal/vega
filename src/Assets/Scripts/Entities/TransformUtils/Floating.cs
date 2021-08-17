using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
	[SerializeField]
	private float floatingRange = .1f;
	[SerializeField]
	private float floatingSpeed = 2f;

	private void Update()
	{
		float shift = floatingRange * Mathf.Cos(Time.time * floatingSpeed);
		transform.position += new Vector3(0, Time.deltaTime * shift, 0);
	}
}
