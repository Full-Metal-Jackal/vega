using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
	public class PlayerPortait : MonoBehaviour
	{
		[SerializeField]
		private Image image;

	//	private void Awake() =>
	//		PlayerController.Instance.OnPossessed += (player) =>
	//		{
	//			image.sprite = player.PersonData.mediumPortrait;
	//		};
	}
}
