using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Circuitry
{
	public class Izar : DeviceAssembly
	{
		public override void Setup()
		{
			label = "Izar assembly";
			grid.CreateEmpty(Shape.Rect(10, 4));
		}
	}
}
