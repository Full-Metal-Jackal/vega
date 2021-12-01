using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraPixelationRenderPass : ScriptableRenderPass
{
	const string profilerTag = "CameraPixelationPass";

	private readonly Material material;

	private RenderTargetIdentifier cameraColorTex, pixelTex, ditheredDepthTex;
	private static readonly int ditheredDepthTexID = Shader.PropertyToID("_CameraPixelationDepthTexture");
	private static readonly int pixelTexID = Shader.PropertyToID("_CameraPixelTex");
	private static readonly int offsetID = Shader.PropertyToID("_CameraOffset");

	private readonly ProfilingSampler cameraPixelationProfilingSampler;
	private static readonly ShaderTagId shaderTagId = new ShaderTagId("UniversalForward");
	private FilteringSettings filteringSettings;

	public CameraPixelationRenderPass(CameraPixelationRendererFeature.Settings settings)
	{
		cameraPixelationProfilingSampler = new ProfilingSampler(profilerTag);

		renderPassEvent = settings.renderPassEvent;
		filteringSettings = new FilteringSettings(
			RenderQueueRange.all,  // <TODO> Might be related to transparent rendering issue.
			settings.layerMask,
			1u << (settings.rendererLayerMask - 1)
		);
		material = settings.material;
	}

	public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
	{
		ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);

		RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

		descriptor.colorFormat = RenderTextureFormat.ARGB4444;
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
			SortingCriteria.CommonOpaque
		);

		CommandBuffer cmd = CommandBufferPool.Get();
		using (new ProfilingScope(cmd, cameraPixelationProfilingSampler))
		{
			// Setup RT that has to be pixelated
			cmd.SetRenderTarget(
				pixelTex, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare,
				ditheredDepthTex, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare
			);
			cmd.ClearRenderTarget(true, true, Color.clear);

			context.ExecuteCommandBuffer(cmd);
			cmd.Clear();

			// Note: may aswell use _ScreenParams.xy * WorldToViewportPoint, but seems to have less precision that way
			if (Camera.main)
				material.SetVector(offsetID, -Camera.main.WorldToScreenPoint(Vector3.zero));
			context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
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
