using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Mob mob;

	/// <summary>
	/// How much the camera position is influenced by the cursor.
	/// </summary>
	public float cursorFactor = .33f;

	/// <summary>
	/// Smoothing applied to the camera movement.
	/// </summary>
	public float movementSmoothing = .04f;
	public float positionTolerance = .02f;
	private Vector3 currentVelocity = Vector3.zero;

	private void Awake()
	{
		if (Game.cameraController)
			throw new System.Exception($"Multiple instances of camera controller detected: {this}, {Game.cameraController}");
		Game.cameraController = this;
	}

	public void SetTrackedMob(Mob mob) => this.mob = mob;

	private void Update()
	{
		Follow();
	}

	private void Follow()
	{
		if (mob is null)
			return;

		Vector3 mobPos = mob.Body.position;
		mobPos.y = 0;

		Vector3 cursorShift = Vector3.zero;

		if (cursorFactor > .0f && Game.IsWorldInputAllowed)
		{
			Vector3 cursorPos = GetWorldCursorPosition();
			cursorShift = (cursorPos - mobPos) * cursorFactor;
		}
		Vector3 targetPos = mobPos + cursorShift;

		Vector3 currentPos = transform.position;
		if (Vector3.Distance(currentPos, targetPos) < positionTolerance)
			return;

		transform.position = Vector3.SmoothDamp(
			transform.position,
			targetPos,
			ref currentVelocity,
			movementSmoothing
		);
	}

	/// <summary>
	/// Projects cursor to the world floor plane and returns the result.
	/// </summary>
	/// <param name="heightOffset">Vertical offset of the plane the cursor is projected onto.</param>
	/// <returns>Cursor position in the world or Vector3.zero in case of error.</returns>
	public static Vector3 GetWorldCursorPosition(float heightOffset = 0)
	{
		Plane plane = new Plane(Vector3.up, heightOffset);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (plane.Raycast(ray, out float distance))
			return ray.GetPoint(distance);
		return Vector3.zero;
	}
}
