using UnityEngine;

public class RotatingBillboard : BillboardSprite
{
	[SerializeField]
	private Transform referenceOrientation;

	protected override void FaceCam()
	{
		transform.forward = Camera.main.transform.forward;
		transform.rotation *= Quaternion.Euler(0, 0,
			Vector3.SignedAngle(
				Vector3.ProjectOnPlane(referenceOrientation.forward, Camera.main.transform.forward),
				Camera.main.transform.right,
				Vector3.up
			) 
		);
	}
}
