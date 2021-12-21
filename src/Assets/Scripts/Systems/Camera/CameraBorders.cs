using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorders : MonoSingleton<CameraBorders>
{
	[SerializeField]
	private Transform min;
	public Vector3 Min => min.position;

	[SerializeField]
	private Transform max;
	public Vector3 Max => max.position;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow + Color.red;
		Gizmos.DrawWireCube((Max + Min) * .5f, Max - Min);
		Gizmos.DrawWireSphere(Min, .5f);
		Gizmos.DrawWireSphere(Max, .5f);
	}
}
