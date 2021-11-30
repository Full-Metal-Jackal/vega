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
		if ((currentRotationTime -= Time.deltaTime) <= 0)
		{
			currentRotationTime = rotationTime;
			transform.Rotate(new Vector3(0, degree, 0));

			Vector3 q = Vector3.one;
			// q = Camera.main.projectionMatrix * q;
			q = Camera.main.worldToCameraMatrix * q;
			print(q);
		}
	}
}
