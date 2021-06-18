using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class SumOfTwo : Circuit
	{
		private DataInput inputA;
		private DataInput inputB;
		private DataOutput output;

		public override void Setup()
		{
			Shape = Shape.Rect(1, 2);

			inputA = AddDataInput<Number>("A");
			inputB = AddDataInput<Number>("B");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", Sum, "On computed");
		}

		protected bool Sum()
		{
			if (!UseFullPower())
				return false;

			if (!(inputA.Value is Number numberA && inputB.Value is Number numberB))
				return false;

			output.Push((Number)(numberA + numberB));
			Sleep(CooldownPerUse);
			return true;
		}
	}
}
