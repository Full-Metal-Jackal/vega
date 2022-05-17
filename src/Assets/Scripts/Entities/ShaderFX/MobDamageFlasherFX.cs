using UnityEngine;

public class MobDamageFlasherFX : EntityShaderFX
{
	private float currentFlashLife, targetFlashLife;

	private float peakIntensity;

	// <TODO> Should be bound to mob's OnActiveItemChanged, not implemented yet.
	//private Renderer[] itemsMeshRenderers;

	private static readonly int flashID = Shader.PropertyToID("_Flash");

	protected override void Awake()
	{
		base.Awake();

		if (Entity is Mob mob)
			mob.OnDamaged += (Mob __) => Flash();
		else
			Debug.LogError($"{this} should be assigned to a mob.");
	}

	private void Update()
	{
		currentFlashLife = Mathf.Max(currentFlashLife - Time.deltaTime, 0f);

		float intensity = 0;
		if (currentFlashLife > 0f)
			intensity = currentFlashLife / targetFlashLife * peakIntensity;
		else
			enabled = false;

		foreach (Material material in Materials)
			material.SetFloat(flashID, intensity);
	}

	public void Flash(float flashTime = .1f, float intensity = 4f)
	{
		enabled = true;

		peakIntensity = intensity;
		currentFlashLife = targetFlashLife = flashTime;
	}
}
