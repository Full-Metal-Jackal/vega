using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace EditorTools.DialogueGraph
{
	public class BranchingDialogueNode : DialogueNode
	{
		public BranchingDialogueNode()
		{
			SetupButtons();
		}

		public void SetupButtons()
		{
			titleContainer.Add(new Button(() => AddOutput())
			{
				text = "Add Option"
			});
		}
	}
}
