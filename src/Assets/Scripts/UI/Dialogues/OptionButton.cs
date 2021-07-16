using DialogueEditor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Dialogue
{
	[RequireComponent(typeof(Button))]
	public class OptionButton : DialogueButton, IPointerEnterHandler
	{
		[SerializeField]
		private Image selector;

		[SerializeField]
		private TextMeshProUGUI textMesh;

		public override void Setup(ConversationNode node)
		{
			base.Setup(node);
			textMesh.text = node.Text;
			textMesh.font = node.TMPFont;
		}

		public void OnPointerEnter(PointerEventData eventData) =>
			DialogueWindow.Instance.SelectedOption = this;

		public void ToggleSelect(bool toggle)
		{
			selector.enabled = toggle;
			// <TODO> If true, play select sound here.
		}
	}
}
