using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Log : Circuit
	{
		private DataInput input;
		private DataInput logBase;
		private DataOutput output;

		public override void Setup()
		{
			Shape = Shape.Rect(1, 2);

			input = AddDataInput<Number>("Number");
			logBase = AddDataInput<Number>("Base");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", CalculateLogarithm, "On computed");
		}

		protected bool CalculateLogarithm()
		{
			if (!UseFullPower())
				return false;

			if (!(input.Value is Number numberA && logBase.Value is Number numberB))
				return false;

			output.Push((Number)Mathf.Log(numberA, numberB));
			Sleep(CooldownPerUse);
			return true;
		}
	}
}
