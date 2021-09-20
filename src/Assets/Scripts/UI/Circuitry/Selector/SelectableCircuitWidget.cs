using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using Circuitry;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public class SelectableCircuitWidget : DraggableCircuitWidget, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		private CircuitTooltipWidget tooltipPrefab;
		private CircuitTooltipWidget tooltip;

		[SerializeField]
		private CircuitWidget circuitWidgetPrefab;

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

			if (tooltip)
				DestroyTooltip();
		}

		public void CreateTooltip()
		{
			tooltip = Instantiate(tooltipPrefab);
			tooltip.transform.SetParent(CircuitConstructor.Instance.transform);
			
			tooltip.Setup(CircuitPrefab);
		}

		public void DestroyTooltip()
		{
			Destroy(tooltip.gameObject);
		}

		public void OnGUI()
		{
			if (!hovered)
				return;

			if (tooltip)
			{
				tooltip.transform.position = Mouse.current.position.ReadValue();
				return;
			}

			tooltipDelay += Time.deltaTime;
			if (tooltipDelay > initialTooltipDelay)
				CreateTooltip();
		}
		public override void DropOnAssembly(AssemblyWidget assemblyWidget, Vector2Int cell)
		{
			CircuitWidget circuitWidget = Instantiate(circuitWidgetPrefab);
			circuitWidget.Setup(CircuitPrefab);

			if (!assemblyWidget.AddCircuit(circuitWidget, cell))
				Destroy(circuitWidget.gameObject);
		}
	}
}
