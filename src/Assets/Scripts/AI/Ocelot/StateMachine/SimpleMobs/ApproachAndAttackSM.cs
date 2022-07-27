using OcelotAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachAndAttackSM : AIStateMachine
{
	[SerializeField]
	private float maxAttackDistance = 10f;

	[SerializeField]
	private AIBehaviour approachBehaviour;

	[SerializeField]
	private AIBehaviour attackBehaviour;

	protected override void Start()
	{
		SetBehaviourSequence(attackBehaviour, approachBehaviour);
	}

	public override void UpdateBehaviour()
	{
		base.UpdateBehaviour();

		if (ActiveBehaviour == null)
			ActiveBehaviour = approachBehaviour;

		bool canSwitchToAttack = ActiveBehaviour != attackBehaviour
			&& approachBehaviour.Complete
			&& attackBehaviour.Available
			&& Controller.CanShootTarget
			&& Vector3.Distance(
				Controller.Target.transform.position,
				Controller.Possessed.transform.position
			) < maxAttackDistance;

		if (canSwitchToAttack)
			ActiveBehaviour = attackBehaviour;
	}
}
