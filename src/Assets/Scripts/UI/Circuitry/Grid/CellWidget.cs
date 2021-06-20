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

		[SerializeField]
		private Image background;

		private void Start() => SetHighlight(false);

		public Color BorderColor
		{
			get => border.color;
			set => border.color = value;
		}

		public Color BackgroundColor
		{
			get => background.color;
			set => background.color = value;
		}

		public void SetHighlight(bool enabled, bool error = false) => BorderColor = enabled ? (error ? errorColor : highlightedColor) : normalColor;
	}
}
