using UnityEngine;
using TMPro;

namespace UI.HUD
{
	public class PlayerName : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text text;

		private void Awake() =>
			PlayerController.Instance.OnPossessed += player => text.text = player.Name;
	}
}
