using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	public class CellWidget : MonoBehaviour
	{
		[SerializeField]
		private Color normalColor;

		[SerializeField]
		private Color highlightedColor;

		[SerializeField]
		private Color errorColor;

		[SerializeField]
		private Image border;

		private void Start() => SetHighlight(false);

		public void SetHighlight(bool enabled, bool error = false) => border.color = enabled ? (error ? errorColor : highlightedColor) : normalColor;
	}
}
