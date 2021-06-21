using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Sub : Circuit
	{
		private DataInput minuend;
		private DataInput substrahend;
		private DataOutput output;

		public override void Setup()
		{
			Shape = Shape.Rect(1, 2);

			minuend = AddDataInput<Number>("Minuend");
			substrahend = AddDataInput<Number>("Subtrahend");
			output = AddDataOutput<Number>("Out");

			AddPulsePipeline("Compute", Substract, "On computed");
		}

		protected bool Substract()
		{
			if (!UseFullPower())
				return false;

			if (!(minuend.Value is Number numberA && substrahend.Value is Number numberB))
				return false;

			output.Push((Number)(numberA - numberB));
			Sleep(CooldownPerUse);
			return true;
		}
	}
}
