using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Represents in-game data blocks.
	/// </summary>
	public class Data
	{
		public static readonly string typeName = "ANY";

		public static explicit operator Data(int number) => new Number(number);
		public static explicit operator Data(string text) => new Data();
		public static explicit operator Data(List<Data> text) => new Data();
	}
}
