using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
	[SerializeField]
	private Light flashingLight;

	[SerializeField]
	private float flashingSpeed = 10f;

	[SerializeField]
	private float maxIntensity = 1f;

	private void Update()
	{
		if (!flashingLight)
			return;

		flashingLight.intensity = Mathf.Sin(Time.time * flashingSpeed) * maxIntensity;
	}
}
