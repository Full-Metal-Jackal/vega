using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Sign : Circuit
	{
		private DataInput input;
		private DataOutput output;

		public override void Setup()
		{
			input = AddDataInput<Number>("In");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", CalculateSign, "On computed");
		}

		protected bool CalculateSign()
		{
			if (!UseFullPower())
				return false;

			if (!(input.Value is Number number))
				return false;

			output.Push((Number)Mathf.Sign(number));
			Sleep(CooldownPerUse);
			return true;
		}
	}
}
