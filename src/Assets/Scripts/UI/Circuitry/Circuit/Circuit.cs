using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Circuitry;
using System;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public class Circuit : MonoBehaviour
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

		public Vector2 OriginOffset => -(Grid.Layout.cellSize * (BoundCircuit.Shape.GetSize() - Vector2Int.one) * new Vector2Int(1, -1) * .5f) * RectTransform.localScale;
		
		[ObsoleteAttribute("This getter is actually more reliable, but won't work until the grid is setup. May be merged with the previous one later.")]
		public Vector2 OriginOffsetByGrid => -(Grid.Layout.cellSize * .5f + Grid.RectTransform.rect.min) * RectTransform.localScale;

		public RectTransform RectTransform { get; private set; }
		public Circuitry.Circuit BoundCircuit { get; private set; }

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

			BoundCircuit = GetComponent<Circuitry.Circuit>();
			RectTransform = GetComponent<RectTransform>();

			return Initialized = true;
		}

		private void Start()
		{
			Setup();
		}

		public virtual void Setup()
		{
			Grid.BuildGrid(BoundCircuit.Shape);
		}

		private PinWidgetBase AddPin(GameObject widgetPrefab, Transform toParent, Pin pin)
		{
			GameObject widgetObject = Instantiate(widgetPrefab);

			if (!(widgetObject && widgetObject.TryGetComponent(out PinWidgetBase widget)))
				throw new System.Exception($"{this} received invalid pin prefab: {widgetPrefab}.");

			widgetObject.transform.SetParent(toParent);

			widget.Setup(pin);

			return widget;
		}

		public PinWidgetBase AddPin(GameObject widgetPrefab, DataInput dataInput) => AddPin(widgetPrefab, dataInputs, dataInput);
		public PinWidgetBase AddPin(GameObject widgetPrefab, DataOutput dataOutput) => AddPin(widgetPrefab, dataOutputs, dataOutput);
		public PinWidgetBase AddPin(GameObject widgetPrefab, PulseInput pulseInput) => AddPin(widgetPrefab, pulseInputs, pulseInput);
		public PinWidgetBase AddPin(GameObject widgetPrefab, PulseOutput pulseOutput) => AddPin(widgetPrefab, pulseOutputs, pulseOutput);

		public override string ToString() => $"{BoundCircuit}'s visualization";
	}
}
