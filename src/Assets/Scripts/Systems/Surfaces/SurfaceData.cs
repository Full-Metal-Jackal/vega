using System.Linq;
using UnityEngine;
using kTools.Decals;

public enum SurfaceType
{
	Flesh,
	Glass,
	Metal,
	Plastic,
	Concrete,
}

[CreateAssetMenu(menuName ="Surface Data")]
public class SurfaceData : ScriptableObject
{
	[field: SerializeField, Header("General data")]
	public SurfaceType SurfaceType { get; private set; }

	[field: SerializeField, Header("Impact sprites")]
	public bool ReceivesExplosionImpacts { get; private set; } = true;
	[field: SerializeField]
	public DecalData[] BulletHoles { get; private set; }
	[field: SerializeField]
	public DecalData[] EnergyImpacts { get; private set; }
}
