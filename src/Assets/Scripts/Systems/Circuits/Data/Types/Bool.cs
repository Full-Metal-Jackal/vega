using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Bool : Data
	{
		public new static readonly string typeName = "BOOL";
		public bool Value { get; set; } = false;
		public Bool(bool boolean)
		{
			Value = boolean;
		}

		public static implicit operator bool(Bool boolean) => boolean.Value;
		public static implicit operator Bool(bool boolean) => new Bool(boolean);

		public static implicit operator Bool(Number number) => number.Equals(0);
		public static implicit operator Bool(Text text) => ((string)text).Length > 0;
		public static implicit operator Bool(List list) => ((List<Data>)list).Count > 0;
	}
}
