using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CircuitConstructor
{
	public class ConstructorViewport : MonoBehaviour, IScrollHandler
	{
		public bool Initialized { get; private set; } = false;

		private RectTransform content;
		public RectTransform Content => content;

		public float minZoom = .6f, maxZoom = 3f;
		public float zoomSensivity = .1f;
		
		private float zoom;
		public float Zoom
		{
			get => zoom;
			set
			{
				zoom = Mathf.Clamp(value, minZoom, maxZoom);
				content.localScale = new Vector2(zoom, zoom);
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

			content = transform.Find("Content").GetComponent<RectTransform>();

			return Initialized = true;
		}

		public void OnScroll(PointerEventData eventData)
		{
			Zoom += eventData.scrollDelta.y * zoomSensivity;
		}
	}
}
