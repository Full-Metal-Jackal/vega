using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
	[RequireComponent(typeof(CanvasScaler))]
	public sealed class Hud : MonoSingleton<Hud>
	{
		public void Toggle(bool toggle) => gameObject.SetActive(!Game.PlayingScene && toggle);
	}
}
