using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(Rect))]
[RequireComponent(typeof(Texture))]
public class ImageAspectFill : MonoBehaviour
{
	private RectTransform rt;
	private RawImage img;
	private Rect lastBounds;
	private Texture lastTexture;

	private void Awake()
	{
		rt = transform as RectTransform;
		img = GetComponent<RawImage>();

		enabled = false;
	}
	
	private void Update()
	{
		if (rt.rect != lastBounds || img.mainTexture != lastTexture)
			UpdateUV();
	}

	private void UpdateUV()
	{
		lastBounds = rt.rect;
		lastTexture = img.mainTexture;

		float frameAspect = lastBounds.width / lastBounds.height;
		float imageAspect = lastTexture.width / lastTexture.height;
		float w = frameAspect / imageAspect;
		float h = imageAspect / frameAspect;

		if (frameAspect == imageAspect)
			img.uvRect = new Rect(0f, 0f, 1f, 1f);
		else if (w < 1f)
			img.uvRect = new Rect(0.5f - w * 0.5f, 0f, w, 1);
		else
			img.uvRect = new Rect(0f, 0.5f - h * 0.5f, 1f, h);
	}
}
