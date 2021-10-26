using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CircuitConstructor
{
	public class ConstructorViewport : MonoBehaviour, IScrollHandler
	{
		public bool Initialized { get; protected set; } = false;

		[field: SerializeField]
		public RectTransform Content { get; private set; }

		public float minZoom = .3f, maxZoom = 1.0f;
		public float zoomSensivity = .05f;
		
		private float __zoom;
		public float Zoom
		{
			get => __zoom;
			set
			{
				__zoom = Mathf.Clamp(value, minZoom, maxZoom);
				Content.localScale = new Vector2(__zoom, __zoom);
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

			Zoom = (maxZoom + minZoom) * 0.5f;

			return Initialized = true;
		}

		public void OnScroll(PointerEventData eventData)
		{
			Zoom += eventData.scrollDelta.y * zoomSensivity;
		}
	}
}
