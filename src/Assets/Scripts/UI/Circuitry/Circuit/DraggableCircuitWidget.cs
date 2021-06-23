using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CircuitConstructor
{
	public abstract class DraggableCircuitWidget : CircuitWidgetBase, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		[SerializeField]
		private GameObject circuitGhostPrefab;
		private GhostCircuitWidget ghost;

		// In future, we may want to change this value to implement the ability to drag circuits at specific point.
		public Vector2 GripOffset => Circuit.OriginOffset;

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			CreateGhost();

			PostBeginDrag(eventData);
		}

		public virtual void PostBeginDrag(PointerEventData eventData)
		{
		}

		private void CreateGhost()
		{
			ghost = Instantiate(circuitGhostPrefab).GetComponent<GhostCircuitWidget>();
			ghost.RectTransform.SetParent(Game.CircuitConstructor.transform, false);
			ghost.Setup(this, CircuitPrefab, GripOffset);
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			ghost.Suicide();

			PostEndDrag(eventData);
		}

		public virtual void PostEndDrag(PointerEventData eventData)
		{
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			ghost.SetPosition(eventData.pointerCurrentRaycast.screenPosition);
		}

		public abstract void DropOnAssembly(AssemblyWidget assemblyWidget, Vector2Int cell);
	}
}
