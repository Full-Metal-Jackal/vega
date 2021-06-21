using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(CircuitPorts))]
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
		private CircuitPorts ports;

		protected override bool Initialize()
		{
			ports = GetComponent<CircuitPorts>();

			return base.Initialize();
		}

		public override void Setup(GameObject circuitPrefab)
		{
			base.Setup(circuitPrefab);

			label.text = Circuit.BoundCircuit.Label;
			desc.text = Circuit.BoundCircuit.Desc;
			ports.Setup(Circuit);
		}
	}
}
