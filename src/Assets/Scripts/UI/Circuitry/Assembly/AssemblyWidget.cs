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
		public bool Initialized { get; private set; } = false;

		[SerializeField]
		private ConstructorGrid grid;

		private void Awake()
		{
			Initialize();
		}

		private void Start()
		{
			grid.BuildGrid(BoundAssembly.grid);
		}

		private bool Initialize()
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
	}
}
