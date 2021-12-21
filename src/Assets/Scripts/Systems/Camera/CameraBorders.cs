using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorders : MonoSingleton<CameraBorders>
{
	[field: SerializeField]
	public Vector3 Min { get; private set; }

	[field: SerializeField]
	public Vector3 Max { get; private set; }

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow + Color.red;
		Gizmos.DrawWireCube((Max + Min) * .5f, Max - Min);
		Gizmos.DrawWireSphere(Min, .5f);
		Gizmos.DrawWireSphere(Max, .5f);
	}
}
