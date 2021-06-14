using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Bump : Circuit
	{
		private DataInput input;
		private DataOutput output;

		public override void Setup()
		{
			input = AddDataInput<Number>("In");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", DoBump, "On computed");
		}

		protected bool DoBump()
		{
			if (!UseFullPower())
				return false;

			if (!(input.Value is Number number))
				return false;

			output.Push(++number);
			Sleep(CooldownPerUse);
			return true;
		}
	}
}
