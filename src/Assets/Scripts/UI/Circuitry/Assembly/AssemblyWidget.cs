using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Circuitry;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(Assembly))]
	public class AssemblyWidget : MonoBehaviour, ITriggerable<Assembly>
	{
		public Assembly BoundAssembly { get; private set; }
		public bool Initialized { get; protected set; } = false;

		public float minZoom = .6f;
		public float preferedZoom = .71f;

		[SerializeField]
		private AssemblyGrid grid;
		public AssemblyGrid Grid => grid;

		[SerializeField]
		private RectTransform cirucitHolder;

		private void Awake()
		{
			Initialize();
		}

		private void Start()
		{
			grid.BuildGrid(BoundAssembly.grid);
		}

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			BoundAssembly = GetComponent<Assembly>();

			EventHandler.Bind(this);

			return Initialized = true;
		}

		public bool Trigger(Assembly caller)
		{
			return true;
		}

		public bool MoveCircuit(CircuitWidget circuitWidget, Vector2Int cell)
		{
			if (!BoundAssembly.MoveCircuit(circuitWidget.Circuit.BoundCircuit, cell))
				return false;

			PlaceCircuitOnGrid(circuitWidget, cell);
			return true;
		}

		public bool AddCircuit(CircuitWidget circuitWidget, Vector2Int cell)
		{
			if (!BoundAssembly.AddCircuit(circuitWidget.Circuit.BoundCircuit, cell))
				return false;

			PlaceCircuitOnGrid(circuitWidget, cell);
			return true;
		}

		public void PlaceCircuitOnGrid(CircuitWidget circuitWidget, Vector2Int cell)
		{
			circuitWidget.RectTransform.SetParent(cirucitHolder, false);

			// <TODO> Ivestigate why y-coordinate has to be inverted.
			Vector2 positionOnGrid = grid.GetCellWidget(cell).GetComponent<RectTransform>().anchoredPosition;
			positionOnGrid += (grid.RectTransform.rect.min * new Vector2Int(1, -1)) - circuitWidget.Circuit.OriginOffset;

			circuitWidget.RectTransform.anchoredPosition = positionOnGrid;
		}
	}
}
