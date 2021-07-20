using UnityEngine;

public class RotatingBillboard : BillboardSprite
{
	[SerializeField]
	private Transform referenceOrientation;

	protected override void FaceCam()
	{
		transform.forward = Camera.main.transform.forward;

		float angle = Vector3.SignedAngle(referenceOrientation.forward, Vector3.forward, Vector3.up);

		Debug.Log($"angle: {angle}");

		transform.rotation *= Quaternion.AngleAxis(
			angle,
			Camera.main.transform.forward
		);
	}
}
