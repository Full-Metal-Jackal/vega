using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.CircuitConstructor
{
	public class AssemblyCellWidget : CellWidget, IPointerEnterHandler, IDropHandler
	{
		public Vector2Int Cell { get; protected set; }
		public AssemblyGridWidget Grid { get; protected set; }

		public void OnDrop(PointerEventData eventData)
		{
			if (!(eventData.dragging && eventData.pointerDrag.TryGetComponent(out CircuitGhostWidget ghost)))
				return;
			Grid.OnGhostDropped(ghost, Cell);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!(eventData.dragging && eventData.pointerDrag.TryGetComponent(out CircuitGhostWidget ghost)))
				return;
			Grid.OnGhostHover(ghost, Cell);
		}

		public void Setup(Vector2Int cell, AssemblyGridWidget grid)
		{
			Cell = cell;
			Grid = grid;
		}
	}
}
