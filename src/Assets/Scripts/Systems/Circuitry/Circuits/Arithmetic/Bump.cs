using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Bump : Circuit
	{
		private readonly DataInput<Number> input;
		private readonly DataOutput<Number> output;

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
			int number = input.Value;
			output.Push(++number);
			return true;
		}
	}
}
