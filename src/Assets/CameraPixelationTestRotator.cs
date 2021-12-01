using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPixelationTestRotator : MonoBehaviour
{
	[SerializeField]
	private float rotationTime = 3f;
	private float currentRotationTime;
	[SerializeField]
	private float degree = 45f;


	void Start()
	{
		currentRotationTime = rotationTime;
	}

	// Update is called once per frame
	void Update()
	{
		if ((currentRotationTime -= Time.deltaTime) > 0)
			return;
		currentRotationTime = rotationTime;
		transform.Rotate(new Vector3(0, degree, 0));
	}
}
