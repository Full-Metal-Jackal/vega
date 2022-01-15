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
	private float minIntensity = 0f;
	[SerializeField]
	private float maxIntensity = 1f;
	private float intensityRange = 1f;

	private void Awake()
	{
		intensityRange = maxIntensity - minIntensity;
	}

	private void Update()
	{
		if (!flashingLight)
			return;

		flashingLight.intensity = minIntensity + Mathf.Sin(Time.time * flashingSpeed) * intensityRange;
	}
}
