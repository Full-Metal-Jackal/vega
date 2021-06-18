using UnityEngine;
using UnityEngine.UI;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class CircuitWidget : DraggableCircuitWidget, ITriggerable<Circuitry.Circuit>
	{
		[SerializeField]
		private GameObject cooldownOverlayPrefab;
		private CircuitCooldownOverlay cooldownOverlay;

		protected override void Setup()
		{
			CreateCooldownOverlay();
		}

		public void CreateCooldownOverlay()
		{
			if (cooldownOverlay)
			{
				Debug.LogWarning($"Multiple attempts to create a cooldown overlay for {this}.");
				return;
			}

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
			assemblyWidget.MoveCircuit(Circuit, cell);
		}
	}
}
