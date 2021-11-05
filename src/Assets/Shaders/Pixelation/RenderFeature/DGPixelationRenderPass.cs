using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DGRenderPass : ScriptableRenderPass
{
	const string profilerTag = "DGPixelationPass";

	private Material material;

	private RenderTargetIdentifier cameraColorTex, pixelTex, ditheredDepthTex;
	static int ditheredDepthTexID = Shader.PropertyToID("_DitheredDepthTexture");
	static int pixelTexID = Shader.PropertyToID("_PixelTex");

	// Madalaski tutorial
	private ProfilingSampler dgProfilingSampler;
	private ShaderTagId shaderTagId = new ShaderTagId("UniversalForward");
	private FilteringSettings filteringSettings;

	public DGRenderPass(DGPixelationRenderFeature.Settings settings)
	{
		dgProfilingSampler = new ProfilingSampler(profilerTag);

		renderPassEvent = settings.renderPassEvent;
		filteringSettings = new FilteringSettings(
			RenderQueueRange.all,  // <TODO> Might be related to transparent rendering issue.
			settings.layerMask
		);
		material = settings.material;
	}

	public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
	{
		ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);
		
		RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

		descriptor.colorFormat = RenderTextureFormat.ARGB1555;
		cmd.GetTemporaryRT(pixelTexID, descriptor, FilterMode.Point);
		pixelTex = new RenderTargetIdentifier(pixelTexID);

		descriptor.colorFormat = RenderTextureFormat.Depth;
		cmd.GetTemporaryRT(ditheredDepthTexID, descriptor, FilterMode.Point);
		ditheredDepthTex = new RenderTargetIdentifier(ditheredDepthTexID);

		cameraColorTex = renderingData.cameraData.renderer.cameraColorTarget;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		DrawingSettings drawingSettings = CreateDrawingSettings(
			shaderTagId,
			ref renderingData,
			SortingCriteria.CommonOpaque  // <TODO> Might be related to transparent rendering issue.
		);

		CommandBuffer cmd = CommandBufferPool.Get();
		using (new ProfilingScope(cmd, dgProfilingSampler))
		{
			// Setup RT that has to be pixelated
			cmd.SetRenderTarget(
				pixelTex, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare,
				ditheredDepthTex, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare
			);
			cmd.ClearRenderTarget(true, true, Color.clear);

			context.ExecuteCommandBuffer(cmd);
			cmd.Clear();

			// Render objects that need to be pixelated
			context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);

			// And then we pixelate the objects via DGPixelation material and slap the result onto the camera's RT.
			cmd.Blit(pixelTex, cameraColorTex, material);

			context.ExecuteCommandBuffer(cmd);
			cmd.Clear();
		}

		CommandBufferPool.Release(cmd);
	}

	public override void OnCameraCleanup(CommandBuffer cmd)
	{
		cmd.ReleaseTemporaryRT(pixelTexID);
		cmd.ReleaseTemporaryRT(ditheredDepthTexID);
	}
}
