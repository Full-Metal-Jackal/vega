using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DGPixelationRenderFeature : ScriptableRendererFeature
{
	[System.Serializable]
	public class Settings
	{
		public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
		public LayerMask layerMask = ~0;
		public Material material;
	}
	public Settings settings = new Settings();

	private DGRenderPass pass;

	public override void Create()
	{
		pass = new DGRenderPass(settings);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(pass);
	}
}
