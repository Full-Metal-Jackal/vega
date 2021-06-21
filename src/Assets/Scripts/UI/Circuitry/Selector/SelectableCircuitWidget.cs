using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public class SelectableCircuitWidget : DraggableCircuitWidget, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		private GameObject tooltipPrefab;
		private GameObject tooltipObject;

		[SerializeField]
		private GameObject circuitWidgetPrefab;

		public float initialTooltipDelay = .5f;
		private float tooltipDelay = 0f;
		private bool hovered = false;

		protected override void Setup()
		{
			base.Setup();
		}

		public override void PostBeginDrag(PointerEventData eventData)
		{
			DestroyTooltip();
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
			tooltip.Setup(CircuitPrefab);
		}

		public void DestroyTooltip()
		{
			Destroy(tooltipObject);
		}

		public void OnGUI()
		{
			if (!hovered)
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
		public override void DropOnAssembly(AssemblyWidget assemblyWidget, Vector2Int cell)
		{
			CircuitWidget circuitWidget = Instantiate(circuitWidgetPrefab).GetComponent<CircuitWidget>();
			circuitWidget.Setup(CircuitPrefab);

			if (!assemblyWidget.AddCircuit(circuitWidget, cell))
				Destroy(circuitWidget.gameObject);
		}
	}
}
