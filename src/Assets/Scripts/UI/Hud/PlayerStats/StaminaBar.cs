using UnityEngine;

namespace UI
{
	public class StaminaBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;
		
		private Mob player;

		private void Start() =>
			player = Game.PlayerController.Possessed;
		
		private void Update() =>
			rectTransform.localScale = new Vector3(player.Stamina / player.MaxStamina, 1.0f, 1.0f);
	}
}
