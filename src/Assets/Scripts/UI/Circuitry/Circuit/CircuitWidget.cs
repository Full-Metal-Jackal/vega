using UnityEngine;
using UnityEngine.UI;

using Circuitry;
using UnityEngine.EventSystems;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(CanvasGroup))]
	[RequireComponent(typeof(CircuitConnections))]
	public class CircuitWidget : DraggableCircuitWidget, ITriggerable<Circuitry.Circuit>
	{
		private CanvasGroup canvasGroup;

		[SerializeField]
		private GameObject cooldownOverlayPrefab;
		private CircuitCooldownOverlay cooldownOverlay;

		private CircuitConnections connections;

		protected override bool Initialize()
		{
			canvasGroup = GetComponent<CanvasGroup>();
			connections = GetComponent<CircuitConnections>();

			return base.Initialize();
		}

		public override void Setup(GameObject circuitPrefab)
		{
			base.Setup(circuitPrefab);

			CreateCooldownOverlay();
			connections.Setup(Circuit);
			EventHandler.Bind(this);
		}

		public override void OnBeginDrag(PointerEventData eventData)
		{
			base.OnBeginDrag(eventData);
			canvasGroup.blocksRaycasts = false;
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			base.OnEndDrag(eventData);
			canvasGroup.blocksRaycasts = true;
		}

		public void CreateCooldownOverlay()
		{
			if (cooldownOverlay)
			{
				Debug.LogWarning($"Multiple attempts to create a cooldown overlay for {this}.");
				return;
			}

			Circuit.Icon.gameObject.AddComponent<Mask>();
			GameObject cooldownOverlayObject = Instantiate(cooldownOverlayPrefab);
			if (!cooldownOverlayPrefab.TryGetComponent(out cooldownOverlay))
				throw new System.Exception($"{this} has invalid cooldown overlay prefab.");

			cooldownOverlayObject.transform.SetParent(Circuit.Icon.transform, false);
		}

		public bool Trigger(Circuitry.Circuit caller)
		{
			if (caller.IsSleeping)
				StartCooldownAnimation();

			return true;
		}

		public void StartCooldownAnimation()
		{
			cooldownOverlay.StartCooldownAnimation(Circuit.BoundCircuit.CooldownPerUse);
		}

		public override void DropOnAssembly(AssemblyWidget assemblyWidget, Vector2Int cell)
		{
			assemblyWidget.MoveCircuit(this, cell);
		}
	}
}
