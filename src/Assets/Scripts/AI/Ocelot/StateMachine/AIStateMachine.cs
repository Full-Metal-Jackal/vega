using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OcelotAI
{
	/// <summary>
	/// Mob's behaviour selector.
	/// Chooses what behaviour the controller's mob should take.
	/// Every class of mob should have its own selector class.
	/// </summary>
	public class AIStateMachine : MonoBehaviour
	{
		[field: SerializeField]
		public AIController Controller { get; private set; }

		private AIBehaviour[] _behaviours;
		public AIBehaviour[] Behaviours
		{
			get
			{
				AIBehaviour[] copy = new AIBehaviour[_behaviours.Length];
				_behaviours.CopyTo(copy, 0);

				return copy;
			}
			private set =>
				_behaviours = value;
		}

		private AIBehaviour _activeBehaviour;
		public AIBehaviour ActiveBehaviour
		{
			get => _activeBehaviour;
			protected set
			{
				_activeBehaviour = value;

				foreach (AIBehaviour behaviour in Behaviours)
					behaviour.Active = behaviour == _activeBehaviour;
			}
		}

		protected virtual void Awake()
		{
			AIBehaviour[] behaviours = GetComponentsInChildren<AIBehaviour>(includeInactive: true);
			
			Behaviours = behaviours;
			if (behaviours.Length == 0)
				Debug.LogWarning($"{Controller} has empty state machine.");
		}

		protected virtual void Start()
		{
			AIBehaviour[] behaviours = Behaviours;

			if (behaviours.Length > 0)
				ActiveBehaviour = behaviours[0];
		}

		public virtual void UpdateBehaviour()
		{
		}

		protected void SetBehaviourSequence(AIBehaviour from, AIBehaviour to) =>
			from.OnComplete += () => ActiveBehaviour = to;
	}
}
