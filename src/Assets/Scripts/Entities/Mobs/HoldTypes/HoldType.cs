using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Data/HoldType"), Serializable]
public class HoldType : ScriptableObject
{
	[field: SerializeField]
	public Vector3 SocketOffset { get; private set; } = Vector3.zero;

	[field: SerializeField]
	public Quaternion AngleOffset { get; private set; } = Quaternion.identity;

	[field: SerializeField]
	public int AnimatorValue { get; private set; }  = 0;
}
