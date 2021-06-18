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

		public Vector2 GripOffset
		{
			get
			{
				Vector2 gripOffset = -(Circuit.Grid.Layout.cellSize * .5f + Circuit.Grid.RectTransform.rect.min);
				return gripOffset * RectTransform.localScale;
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			CreateGhost();
		}

		private void CreateGhost()
		{
			ghost = Instantiate(circuitGhostPrefab).GetComponent<GhostCircuitWidget>();
			ghost.RectTransform.SetParent(Game.circuitConstructor.transform, false);
			ghost.Setup(CircuitPrefab, GripOffset);
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			ghost.Suicide();
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			ghost.SetPosition(eventData.pointerCurrentRaycast.screenPosition);
		}

		public abstract void DropOnAssembly(AssemblyWidget assemblyWidget, Vector2Int cell);
	}
}
