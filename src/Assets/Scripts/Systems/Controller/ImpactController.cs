using System;
using UnityEngine;
using UnityEngine.VFX;
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
	private Decal decalPrefab;

	private Decal[] decals;

	protected override void Awake()
	{
		decals = new Decal[max];
		for (int i = 0; i < max; ++i)
			decals[i] = InstantiateDecal();
	}

	public void SpawnDecal(Vector3 point, Vector3 normal, Transform targetTransform, ImpactType type, float scale)
	{
		SurfaceDataHolder holder = targetTransform.GetComponentInParent<SurfaceDataHolder>();
		if (!holder || !holder.SurfaceData)
			return;
		
		Decal decal = NextDecal();
		if (!decal)
			return;
		
		DecalData decalData = GetImpactTypeData(holder.SurfaceData, type);
		if (!decalData)
			return;
		
		decal.transform.localScale = new Vector3(scale, scale, scale);
		decal.decalData = decalData;

		decal.transform.SetPositionAndRotation(point, Quaternion.FromToRotation(-Vector3.forward, normal));
		decal.transform.SetParent(targetTransform, worldPositionStays: true);
		decal.gameObject.SetActive(true);
	}

	public void SpawnDecal(Vector3 point,ContactPoint contact, ImpactType type, float scale) =>
		SpawnDecal(point, contact.normal, contact.otherCollider.transform, type, scale);

	public void SpawnImpactEffect(Vector3 point, Vector3 normal, Transform targetTransform, VisualEffect impactEffect, float lifespan)
	{
		impactEffect.transform.SetPositionAndRotation(point, Quaternion.FromToRotation(-Vector3.forward, normal));
		impactEffect.transform.SetParent(targetTransform, worldPositionStays: true);
		impactEffect.enabled = true;
		impactEffect.Play();
		Destroy(impactEffect.gameObject, lifespan);
	}

	public void SpawnImpactEffect(Vector3 point, ContactPoint contact, VisualEffect impactEffect, float lifespan) =>
		SpawnImpactEffect(point, contact.normal, contact.otherCollider.transform, impactEffect, lifespan);

	public void ClearDecals()
	{
		foreach (Decal decal in decals)
			ResetDecal(decal);
	}

	private Decal InstantiateDecal()
	{
		Decal decal = Instantiate(decalPrefab);
		ResetDecal(decal);

		return decal;
	}

	private Decal NextDecal()
	{
		Decal decal = decals[nextIdx];
		if (++nextIdx == max)
			nextIdx = 0;

		return decal;
	}

	private void ResetDecal(Decal decal)
	{
		decal.transform.SetParent(transform);
		decal.gameObject.SetActive(false);
	}

	private static DecalData GetImpactTypeData(SurfaceData surfaceData, ImpactType type)
	{
		switch (type)
		{
		case ImpactType.Bullet:
			return PickDecal(surfaceData.BulletHoles);
		case ImpactType.Energy:
			return PickDecal(surfaceData.EnergyImpacts);
		case ImpactType.Explosion:
			if (!surfaceData.ReceivesExplosionImpacts)
				return null;
			return PickDecal(surfaceData.ExplosionImpacts);
		default:
			throw new Exception($"Invalid {typeof(ImpactType)} specified: {type}");
		}
	}

	private static DecalData PickDecal(DecalData[] data) => data.Length > 0 ? Utils.Pick(data) : null;
}
