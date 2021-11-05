using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
	[SerializeField]
	private float floatingRange = .1f;
	
	[SerializeField]
	private float floatingSpeed = 2f;

	[SerializeField]
	private Vector3 direction = Vector3.up;

	private void Awake()
	{
		direction.Normalize();
	}

	private void Update()
	{
		float shift = floatingRange * Mathf.Cos(Time.time * floatingSpeed);
		transform.position += direction * Time.deltaTime * shift;
	}
}
