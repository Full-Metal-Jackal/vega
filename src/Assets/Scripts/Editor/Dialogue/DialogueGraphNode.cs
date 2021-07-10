using System;
using UnityEditor.Experimental.GraphView;

namespace EditorTools.DialogueGraph
{
	/// <summary>
	/// Abstract node structure for hierarchical convenience.
	/// Inherited by both utility and actual dialogue nodes.
	/// </summary>
	public abstract class DialogueGraphNode : Node
	{
		/// <summary>
		/// Guid needed for serialization.
		/// </summary>
		public Guid Guid { get; private set; }

		public DialogueGraphNode()
		{
			Guid = Guid.NewGuid();
		}

		/// <summary>
		/// The character's line.
		/// </summary>
		public string line = "";

		protected virtual Port AddOutput(string name = "Out") =>
			AddPort(name, Direction.Output, Port.Capacity.Single);
		protected virtual Port AddInput(string name = "In") =>
			AddPort(name, Direction.Input, Port.Capacity.Multi);

		private Port AddPort(string name, Direction direction, Port.Capacity capacity)
		{
			Port port = InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(int));
			port.portName = name;
			
			outputContainer.Add(port);
			RefreshExpandedState();
			RefreshPorts();

			return port;
		}
	}
}
