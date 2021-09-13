using System;
using UnityEngine;

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

	public void SpawnDecal(Vector3 point, Vector3 normal, Transform targetTransform, ImpactType type)
	{
		SurfaceDataHolder holder = targetTransform.GetComponentInParent<SurfaceDataHolder>();
		if (!holder || !holder.SurfaceData)
			return;
		
		GameObject decal = NextDecal();
		if (!decal)
			return;
		
		SpriteRenderer spriteRenderer = decal.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = GetImpactTypeSprite(holder.SurfaceData, type);

		decal.transform.position = point;
		decal.transform.rotation = Quaternion.FromToRotation(Vector3.forward, normal);
		decal.transform.SetParent(targetTransform, worldPositionStays: true);
		decal.SetActive(true);
	}

	public void SpawnDecal(ContactPoint contact, ImpactType type) =>
		SpawnDecal(contact.point, contact.normal, contact.otherCollider.transform, type);

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
		if (!inst.GetComponent<SpriteRenderer>())
			throw new Exception(
				$"Invalid prefab specified in {this}, make sure it has a {typeof(SpriteRenderer)} component"
			);
	}

	private Sprite GetImpactTypeSprite(SurfaceData surfaceData, ImpactType type)
	{
		switch (type)
		{
			case ImpactType.Bullet:
				return Utils.Pick(surfaceData.BulletHoles);
			case ImpactType.Energy:
				return Utils.Pick(surfaceData.EnergyMarks);
			default:
				return null;
		}
	}
}
