using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(CanvasScaler))]
	public sealed class Hud : MonoSingleton<Hud>
	{
		[SerializeField]
		private GameObject playerStats, weaponSelector;

		// Unused yet, added for future reference
		private CanvasScaler canvasScaler;

		private bool componentsActive = true;

		protected override void Awake()
		{
			canvasScaler = GetComponent<CanvasScaler>();
		}

		// NOTE: currently unused, reserved for future applications (e.g. toggling hud)
		private void ToggleComponents(bool toggle)
		{
			playerStats.SetActive(toggle);
			weaponSelector.SetActive(toggle);

			componentsActive = toggle;
		}

		public void Toggle(bool toggle) => gameObject.SetActive(toggle);
	}
}
