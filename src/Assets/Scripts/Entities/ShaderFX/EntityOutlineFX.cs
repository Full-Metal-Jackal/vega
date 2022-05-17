using UnityEngine;

public class EntityOutlineFX : EntityShaderFX
{
	private static readonly int outlineID = Shader.PropertyToID("_Flash");  // <TODO> implement actual outline in the shader

	public Color color = Color.white;

	private bool startEnabled;

	private void Start()
	{
		enabled = startEnabled;
	}

	private void OnEnable()
	{
		foreach (Material material in Materials)
			// material.SetColor(outlineID, color);
			material.SetFloat(outlineID, 1f);
	}

	private void OnDisable()
	{
		foreach (Material material in Materials)
			material.SetFloat(outlineID, 0f);
	}
}
