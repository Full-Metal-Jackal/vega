using System;
using System.Collections.Generic;
using UnityEngine;

namespace EditorTools.DialogueGraph
{
	[Serializable]
	public class DialogueData : ScriptableObject
	{
		private readonly HashSet<DialogueNodeData> nodes = new HashSet<DialogueNodeData>();
		public HashSet<DialogueNodeData> Nodes => new HashSet<DialogueNodeData>(nodes);

		private readonly HashSet<DialogueLinkData> links = new HashSet<DialogueLinkData>();
		public HashSet<DialogueLinkData> Links => new HashSet<DialogueLinkData>(links);

		public void AddNode(DialogueNodeData node) => nodes.Add(node);
		public void AddLink(DialogueLinkData link) => links.Add(link);
	}
}
