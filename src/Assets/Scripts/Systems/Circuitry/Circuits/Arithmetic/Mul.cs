using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Mul : Circuit
	{
		private DataInput multiplicand;
		private DataInput multiplier;
		private DataOutput output;

		public override void Setup()
		{
			Shape = Shape.Rect(2, 1);

			multiplicand = AddDataInput<Number>("Multiplicand");
			multiplier = AddDataInput<Number>("Multiplier");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", Multiply, "On computed");
		}

		protected bool Multiply()
		{
			if (!UseFullPower())
				return false;

			if (!(multiplicand.Value is Number numberA && multiplier.Value is Number numberB))
				return false;

			output.Push((Number)(numberA * numberB));
			Sleep(CooldownPerUse);
			return true;
		}
	}
}
