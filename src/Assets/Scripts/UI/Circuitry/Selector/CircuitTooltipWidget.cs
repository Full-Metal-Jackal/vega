using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	public class CircuitTooltipWidget : CircuitWidgetBase
	{
		[SerializeField]
		private Text label;

		[SerializeField]
		private Text desc;

		[SerializeField]
		private RectTransform circuitHolder;
		protected override RectTransform CircuitHolder => circuitHolder;

		public override void Setup(GameObject circuitPrefab)
		{
			base.Setup(circuitPrefab);

			label.text = Circuit.BoundCircuit.Label;
			desc.text = Circuit.BoundCircuit.Desc;
		}
	}
}
