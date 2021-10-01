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

   [SerializeField]
	protected Transform[] barrels;

	private int currentBarrel = 0;
	public override Transform Barrel => barrels[++currentBarrel % barrels.Length];
}
