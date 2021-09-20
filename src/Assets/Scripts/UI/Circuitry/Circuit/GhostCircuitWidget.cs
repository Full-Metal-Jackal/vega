using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public class GhostCircuitWidget : CircuitWidgetBase
	{
		public CircuitWidget CircuitWidgetPrefab { get; protected set; }
		public DraggableCircuitWidget Source { get; protected set; }

		public AssemblyGrid AssemblyGrid
		{
			get
			{
				AssemblyGrid result = null;
				if (CircuitConstructor.Instance.AssemblyWidget)
					result = CircuitConstructor.Instance.AssemblyWidget.Grid;
				return result;
			}
		}

		public void Setup(DraggableCircuitWidget source, CircuitContainer circuitPrefab)
		{
			Source = source;
			Setup(circuitPrefab);
		}

		private void OnGUI()
		{
			RectTransform.localScale = CircuitConstructor.Instance.AssemblyWidget.Grid.transform.parent.localScale
				* CircuitConstructor.Instance.Viewport.Zoom;
		}

		public void Suicide()
		{
			Destroy(gameObject);
			if (AssemblyGrid)
				AssemblyGrid.OnGhostDestroyed(this);
		}

		public void SetPosition(Vector2 position)
		{
			RectTransform.position = position;
			position += Source.GripOffset;
			if (AssemblyGrid)
				AssemblyGrid.OnGhostHover(this, position);
		}
	}
}
