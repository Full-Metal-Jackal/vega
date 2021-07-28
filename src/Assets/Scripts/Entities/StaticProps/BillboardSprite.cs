using UnityEngine;

[ExecuteInEditMode]
public class BillboardSprite : MonoBehaviour
{
	// Sprites of dynamically spawned entities need to face the camera even before the first frame
	// since their incorrent rotation is quite noticable at the moment of their creation.
	private void Start() => FaceCam();

	private void LateUpdate() => FaceCam();

	protected virtual void FaceCam() =>
		transform.forward = Camera.main.transform.forward;
}
