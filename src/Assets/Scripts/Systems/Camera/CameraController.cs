using System.Collections.Generic;
using Scenario;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoSingleton<CameraController>
{
	private Mob mob;

	/// <summary>
	/// How much the camera position is influenced by the cursor.
	/// 1 is default weight.
	/// </summary>
	[SerializeField]
	private float cursorWeight = .33f;

	/// <summary>
	/// Smoothing applied to the camera movement.
	/// </summary>
	[SerializeField]
	private float movementSmoothing = .04f;

	[SerializeField]
	private float positionTolerance = .02f;
	
	private Vector3 currentVelocity = Vector3.zero;

	public void SetTrackedMob(Mob mob)
	{
		if (this.mob)
			RemovePOI(this.mob);

		this.mob = mob;
		AddPOI(mob.transform);
	}

	// Since the camera is not rotating in game, it would be better
	// to calculate this rotation only once instead of recalculating it every time it's needed.
	public Quaternion VerticalRotation { get; private set; }

	private readonly Dictionary<Transform, float> points = new Dictionary<Transform, float>();

	private void Start() =>
		RecalculateRotation();

	private void RecalculateRotation()
	{
		Vector3 horForward = Camera.main.transform.forward;
		horForward.y = 0;
		VerticalRotation = Quaternion.AngleAxis(
			Vector3.SignedAngle(Vector3.forward, horForward, Vector3.up),
			Vector3.up
		);
	}

	private void Update() =>
		Follow();

	private void Follow()
	{
		int totalPoints = points.Count;

		bool cursorActive = cursorWeight > .0f && !Game.Paused;
		Vector3 cursorPos = Vector3.zero;
		if (cursorActive)
		{
			totalPoints++;
			cursorPos = GetWorldCursorPos();
		}

		Vector3 center = cursorPos;
		foreach (Transform pointTransform in points.Keys)
			center += pointTransform.position;
		center /= totalPoints;

		Vector3 shift = (cursorPos - center) * cursorWeight;
		foreach (KeyValuePair<Transform, float> poi in points)
			shift += (poi.Key.position - center) * poi.Value;
		shift /= totalPoints;

		Vector3 target = center + shift;

		if (Vector3.Distance(target, transform.position) < positionTolerance)
			return;

		transform.position = Vector3.SmoothDamp(
			transform.position,
			target,
			ref currentVelocity,
			movementSmoothing
		);
	}

	private void OlFollow()
	{
		int totalPoints = points.Count;

		bool cursorActive = cursorWeight > .0f && !Game.Paused;
		Vector3 cursorPos = Vector3.zero;
		if (cursorActive)
		{
			totalPoints++;
			cursorPos = GetWorldCursorPos();
		}

		Vector3 center = cursorPos;
		foreach (Transform pointTransform in points.Keys)
			center += pointTransform.position;
		center /= totalPoints;

		Vector3 shift = (cursorPos - center) * cursorWeight;
		foreach (KeyValuePair<Transform, float> poi in points)
			shift += (poi.Key.position - center) * poi.Value;
		shift /= totalPoints;

		Vector3 target = center + shift;

		if (Vector3.Distance(target, transform.position) < positionTolerance)
		{
			transform.position = target;
			return;
		}

		transform.position = Vector3.SmoothDamp(
			transform.position,
			target,
			ref currentVelocity,
			movementSmoothing
		);
	}

	/// <summary>
	/// Projects cursor to the world floor plane and returns the result.
	/// </summary>
	/// <param name="heightOffset">Vertical offset of the plane the cursor is projected onto.</param>
	/// <returns>Cursor position in the world or Vector3.zero in case of error.</returns>
	public static Vector3 GetWorldCursorPos(float heightOffset = 0)
	{
		Plane plane = new Plane(Vector3.up, heightOffset);
		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
		if (plane.Raycast(ray, out float distance))
			return ray.GetPoint(distance);
		return Vector3.zero;
	}


	public void AddPOI(MonoBehaviour poi, float weight = 1f) => points.Add(poi.transform, weight);
	public void AddPOI(Transform poi, float weight = 1f) => points.Add(poi, weight);
	public bool RemovePOI(MonoBehaviour poi) => points.Remove(poi.transform);
	public bool RemovePOI(Transform poi) => points.Remove(poi);
}
