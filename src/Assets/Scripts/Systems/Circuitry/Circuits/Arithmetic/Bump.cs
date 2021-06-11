using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Bump : Circuit
	{
		private readonly DataInput input;
		private readonly DataOutput output;

		public Bump() : base()
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
			return true;
		}
	}
}
