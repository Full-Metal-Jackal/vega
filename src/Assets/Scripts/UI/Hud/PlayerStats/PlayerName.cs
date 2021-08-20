using UnityEngine;
using TMPro;

namespace UI.HUD
{
	public class PlayerName : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text text;

		private void Start() =>
			PlayerController.Instance.OnPossessed += player => text.text = player.Name;
	}
}
