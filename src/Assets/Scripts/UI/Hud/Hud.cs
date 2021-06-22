using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(CanvasScaler))]
	public class Hud : MonoBehaviour
	{
		[field: SerializeField]
		public HealthBar HealthBar { get; private set; }

		[field: SerializeField]
		public StaminaBar StaminaBar { get; private set; }

		[field: SerializeField]
		public PlayerName PlayerName { get; private set; }

		// Unused yet, added for future reference
		private CanvasScaler canvasScaler;
		
		private bool componentsActive = false;

		private void Awake()
		{
			canvasScaler = GetComponent<CanvasScaler>();
			
			Initialize();
		}

		protected virtual void Initialize()
		{
			if (Game.hud)
				throw new Exception($"Multiple instances of HUD detected: {this}, {Game.hud}");

			Game.hud = this;
		}

		private void Start() =>
			canvasScaler.gameObject.SetActive(true);

		/// <summary>
		///	Registers (activates) components which are inactive by default
		///	<para>NOTE: might as well change this to be a method to toggle hud</para>
		/// </summary>
		public void RegisterComponents()
		{
			if (componentsActive) {
				return;
			}

			HealthBar.gameObject.SetActive(true);
			StaminaBar.gameObject.SetActive(true);
			PlayerName.gameObject.SetActive(true);
		}
	}
}
