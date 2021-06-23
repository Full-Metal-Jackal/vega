using UnityEngine;

namespace UI
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;
		
		private Mob player;

		private void Start() =>
			player = PlayerController.Instance.Possessed;
		
		private void Update() =>
			rectTransform.localScale = new Vector3(player.Health / player.MaxHealth, 1.0f, 1.0f);
	}
}
