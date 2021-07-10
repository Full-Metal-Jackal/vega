using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace EditorTools.DialogueGraph
{
	public static class DialogueAssetManager
	{
		private static readonly string defaultFolder = "Assets/Dialogues";
		private static readonly string extension = "asset";

		public static void Save(DialogueGraphView view, string fileName)
		{
			DialogueData dialogueData = ScriptableObject.CreateInstance<DialogueData>();

			foreach (Edge edge in view.Edges)
			{
				Port input = edge.input;
				Port output = edge.output;
				DialogueLinkData data = ScriptableObject.CreateInstance<DialogueLinkData>();
				data.Setup(
					(input.node as DialogueGraphNode).Guid,
					(output.node as DialogueGraphNode).Guid,
					edge.output.portName
				);
				dialogueData.AddLink(data);
			}

			foreach (DialogueGraphNode node in view.Nodes)
			{
				DialogueNodeData data = ScriptableObject.CreateInstance<DialogueNodeData>();
				data.Setup(node.Guid, node.GetPosition().position, node.line);
				dialogueData.AddNode(data);
			}

			string folder = defaultFolder;
			if (!AssetDatabase.IsValidFolder(folder))
				folder = "Assets";
			// <TODO> There'll be a chic dialogue window later, trust me.

			AssetDatabase.CreateAsset(dialogueData, $"{folder}/{fileName}.{extension}");
			AssetDatabase.SaveAssets();
		}

		public static void Load(DialogueGraphView view, string fileName)
		{
			// <TODO> There'll be a chic dialogue window later, trust me.
			string path = $"{defaultFolder}/{fileName}.{extension}";
			if (!(Resources.Load<DialogueData>(path) is DialogueData data))
			{
				Debug.Log($"File {path} doesn't exist.");
				return;
			}

			view.ClearElements();

			foreach (DialogueNodeData nodeData in data.Nodes)
			{
				DialogueGraphNode node = view.AddNode(nodeData.line);
				// node.Guid = nodeData.guid;
			}
		}
	}
}
