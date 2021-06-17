using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	public class CellWidget : MonoBehaviour
	{
		public bool Initialized { get; private set; } = false;

		[SerializeField]
		private Color normalColor;
		
		[SerializeField]
		private Color highlightedColor;

		[SerializeField]
		private Image border;

		public void SetHighlight(bool enabled) => border.color = enabled ? highlightedColor : normalColor;
	}
}
