using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Circuitry;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(Assembly))]
	public class AssemblyWidget : MonoBehaviour
	{
		public Assembly Assembly { get; private set; }
		public bool Initialized { get; protected set; } = false;

		public float minZoom = .6f;
		public float preferedZoom = .71f;

		[field: SerializeField]
		public AssemblyGrid Grid { get; private set; }

		[SerializeField]
		private RectTransform cirucitHolder;

		private void Awake()
		{
			Initialize();
		}

		private void Start()
		{
			Grid.BuildGrid(Assembly.grid);
		}

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			Assembly = GetComponent<Assembly>();

			return Initialized = true;
		}

		public bool MoveCircuit(CircuitWidget circuitWidget, Vector2Int cell)
		{
			if (!Assembly.MoveCircuit(circuitWidget.Circuit.Circuit, cell))
				return false;

			PlaceCircuitOnGrid(circuitWidget, cell);

			return true;
		}

		public bool AddCircuit(CircuitWidget circuitWidget, Vector2Int cell)
		{
			if (!Assembly.AddCircuit(circuitWidget.Circuit.Circuit, cell))
				return false;

			PlaceCircuitOnGrid(circuitWidget, cell);
			return true;
		}

		public void PlaceCircuitOnGrid(CircuitWidget circuitWidget, Vector2Int cell)
		{
			circuitWidget.RectTransform.SetParent(cirucitHolder, false);

			Vector2 positionOnGrid = Grid.GetCellWidget(cell).GetComponent<RectTransform>().anchoredPosition;
			positionOnGrid += Grid.RectTransform.rect.min * new Vector2Int(1, -1);
			positionOnGrid -= circuitWidget.Circuit.OriginOffset * CircuitConstructor.Instance.AssemblyWidget.Grid.transform.localScale;

			circuitWidget.RectTransform.anchoredPosition = positionOnGrid;
		}
	}
}
