using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(CircuitConnections))]
	public class CircuitTooltipWidget : CircuitWidgetBase
	{
		[SerializeField]
		private Text label;

		[SerializeField]
		private Text desc;

		[SerializeField]
		private RectTransform circuitHolder;
		protected override RectTransform CircuitHolder => circuitHolder;

		[SerializeField]
		private CircuitConnections connections;

		protected override bool Initialize()
		{
			connections = GetComponent<CircuitConnections>();

			return base.Initialize();
		}

		public override void Setup(GameObject circuitPrefab)
		{
			base.Setup(circuitPrefab);

			label.text = Circuit.BoundCircuit.Label;
			desc.text = Circuit.BoundCircuit.Desc;
			connections.Setup(Circuit);
		}
	}
}
