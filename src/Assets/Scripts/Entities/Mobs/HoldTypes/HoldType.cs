using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Data/HoldType"), Serializable]
public class HoldType : ScriptableObject
{
	[field: SerializeField]
	public Quaternion MobAngleOffset { get; private set; } = Quaternion.identity;

	[field: SerializeField]
	public int AnimatorValue { get; private set; } = 0;

	[field: SerializeField]
	public SocketType Socket { get; set; } = SocketType.RightHand;

	/// <summary>
	/// If hands Ik should be used for the non-parent hand of the item's owner.
	/// Used for weapons held with both hands.
	/// </summary>
	public bool UsesHandIk = false;

	public enum SocketType
	{
		Belt,
		LeftHand,
		RightHand,
		Spine
	}

	public Transform GetSocket(Mob mob)
	{
		if (mob is Humanoid humanoid)
		{
			Transform socketTransform = null;
			switch (Socket)
			{
			case SocketType.RightHand:
				socketTransform = humanoid.RightHandSocket;
				break;
			case SocketType.LeftHand:
				socketTransform = humanoid.LeftHandSocket;
				break;
			}
			if (socketTransform)
				return socketTransform;
		}

		return mob.ItemSocket;
	}
}
