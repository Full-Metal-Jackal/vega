using UnityEngine;

namespace UI
{
	public class HoverableGameObjectToggler : HoverableHighlight
	{
		public GameObject highlightGameObject;

		public override void SetHighlight(bool enabled) =>
			highlightGameObject.SetActive(enabled);
	}
}
