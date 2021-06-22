using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class PlayerName : MonoBehaviour
	{
		[SerializeField]
		private Text text;
		
		private Mob player;

		private void Awake() =>
			gameObject.SetActive(false);

		private void Start() =>
			player = Game.playerController.Possessed;
		
		private void Update() =>
			text.text = player.Name;
	}
}
