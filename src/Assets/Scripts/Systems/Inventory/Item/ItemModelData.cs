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
	/// The transform local offsets of which are considered while parenting the model to the item socket.
	/// Usually matches RightHandGrip.
	/// </summary>
	[field: SerializeField]
	public Transform ParentingOrigin { get; private set; }

	public void Suicide() => Destroy(gameObject);

	private void OnDrawGizmos()
	{
		if (LeftHandGrip)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(LeftHandGrip.position, .02f);
			Gizmos.DrawRay(LeftHandGrip.position, LeftHandGrip.forward * .06f);
		}

		if (RightHandGrip)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(RightHandGrip.position, .02f);
			Gizmos.DrawRay(RightHandGrip.position, RightHandGrip.forward * .06f);
		}
	}
}
