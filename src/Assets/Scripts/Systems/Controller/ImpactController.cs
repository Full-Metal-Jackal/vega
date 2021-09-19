using System;
using UnityEngine;
using kTools.Decals;

public enum ImpactType
{
	Bullet,
	Energy,
	Explosion,
}

public class ImpactController : MonoSingleton<ImpactController>
{
	[SerializeField, Min(1)]
	private int max = 128;

	private int nextIdx = 0;

	[SerializeField]
	private GameObject prefab;

	private GameObject[] decals;

	protected override void Awake()
	{
		decals = new GameObject[max];
		for (int i = 0; i < max; ++i)
			decals[i] = InstantiateDecal();

		ValidatePrefab(decals[0]);
	}

	public void SpawnDecal(Vector3 point, Vector3 normal, Transform targetTransform, ImpactType type, float scale)
	{
		SurfaceDataHolder holder = targetTransform.GetComponentInParent<SurfaceDataHolder>();
		if (!holder || !holder.SurfaceData)
			return;
		
		GameObject decalObj = NextDecal();
		if (!decalObj)
			return;
		
		DecalData decalData = GetImpactTypeData(holder.SurfaceData, type);;
		if (!decalData)
			return;
		
		Decal decal = decalObj.GetComponent<Decal>();
		decal.transform.localScale = new Vector3(scale, scale, scale);
		decal.decalData = GetImpactTypeData(holder.SurfaceData, type);

		decalObj.transform.position = point;
		decalObj.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, normal);
		decalObj.transform.SetParent(targetTransform, worldPositionStays: true);
		decalObj.SetActive(true);
	}

	public void SpawnDecal(ContactPoint contact, ImpactType type, float scale) =>
		SpawnDecal(contact.point, contact.normal, contact.otherCollider.transform, type, scale);

	public void ClearDecals()
	{
		foreach (GameObject decal in decals)
			ResetDecal(decal);
	}

	private GameObject InstantiateDecal()
	{
		GameObject spawned = Instantiate(prefab);
		ResetDecal(spawned);

		return spawned;
	}

	private GameObject NextDecal()
	{
		GameObject decal = decals[nextIdx];
		if (++nextIdx == max)
			nextIdx = 0;

		return decal;
	}

	private void ResetDecal(GameObject decal)
	{
		decal.transform.SetParent(transform);
		decal.SetActive(false);
	}

	private void ValidatePrefab(GameObject inst)
	{
		if (!inst.GetComponent<Decal>())
			throw new Exception(
				$"Invalid prefab specified in {this}, make sure it has a {typeof(Decal)} component"
			);
	}

	private static DecalData GetImpactTypeData(SurfaceData surfaceData, ImpactType type)
	{
		switch (type)
		{
			case ImpactType.Bullet:
				return PickDecal(surfaceData.BulletHoles);
			case ImpactType.Energy:
				return PickDecal(surfaceData.EnergyImpacts);
			default:
				throw new Exception($"Invalid {typeof(ImpactType)} specified: {type}");
		}
	}

	private static DecalData PickDecal(DecalData[] data) => data.Length > 0 ? Utils.Pick(data) : null;
}
