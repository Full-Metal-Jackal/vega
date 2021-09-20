using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Circuitry;
using System;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public class CircuitContainer : MonoBehaviour
	{
		public bool Initialized { get; protected set; } = false;

		[field: SerializeField]
		public Image Icon { get; private set; }

		[field: SerializeField]
		public CellGridWidget Grid { get; private set; }

		[SerializeField]
		private RectTransform dataInputs;

		[SerializeField]
		private RectTransform dataOutputs;

		[SerializeField]
		private RectTransform pulseInputs;

		[SerializeField]
		private RectTransform pulseOutputs;

		public Vector2 OriginOffset =>
			-((Circuit.Shape.Center - new Vector2(.5f, .5f)) * new Vector2(1f, -1f))
			* Grid.Layout.cellSize;

		public Circuit Circuit { get; private set; }

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

			Circuit = GetComponent<Circuit>();

			return Initialized = true;
		}

		private void Start()
		{
			Setup();
		}

		public virtual void Setup()
		{
			Grid.BuildGrid(Circuit.Shape);
		}

		private PinWidget AddPin(PinWidget widgetPrefab, Transform toParent, Pin pin)
		{
			if (!(Instantiate(widgetPrefab) is PinWidget widget))
				throw new Exception($"Invalid pin widget prefab provided to {this} for {pin}: {widgetPrefab}.");

			widget.transform.SetParent(toParent);
			widget.Setup(pin);

			return widget;
		}

		public PinWidget AddPin(PinWidget widgetPrefab, DataInput dataInput) => AddPin(widgetPrefab, dataInputs, dataInput);
		public PinWidget AddPin(PinWidget widgetPrefab, DataOutput dataOutput) => AddPin(widgetPrefab, dataOutputs, dataOutput);
		public PinWidget AddPin(PinWidget widgetPrefab, PulseInput pulseInput) => AddPin(widgetPrefab, pulseInputs, pulseInput);
		public PinWidget AddPin(PinWidget widgetPrefab, PulseOutput pulseOutput) => AddPin(widgetPrefab, pulseOutputs, pulseOutput);

		public override string ToString() => $"{Circuit}'s visualization";
	}
}
