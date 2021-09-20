using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Mod : Circuit
	{
		private DataInput inputA;
		private DataInput inputB;
		private DataOutput output;

		public override void Setup()
		{
			Shape = Shape.Rect(2, 1);

			inputA = AddDataInput<Number>("Dividend");
			inputB = AddDataInput<Number>("Divisor");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", CalculateMod, "On computed");
		}

		protected bool CalculateMod()
		{
			if (!UseFullPower())
				return false;

			if (!(inputA.Value is Number numberA && inputB.Value is Number numberB))
				return false;

			if (numberB != 0)
				output.Push((Number)(numberA % numberB));
			else
				Logging.Log($"{this}: zero division attempt discarded.");

			Sleep(CooldownPerUse);
			return true;
		}
	}
}
