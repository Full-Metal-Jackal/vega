using UnityEngine;

namespace UI.CircuitConstructor
{
	public class CircuitConstructor : MonoSingleton<CircuitConstructor>
	{
		[field: SerializeField]
		public ConstructorViewport Viewport { get; private set; }
		public Vector2 ViewportScale => Viewport.Content.localScale;

		[field: SerializeField]
		public AssemblyWidget AssemblyWidget { get; private set; }

		public bool IsOpened => gameObject.activeInHierarchy;

		private void Awake()
		{
			Initialize();
		}

		protected virtual void Initialize()
		{
			if (!Viewport)
				throw new System.Exception("No viewport assigned to the circuit constructor.");
		}

		private void Start()
		{
			Setup();
		}

		public void Setup()
		{
			// <TODO> Remove this as soon as we implement OpenAssembly.
			Viewport.Zoom = .7f;

			gameObject.SetActive(false);
		}

		public void Open()
		{
			Debug.Log("Opening the circuit constructor...");
			gameObject.SetActive(true);
			Game.InputState = InputState.UIOnly;
		}

		public void Open(AssemblyWidget assemblyWidget)
		{
			Open();

			throw new System.NotImplementedException();

			// OpenAssembly(assemblyWidget);
		}

		public void OpenAssembly(AssemblyWidget assemblyWidget)
		{
			AssemblyWidget = assemblyWidget;

			Viewport.minZoom = assemblyWidget.minZoom;
			Viewport.Zoom = assemblyWidget.preferedZoom;

			throw new System.NotImplementedException();
		}

		public void Close()
		{
			Debug.Log("Closing the circuit constructor...");
			gameObject.SetActive(false);
			Game.InputState = InputState.WorldOnly;
		}

		public void ShowCircuitInfo(Circuitry.Circuit circuit)
		{
			Debug.Log($"Information about {circuit} should be shown now.");
		}
	}
}
