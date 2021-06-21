using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Abs : Circuit
	{
		private DataInput input;
		private DataOutput output;

		public override void Setup()
		{
			input = AddDataInput<Number>("In");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", CalculateAbsoluteValue, "On computed");
		}

		protected bool CalculateAbsoluteValue()
		{
			if (!UseFullPower())
				return false;

			if (!(input.Value is Number number))
				return false;

			output.Push((Number)Mathf.Abs(number));
			Sleep(CooldownPerUse);
			return true;
		}
	}
}
