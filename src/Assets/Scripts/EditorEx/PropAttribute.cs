using UnityEngine;

namespace EditorEx
{
	public class PropAttribute : PropertyAttribute
	{
		public string Name { get; set; }
		public bool ReadOnly { get; set; }

		public PropAttribute() { }
	}
}
