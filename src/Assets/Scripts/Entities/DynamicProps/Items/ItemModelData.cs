using UnityEngine;

public class ItemModelData : MonoBehaviour
{
	/// <summary>
	/// The target of the owner's left hand's IK.
	/// </summary>
	[field: SerializeField]
	public Transform LeftHandHandle { get; private set; }

	/// <summary>
	/// The target of the owner's right hand's IK.
	/// </summary>
	[field: SerializeField]
	public Transform RightHandHandle { get; private set; }

	public void Suicide() => Destroy(gameObject);
}
