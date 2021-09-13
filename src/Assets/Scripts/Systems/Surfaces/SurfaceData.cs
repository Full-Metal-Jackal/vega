using UnityEngine;

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
	public bool ReceivesExplosionMarks = true;
	[field: SerializeField]
	public Sprite[] BulletHoles;
	[field: SerializeField]
	public Sprite[] EnergyMarks;
}
