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

	public void Suicide() => Destroy(gameObject);
}
