using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunModelData : GunModelData
{
	/// <summary>
	/// The model's spinning part.
	/// </summary>
	[field: SerializeField]
	public Transform BarrelsBase { get; private set; }

	[field: SerializeField]
	public Transform[] Barrels { get; private set; }
}
