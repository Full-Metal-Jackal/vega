using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public interface IPowerSource
	{
		public float Capacity { get; set; }
		public OutputTerminal PowerOutput { get; set; }
		public float Withdraw(float amount);
	}
}
