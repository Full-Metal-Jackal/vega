using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Number : Data
	{
		public new static readonly string typeName = "NUMBER";
		public float Value { get; set; } = 0f;
		public Number(float number = 0f)
		{
			Value = number;
		}

		public static implicit operator int(Number number) => (int)number.Value;
		public static implicit operator Number(int number) => number;
		public static implicit operator float(Number number) => number.Value;
		public static implicit operator Number(float number) => number;

		public static implicit operator Number(Bool boolean) => new Number(boolean ? 1 : 0);
		public static implicit operator Number(Text text) => Single.Parse(text);
	}
}
