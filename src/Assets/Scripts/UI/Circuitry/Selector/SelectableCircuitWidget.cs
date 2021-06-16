using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class SelectableCircuitWidget : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
	{
		public GameObject circuitWidgetPrefab;
		private Circuit circuit;

		[SerializeField]
		private GameObject tooltipPrefab;

		private GameObject tooltipObject;

		public ConstructorGrid grid;
		public Image icon;

		public float initialTooltipDelay = .5f;
		private float tooltipDelay = 0f;
		private bool pointerOver = false;

		public bool Initialized { get; private set; } = false;

		private void Awake()
		{
			Initialize();
		}

		private void Start()
		{
			Setup(circuitWidgetPrefab);
		}

		private bool Initialize()
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
			Debug.Log($"{this} drag began: {eventData.selectedObject}");
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			Debug.Log($"{this} drag ended: {eventData.selectedObject}");
		}

		public void OnDrag(PointerEventData eventData)
		{
			//Debug.Log($"{this} drag ended: {eventData.selectedObject}");
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			pointerOver = true;

			tooltipDelay = 0f;
		}

		public void Update()
		{
			if (!pointerOver)
				return;

			if (tooltipObject)
			{
				tooltipObject.transform.position = Input.mousePosition;
				return;
			}

			tooltipDelay += Time.deltaTime;
			if (tooltipDelay > initialTooltipDelay)
				CreateTooltip();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			pointerOver = false;
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
	}
}
