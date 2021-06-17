using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public class SelectableCircuitWidget : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
	{
		public GameObject circuitWidgetPrefab;
		private Circuit circuit;

		[SerializeField]
		private GameObject circuitGhostPrefab;
		private CircuitGhostWidget ghost;

		[SerializeField]
		private GameObject tooltipPrefab;
		private GameObject tooltipObject;

		public CellGridWidget grid;
		public Image icon;

		public float initialTooltipDelay = .5f;
		private float tooltipDelay = 0f;
		private bool hovered = false;

		public bool Initialized { get; private set; } = false;

		private void Awake()
		{
			Initialize();
		}

		private void Start()
		{
			if (circuitWidgetPrefab)
				Setup(circuitWidgetPrefab);
		}

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			return Initialized = true;
		}

		private void Setup(GameObject circuitWidgetPrefab)
		{
			circuit = circuitWidgetPrefab.GetComponent<Circuit>();
			grid.BuildGrid(circuit.shape);
			icon.sprite = circuitWidgetPrefab.transform.Find("Icon").GetComponent<Image>().sprite;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			CreateGhost();
		}

		private void CreateGhost()
		{
			ghost = Instantiate(circuitGhostPrefab).GetComponent<CircuitGhostWidget>();
			ghost.Setup(circuitWidgetPrefab);
			ghost.RectTransform.SetParent(Game.circuitConstructor.transform, false);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			ghost.Suicide();
		}

		public void OnDrag(PointerEventData eventData)
		{
			ghost.SetPosition(eventData.pointerCurrentRaycast.screenPosition);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			hovered = true;

			tooltipDelay = 0f;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			hovered = false;
			tooltipDelay = 0f;

			if (tooltipObject)
				DestroyTooltip();
		}

		public void CreateTooltip()
		{
			tooltipObject = Instantiate(tooltipPrefab);
			tooltipObject.transform.SetParent(Game.circuitConstructor.transform);
			
			CircuitTooltipWidget tooltip = tooltipObject.GetComponent<CircuitTooltipWidget>();
			tooltip.label.text = circuit.Label;
			tooltip.desc.text = circuit.Desc;
			tooltip.DisplayCircuit(circuitWidgetPrefab);
		}

		public void DestroyTooltip()
		{
			Destroy(tooltipObject);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!hovered)
				return;

			if (tooltipObject)
			{
				tooltipObject.transform.position = eventData.pointerCurrentRaycast.screenPosition;
				return;
			}

			tooltipDelay += Time.deltaTime;
			if (tooltipDelay > initialTooltipDelay)
				CreateTooltip();
		}
	}
}
