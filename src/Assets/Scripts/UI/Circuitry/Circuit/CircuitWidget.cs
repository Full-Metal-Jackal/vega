using UnityEngine;
using UnityEngine.UI;

using Circuitry;
using UnityEngine.EventSystems;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(CanvasGroup))]
	[RequireComponent(typeof(CircuitPorts))]
	public class CircuitWidget : DraggableCircuitWidget, ITriggerable<Circuitry.Circuit>
	{
		private CanvasGroup canvasGroup;

		[SerializeField]
		private GameObject cooldownOverlayPrefab;
		private CircuitCooldownOverlay cooldownOverlay;

		public CircuitPorts Ports { get; private set; }

		protected override bool Initialize()
		{
			canvasGroup = GetComponent<CanvasGroup>();
			Ports = GetComponent<CircuitPorts>();

			return base.Initialize();
		}

		public override void Setup(GameObject circuitPrefab)
		{
			base.Setup(circuitPrefab);

			CreateCooldownOverlay();
			Ports.Setup(Circuit);
			EventHandler.Bind(this);
		}

		public override void PostBeginDrag(PointerEventData eventData)
		{
			canvasGroup.blocksRaycasts = false;
		}

		public override void PostEndDrag(PointerEventData eventData)
		{
			canvasGroup.blocksRaycasts = true;
		}

		public void CreateCooldownOverlay()
		{
			if (cooldownOverlay)
			{
				Debug.LogWarning($"Multiple attempts to create a cooldown overlay for {this}.");
				return;
			}

			GameObject cooldownOverlayObject = Instantiate(cooldownOverlayPrefab);
			if (!cooldownOverlayObject.TryGetComponent(out cooldownOverlay))
			{
				Destroy(cooldownOverlayObject);
				throw new System.Exception($"{this} has invalid cooldown overlay prefab.");
			}
			
			Circuit.Icon.gameObject.AddComponent<Mask>();
			cooldownOverlayObject.transform.SetParent(Circuit.Icon.transform, false);
		}

		public bool Trigger(Circuitry.Circuit caller, string eventLabel)
		{
			switch (eventLabel)
			{
			case "cooldown":
				if (caller.IsSleeping)
					StartCooldownAnimation();
				break;
			default:
				Debug.LogWarning($"{this} encountered unsupported event: {eventLabel}");
				return false;
			}

			return true;
		}

		public void StartCooldownAnimation()
		{
			cooldownOverlay.StartCooldownAnimation(Circuit.BoundCircuit.CooldownPerUse);
		}

		public override void DropOnAssembly(AssemblyWidget assemblyWidget, Vector2Int cell)
		{
			if (!assemblyWidget.MoveCircuit(this, cell))
				return;

			foreach (PinWidget pin in Ports.Pins)
				pin.UpdateLines();
		}
	}
}
