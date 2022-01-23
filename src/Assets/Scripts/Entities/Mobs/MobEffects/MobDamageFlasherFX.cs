using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDamageFlasherFX : MonoBehaviour
{
	private float currentFlashLife, targetFlashLife;

	private float peakIntensity;

	[SerializeField]
	private Renderer[] mobRenderers;

	// <TODO> Should be bound to mob's OnActiveItemChanged, not implemented yet.
	//private MeshRenderer[] itemsMeshRenderers;


	private Material[] materials;

	private static readonly int flashID = Shader.PropertyToID("_Flash");

	private void Awake()
	{
		Mob mob = transform.parent.GetComponentInParent<Mob>();
		if (!mob)
		{
			Debug.LogError($"No mob found for {this}. Mob's renderer FX should be two levels deeper than the mob itself.");
			return;
		}

		mobRenderers = mob.GetComponentsInChildren<Renderer>();

		materials = new Material[mobRenderers.Length];
		
		for (int i = 0; i < mobRenderers.Length;  i++)
			materials[i] = mobRenderers[i].material;

		mob.OnDamaged += (Mob __) => Flash();
	}

	private void Update()
	{
		currentFlashLife = Mathf.Max(currentFlashLife - Time.deltaTime, 0f);

		float intensity = 0;
		if (currentFlashLife > 0f)
			intensity = currentFlashLife / targetFlashLife * peakIntensity;
		else
			enabled = false;

		foreach (Material material in materials)
			material.SetFloat(flashID, intensity);
	}

	public void Flash(float flashTime = .1f, float intensity = 4f)
	{
		enabled = true;

		this.peakIntensity = intensity;
		currentFlashLife = targetFlashLife = flashTime;
	}
}
