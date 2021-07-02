using UnityEngine;

public class ItemModelData : MonoBehaviour
{
	/// <summary>
	/// The target of the owner's left hand's IK.
	/// </summary>
	[field: SerializeField]
	public Transform LeftHandGrip { get; private set; }

	/// <summary>
	/// The target of the owner's right hand's IK.
	/// </summary>
	[field: SerializeField]
	public Transform RightHandGrip { get; private set; }

	/// <summary>
	/// The parenting origin that should match human's rig socket.
	/// </summary>
	[field: SerializeField]
	public Transform Origin { get; private set; }

	public void Suicide() => Destroy(gameObject);
}
