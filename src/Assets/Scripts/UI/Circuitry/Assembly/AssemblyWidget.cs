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

		public void MoveCircuit(Circuit circuit, Vector2Int cell)
		{
			if (!BoundAssembly.MoveCircuit(circuit.BoundCircuit, cell))
				return;


		}

		public void AddCircuit(Circuit circuit, Vector2Int cell)
		{
			if (!BoundAssembly.AddCircuit(circuit.BoundCircuit, cell))
				return;


		}
	}
}
