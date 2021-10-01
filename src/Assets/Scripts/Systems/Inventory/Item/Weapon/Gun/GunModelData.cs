using UnityEngine;

public class GunModelData : ItemModelData
{
	/// <summary>
	/// The gun's barrel position.
	/// </summary>
	[field: SerializeField]
	public virtual Transform Barrel { get; private set; }
}
