using UnityEditor.Experimental.GraphView;

namespace EditorTools.DialogueGraph
{
	/// <summary>
	/// A straightforward node used to separate lines of a linear dialogue.
	/// </summary>
	public class DialogueNode : DialogueGraphNode
	{
		public DialogueNode()
		{
			AddInput();
			AddOutput();
		}
	}
}
