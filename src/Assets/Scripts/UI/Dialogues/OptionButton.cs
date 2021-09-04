using DialogueEditor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Dialogue
{
	[RequireComponent(typeof(Button))]
	public class OptionButton : DialogueButton, IPointerEnterHandler, IPointerExitHandler
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
		
		public void OnPointerExit(PointerEventData eventData)
		{
			if (DialogueWindow.Instance.SelectedOption == this) // in case of a conflict of precedence of these methods
				DialogueWindow.Instance.SelectedOption = null;
		}

		public void ToggleSelect(bool toggle)
		{
			selector.enabled = toggle;
			// <TODO> If true, play select sound here.
		}
	}
}
