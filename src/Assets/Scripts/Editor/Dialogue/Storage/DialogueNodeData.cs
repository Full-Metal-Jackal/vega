using System;
using UnityEngine;

namespace EditorTools.DialogueGraph
{
	[Serializable]
	public class DialogueNodeData : ScriptableObject
	{
		public Guid guid;
		public Vector2 position;

		public string line;

		public void Setup(Guid guid, Vector2 position, string line)
		{
			this.guid = guid;
			this.position = position;

			this.line = line;
		}
	}
}
