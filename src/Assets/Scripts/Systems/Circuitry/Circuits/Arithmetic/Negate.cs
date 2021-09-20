using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Negate : Circuit
	{
		private DataInput input;
		private DataOutput output;

		public override void Setup()
		{
			input = AddDataInput<Number>("In");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", DoNegate, "On computed");
		}

		protected bool DoNegate()
		{
			if (!UseFullPower())
				return false;

			if (!(input.Value is Number number))
				return false;

			output.Push((Number)(-number));
			Sleep(CooldownPerUse);
			return true;
		}
	}
}
