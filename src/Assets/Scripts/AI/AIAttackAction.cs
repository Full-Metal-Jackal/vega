using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{

	[CreateAssetMenu(menuName ="A.I/AI Actions/Attack Action")]
	public class AIAttackAction : AIAction
	{
		public string attackName = "Attack";
		public int attackScore = 3;
		public float recoveryTime = 2;

		public float maximumAttackAngle = 35;
		public float minimumAttackAngle = -35;

		public float minimumDistanceNeededToAttack = 0;
		public float maximumDistanceNeededToAttack = 2;
	}
}

