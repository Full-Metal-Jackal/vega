using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	public abstract class PinWidgetBase : MonoBehaviour
	{
		public bool Initialized { get; protected set; } = false;
		
		public RectTransform RectTransform { get; private set; }

		[SerializeField]
		private Text label;
		public Text Label => label;

		public Circuitry.Pin BoundPin { get; protected set; }

		private void Awake()
		{
			Initialize();
		}

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			RectTransform = GetComponent<RectTransform>();

			return Initialized = true;
		}

		public virtual void Setup(Circuitry.Pin pin)
		{
			BoundPin = pin;
			SetLabel(pin.label);
		}

		public void SetLabel(string text) => Label.text = text;
	}
}
