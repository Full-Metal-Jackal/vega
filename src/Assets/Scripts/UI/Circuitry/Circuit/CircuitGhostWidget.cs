using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public class CircuitGhostWidget : MonoBehaviour
	{
		private RectTransform rectTransform;

		public RectTransform RectTransform => rectTransform;
		public bool Initialized { get; private set; } = false;

		public GameObject CircuitWidgetPrefab {get; protected set;}

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

			rectTransform = GetComponent<RectTransform>();

			return Initialized = true;
		}

		public void Setup(GameObject circuitWidgetPrefab)
		{
			CircuitWidgetPrefab = circuitWidgetPrefab;
			Circuit circuit = circuitWidgetPrefab.GetComponent<Circuit>();

			// After two days of vain attempts to create an adequate resizing behaviour
			// I came to a conclusion that it would be best to simply mimic the original circuit by duplicating its elements directly.
			RectTransform shape = Instantiate(circuitWidgetPrefab.transform.Find("Shape")).GetComponent<RectTransform>();
			shape.SetParent(rectTransform);
			shape.GetComponent<CellGridWidget>().BuildGrid(circuit.shape);

			RectTransform icon = Instantiate(circuitWidgetPrefab.transform.Find("Icon")).GetComponent<RectTransform>();
			icon.SetParent(rectTransform);
		}

		private void OnGUI()
		{
			rectTransform.localScale = Game.circuitConstructor.Viewport.Content.localScale;
		}

		public void Suicide() => Destroy(gameObject);

		public void SetPosition(Vector3 position)
		{
			rectTransform.position = position;
		}
	}
}
