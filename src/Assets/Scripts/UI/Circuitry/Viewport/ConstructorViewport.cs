using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CircuitConstructor
{
	public class ConstructorViewport : MonoBehaviour, IScrollHandler
	{
		public bool Initialized { get; protected set; } = false;

		[field: SerializeField]
		public RectTransform Content { get; private set; }

		public float minZoom = .6f, maxZoom = 3f;
		public float zoomSensivity = .1f;
		
		private float zoom;
		public float Zoom
		{
			get => zoom;
			set
			{
				zoom = Mathf.Clamp(value, minZoom, maxZoom);
				Content.localScale = new Vector2(zoom, zoom);
			}
		}

		private void Awake()
		{
			Initialize();
		}

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			Zoom = 1f;

			return Initialized = true;
		}

		public void OnScroll(PointerEventData eventData)
		{
			Zoom += eventData.scrollDelta.y * zoomSensivity;
		}
	}
}
