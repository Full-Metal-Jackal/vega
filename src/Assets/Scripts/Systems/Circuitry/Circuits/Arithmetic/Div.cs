using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Div : Circuit
	{
		private DataInput divident;
		private DataInput divisor;
		private DataOutput output;

		public override void Setup()
		{
			Shape = Shape.Rect(2, 1);

			divident = AddDataInput<Number>("Dividend");
			divisor = AddDataInput<Number>("Divisor");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", Multiply, "On computed");
		}

		protected bool Multiply()
		{
			if (!UseFullPower())
				return false;

			if (!(divident.Value is Number numberA && divisor.Value is Number numberB))
				return false;

			output.Push((Number)(numberA / numberB));
			Sleep(CooldownPerUse);
			return true;
		}
	}
}
