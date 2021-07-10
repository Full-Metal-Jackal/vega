using System;
using UnityEngine;

namespace EditorTools.DialogueGraph
{
	[Serializable]
	public class DialogueLinkData : ScriptableObject
	{
		public Guid source;
		public Guid target;

		public string port;

		public void Setup(Guid source, Guid target, string port)
		{
			this.source = source;
			this.target = target;

			this.port = port;
		}
	}
}
