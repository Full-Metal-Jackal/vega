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
		private CircuitPorts ports;

		protected override bool Initialize()
		{
			ports = GetComponent<CircuitPorts>();

			return base.Initialize();
		}

		public override void Setup(GameObject circuitPrefab)
		{
			base.Setup(circuitPrefab);

			label.text = Circuit.Circuit.Label;
			desc.text = Circuit.Circuit.Desc;
			ports.Setup(Circuit);
		}
	}
}
