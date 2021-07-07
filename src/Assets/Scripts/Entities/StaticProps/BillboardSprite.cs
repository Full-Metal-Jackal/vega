using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
	// Sprites of dynamically spawned entities need to face the camera even before the first frame
	// since their incorrent rotation is quite noticable at the moment of their creation.
	private void Start() => FaceCam();

	private void LateUpdate() => FaceCam();

	private void FaceCam() =>
		transform.forward = CameraController.Instance.transform.forward;
}
