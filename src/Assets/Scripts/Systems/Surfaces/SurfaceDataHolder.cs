using UnityEngine;

/// <summary>
/// Should be added as a component to a GameObject that has a Collider component
/// </summary>
public class SurfaceDataHolder : MonoBehaviour
{
	[field: SerializeField]
	public SurfaceData SurfaceData { get; private set; }
}
