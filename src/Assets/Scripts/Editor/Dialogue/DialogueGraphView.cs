using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

namespace EditorTools.DialogueGraph
{
	public class DialogueGraphView : GraphView
	{
		EntryDialogueNode entryNode;
		ExitDialogueNode exitNode;

		public IEnumerable<Edge> Edges => edges.ToList().Where(x => x.input.node != null);
		public IEnumerable<DialogueGraphNode> Nodes => nodes.ToList().Cast<DialogueGraphNode>();

		public DialogueGraphView()
		{
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			Setup();
		}

		public void Setup()
		{
			AddEntryNode().SetPosition(new Rect(128, 128, 0, 0));
			AddExitNode().SetPosition(new Rect(256, 128, 0, 0));
		}

		private T AddNode<T>(string line) where T : DialogueGraphNode, new()
		{
			T node = new T()
			{
				title = line,
				line = line
			};

			AddElement(node);
			return node;
		}

		public EntryDialogueNode AddEntryNode()
		{
			if (!(entryNode is null))
			{
				Debug.LogWarning("There can be only one dialogue entry.");
				return null;
			}
			return entryNode = AddNode<EntryDialogueNode>("ENTRY");
		}

		public ExitDialogueNode AddExitNode()
		{
			if (!(exitNode is null))
			{
				Debug.LogWarning("There can be only one dialogue exit.");
				return null;
			}
			return exitNode = AddNode<ExitDialogueNode>("EXIT");
		}

		public DialogueNode AddNode(string line = "") =>
			AddNode<DialogueNode>(line);

		public BranchingDialogueNode AddBranchingNode(string line = "") =>
			AddNode<BranchingDialogueNode>(line);

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			List<Port> compatiblePorts = new List<Port>();

			ports.ForEach(
				(port) =>
				{
					if (startPort.direction != port.direction
						&& startPort.node != port.node
					)
						compatiblePorts.Add(port);
				}
			);

			return compatiblePorts;
		}

		public void ClearElements()
		{
			foreach (Node node in Nodes)
				RemoveElement(node);
		}
	}
}
