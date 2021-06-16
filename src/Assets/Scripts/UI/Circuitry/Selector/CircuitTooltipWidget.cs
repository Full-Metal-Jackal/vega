using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	public class CircuitTooltipWidget : MonoBehaviour
	{
		public Text label;
		public Text desc;
		private Transform circuitHolder;

		private void Awake()
		{
			circuitHolder = transform.Find("PaddedContainer").Find("Circuit");
		}

		public void DisplayCircuit(GameObject circuitWidgetPrefab)
		{
			CircuitWidget circuitWidget = Instantiate(circuitWidgetPrefab).GetComponent<CircuitWidget>();
			circuitWidget.transform.SetParent(circuitHolder, false);

			// <TODO> Make sure that circuitWidget is "deactivated" and can be used only for visualization.
		}
	}
}
