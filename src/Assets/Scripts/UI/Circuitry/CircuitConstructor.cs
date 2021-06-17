using UnityEngine;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class CircuitConstructor : MonoBehaviour
	{
		[SerializeField]
		private ConstructorViewport viewport;
		public ConstructorViewport Viewport => viewport;

		[SerializeField]
		private AssemblyWidget assemblyWidget;
		public AssemblyWidget AssemblyWidget => assemblyWidget;

		public bool Initialized { get; private set; }

		public bool IsOpened => gameObject.activeInHierarchy;

		private void Awake()
		{
			Initialize();
		}

		protected virtual bool Initialize()
		{
			if (Game.circuitConstructor)
				throw new System.Exception($"Multiple instances of circuit constructor detected: {this}, {Game.circuitConstructor}");
			Game.circuitConstructor = this;

			if (!viewport)
				throw new System.Exception("No viewport assigned to the circuit constructor.");

			return Initialized = true;
		}

		private void Start()
		{
			Setup();
		}

		public void Setup()
		{
			// <TODO> Remove this as soon as we implement OpenAssembly.
			viewport.Zoom = .7f;

			gameObject.SetActive(false);
		}

		public void Open()
		{
			Debug.Log("Opening the circuit constructor...");
			gameObject.SetActive(true);
			Game.inputState = InputState.UIOnly;
		}

		public void Open(AssemblyWidget assemblyWidget)
		{
			Open();

			throw new System.NotImplementedException();

			// OpenAssembly(assemblyWidget);
		}

		public void OpenAssembly(AssemblyWidget assemblyWidget)
		{
			this.assemblyWidget = assemblyWidget;

			viewport.minZoom = assemblyWidget.minZoom;
			viewport.Zoom = assemblyWidget.preferedZoom;

			throw new System.NotImplementedException();
		}

		public void Close()
		{
			Debug.Log("Closing the circuit constructor...");
			gameObject.SetActive(false);
			Game.inputState = InputState.WorldOnly;
		}

		public void ShowCircuitInfo(Circuit circuit)
		{
			Debug.Log($"Information about {circuit} should be shown now.");
		}
	}
}
