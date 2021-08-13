using System.Collections.Generic;
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
	private bool followingCursor = true;

	/// <summary>
	/// Smoothing applied to the camera movement in game.
	/// </summary>
	[SerializeField]
	private float inGameSmoothing = .04f;
	/// <summary>
	/// Smoothing applied to the camera movement during scenes.
	/// </summary>
	[SerializeField]
	private float sceneSmoothing = .5f;
	/// <summary>
	/// Current smoothing.
	/// </summary>
	private float followSmoothing = .1f;

	private bool __inScene = false;
	public bool InScene
	{
		get => __inScene;
		set
		{
			__inScene = value;
			followSmoothing = __inScene ? sceneSmoothing : inGameSmoothing;
		}
	}

	[SerializeField]
	private float positionTolerance = .02f;

	[SerializeField]
	private float defaultDistance = 20f;
	[SerializeField]
	private float defaultFOV = 30f;

	/// <summary>
	/// The camera assigned to this controller.
	/// </summary>
	public Camera Camera => Camera.main;

	public float FOV
	{
		get => Camera.fieldOfView;
		set => Camera.fieldOfView = value;
	}

	public float Distance
	{
		get => Vector3.Distance(Camera.transform.position, transform.position);
		set
		{
			if (value <= 0)
				throw new System.Exception("Camera distance should be greater than zero!");

			Camera.transform.position = transform.position + (Camera.transform.position - transform.position).normalized * value;
		}
	}

	private Vector3 currentVelocity = Vector3.zero;

	public void SetTrackedMob(Mob mob)
	{
		if (this.mob)
			RemovePOI(this.mob.gameObject);

		this.mob = mob;
		AddPOI(mob.transform);
		followingCursor = true;
	}

	// Since the camera is not rotating in game, it would be better
	// to calculate this rotation only once instead of recalculating it every time it's needed.
	public Quaternion VerticalRotation { get; private set; }

	private readonly Dictionary<Transform, float> points = new Dictionary<Transform, float>();

	private void Start()
	{
		RecalculateRotation();

		FOV = defaultFOV;
		Distance = defaultDistance;
	}

	private void RecalculateRotation()
	{
		Vector3 horForward = Camera.transform.forward;
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

		Vector3 cursorPos = Vector3.zero;
		bool cursorActive = followingCursor && !Game.Paused;
		if (cursorActive)
		{
			totalPoints++;
			cursorPos = GetWorldCursorPos();
		}

		Vector3 center = cursorPos;
		foreach (Transform pointTransform in points.Keys)
			center += pointTransform.position;
		center /= totalPoints;

		Vector3 shift = cursorActive ? (cursorPos - center) * cursorWeight : Vector3.zero;
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
			followSmoothing
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
		Ray ray = Instance.Camera.ScreenPointToRay(Mouse.current.position.ReadValue());
		if (plane.Raycast(ray, out float distance))
			return ray.GetPoint(distance);
		return Vector3.zero;
	}

	public void SetOnlyPOI(GameObject poi) => SetOnlyPOI(poi.transform);
	public void SetOnlyPOI(Transform poi)
	{
		points.Clear();
		followingCursor = false;
		AddPOI(poi);
	}
	public void ResetPOI() => SetTrackedMob(mob);

	// There are separate methods because unity won't serialize 2 arguments by default.
	public void AddPOI(GameObject poi) => AddPOI(poi.transform, 1f);
	public void AddPOI(GameObject poi, float weight) => AddPOI(poi.transform, weight);
	public void AddPOI(Transform poi, float weight = 1f) => points.Add(poi, weight);

	public bool RemovePOI(GameObject poi) => RemovePOI(poi.transform);
	public bool RemovePOI(Transform poi) => points.Remove(poi);
}
