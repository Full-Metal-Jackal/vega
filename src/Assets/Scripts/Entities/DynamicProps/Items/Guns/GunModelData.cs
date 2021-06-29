using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModelData : ItemModelData
{
	/// <summary>
	/// The gun's barrel position.
	/// </summary>
	[field: SerializeField]
	public Transform Barrel { get; private set; }
}
