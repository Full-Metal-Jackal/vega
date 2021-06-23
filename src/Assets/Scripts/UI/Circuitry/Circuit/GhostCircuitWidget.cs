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
		public GameObject CircuitWidgetPrefab { get; protected set; }
		public DraggableCircuitWidget Source { get; protected set; }

		private Vector2 gripOffset;

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

		public void Setup(DraggableCircuitWidget source, GameObject circuitPrefab, Vector2 offset)
		{
			Source = source;
			gripOffset = offset;
			Setup(circuitPrefab);
		}

		private void OnGUI()
		{
			RectTransform.localScale = CircuitConstructor.Instance.ViewportScale;
		}

		public void Suicide()
		{
			Destroy(gameObject);
			AssemblyGrid.OnGhostDestroyed(this);
		}

		public void SetPosition(Vector2 position)
		{
			RectTransform.position = position;
			position += gripOffset;
			AssemblyGrid.OnGhostHover(this, position);
		}
	}
}
