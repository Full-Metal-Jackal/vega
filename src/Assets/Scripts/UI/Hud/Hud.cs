using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
	[RequireComponent(typeof(CanvasScaler))]
	public sealed class Hud : MonoSingleton<Hud>
	{
		[SerializeField]
		private GameObject weaponSelector;

		// Unused yet, added for future reference
		private CanvasScaler canvasScaler;

		private void Awake()
		{
			canvasScaler = GetComponent<CanvasScaler>();
		}

		public void Toggle(bool toggle) => gameObject.SetActive(toggle);
	}
}
