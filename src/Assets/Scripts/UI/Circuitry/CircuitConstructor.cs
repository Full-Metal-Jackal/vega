using UnityEngine;

namespace UI.CircuitConstructor
{
	public class CircuitConstructor : MonoSingleton<CircuitConstructor>
	{
		[field: SerializeField]
		public ConstructorViewport Viewport { get; private set; }

		[field: SerializeField]
		public AssemblyWidget AssemblyWidget { get; private set; }

		public Vector3 GridScale => AssemblyWidget.Grid.transform.lossyScale;

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
			gameObject.SetActive(false);
		}

		public void Open()
		{
			Debug.Log("Opening the circuit constructor...");
			gameObject.SetActive(true);
			Game.State = GameState.Paused;
		}

		public void Open(AssemblyWidget assemblyWidget)
		{
			Open();
			OpenAssembly(assemblyWidget);
		}

		public void OpenAssembly(AssemblyWidget assemblyWidget)
		{
			AssemblyWidget = assemblyWidget;

			Viewport.minZoom = assemblyWidget.minZoom;
			Viewport.Zoom = assemblyWidget.preferedZoom;
		}

		public void Close()
		{
			Debug.Log("Closing the circuit constructor...");
			gameObject.SetActive(false);
			Game.State = GameState.Normal;
		}

		public void ShowCircuitInfo(Circuitry.Circuit circuit)
		{
			Debug.Log($"Information about {circuit} should be shown now.");
		}
	}
}
