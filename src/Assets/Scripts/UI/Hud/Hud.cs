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
		
		private bool componentsActive = false;

		private void Awake()
		{
			canvasScaler = GetComponent<CanvasScaler>();
			ToggleComponents(false);
		}

		/// <summary>
		///	Registers (activates) components which are inactive by default
		///	<para>NOTE: might as well change this to be a method to toggle hud</para>
		/// </summary>
		public void RegisterComponents()
		{
			if (!componentsActive)
				ToggleComponents(true);
		}

		private void ToggleComponents(bool toggle)
		{
			playerStats.SetActive(toggle);
			weaponSelector.SetActive(toggle);

			componentsActive = toggle;
		}
	}
}
