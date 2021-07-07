using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
	/// <summary>
	/// Such things as bullets need to face the camera before the first frame.
	/// </summary>
	private void Start() => FaceCam();

	private void LateUpdate() => FaceCam();

	private void FaceCam() =>
		transform.forward = CameraController.Instance.transform.forward;
}
