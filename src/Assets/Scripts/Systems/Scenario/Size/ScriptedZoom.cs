using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedZoom : MonoBehaviour
{
	[SerializeField]
	private float lerpSpeed = 1;

	private float lerpTarget = 0;
	private bool isLerping = false;
	private readonly float lerpTolerance = .05f;

	public void SetOrthographicSize(float size) =>
		CameraController.Instance.OrthographicSize = size;
	public void SetLerpSpeed(float speed) =>
		lerpSpeed = speed;

	public void LerpToOrthographicSize(float size)
	{
		lerpTarget = size;
		isLerping = true;
	}

	private void Update()
	{
		if (isLerping)
		{
			float currentSize = CameraController.Instance.OrthographicSize;
			float difference = currentSize - lerpTarget;
			float shift = lerpSpeed * Time.deltaTime * Mathf.Sign(difference);

			CameraController.Instance.OrthographicSize = currentSize - shift;

			if (Mathf.Abs(difference) < lerpTolerance)
				isLerping = false;
		}
	}
}
