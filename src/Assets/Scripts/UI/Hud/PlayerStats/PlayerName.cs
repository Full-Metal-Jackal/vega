using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class PlayerName : MonoBehaviour
	{
		[SerializeField]
		private Text text;

		private void Start() =>
			PlayerController.Instance.OnPossesed += (player) => text.text = player.Name;
	}
}
