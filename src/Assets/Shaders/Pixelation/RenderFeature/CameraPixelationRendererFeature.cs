using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraPixelationRendererFeature : ScriptableRendererFeature
{
	[System.Serializable]
	public class Settings
	{
		public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
		public LayerMask layerMask = ~0;
		public int rendererLayerMask = 1;
		public Material material;
	}
	public Settings settings = new Settings();

	private CameraPixelationRenderPass pass;
	public override void Create()
	{
		pass = new CameraPixelationRenderPass(settings);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(pass);
	}
}
