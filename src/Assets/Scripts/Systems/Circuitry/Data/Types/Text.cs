using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Text : Data
	{
		public new static readonly string typeName = "TEXT";
		public string Value { get; set; } = "";

		public Text(string text)
		{
			Value = text;
		}

		public static implicit operator string(Text text) => text.Value;
		public static implicit operator Text(string text) => new Text(text);

		public static implicit operator Text(Number number) => number.ToString();
		public static implicit operator Text(Bool boolean) => boolean;
	}
}
