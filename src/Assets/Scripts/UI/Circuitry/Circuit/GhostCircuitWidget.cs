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

		private Vector2 gripOffset;

		public AssemblyGrid AssemblyGrid
		{
			get
			{
				AssemblyGrid result = null;
				if (Game.circuitConstructor.AssemblyWidget)
					result = Game.circuitConstructor.AssemblyWidget.Grid;
				return result;
			}
		}

		public void Setup(GameObject circuitPrefab, Vector2 offset)
		{
			gripOffset = offset;
			Setup(circuitPrefab);
		}

		private void OnGUI()
		{
			RectTransform.localScale = Game.circuitConstructor.Viewport.Content.localScale;
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
