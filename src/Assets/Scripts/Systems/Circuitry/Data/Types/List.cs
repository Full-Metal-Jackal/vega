using UnityEngine;

namespace Circuitry
{
	public class List : Data
	{
		public override string TypeName => "LIST";
		public System.Collections.Generic.List<Data> Value { get; private set; } = new System.Collections.Generic.List<Data>();
		public List(System.Collections.Generic.IEnumerable<Data> list)
		{
			Value = (System.Collections.Generic.List<Data>)list;
		}

		public static implicit operator System.Collections.Generic.List<Data>(List list) => list.Value;

		public static implicit operator List(System.Collections.Generic.List<Data> list) => new List(list);

		public override string ToString() => $"[{string.Join(", ", Value)}]";
	}
}

