using System;
using UnityEngine;
using UnityEngine.Events;

namespace OcelotAI
{
	public abstract class AIBehaviour : MonoBehaviour
	{
		public event Action<Vector3> OnMoveDirect;
		public event Action<Vector3> OnMoveTo;
		public event Action<Vector3> OnTurnTo;
		public event Action<Vector3> OnAim;
		public event Action<bool> OnTriggerStateChanged;
		public event Action OnDodge;
		public event Action OnThrow;

		public event Action OnComplete;

		/// <summary>
		/// If positive, determines how long the behaviour will last
		/// until marked complete automatically.
		/// </summary>
		[SerializeField]
		private float duration = 0f;
		private float activationTime = 0f;

		/// <summary>
		/// Determines how long the behaviour should stay unavailable
		/// after its deactivation.
		/// </summary>
		[SerializeField]
		private float cooldown = 0f;
		private float deactivationTime = 0f;
		public bool IsOnCooldown => Time.time <= deactivationTime + cooldown;

		public AIStateMachine StateMachine { get; private set; }

		private bool _active = false;
		public bool Active
		{
			get => _active;
			set
			{
				if (!_active && !value)  // No need to deactivate a behaviour twice
					return;

				if (_active = value)
				{
					activationTime = Time.time;
					ActivateControl(StateMachine.Controller);
				}
				else
				{
					deactivationTime = Time.time;
					DeactivateControl(StateMachine.Controller);
				}

				enabled = _active;
			}
		}

		/// <summary>
		/// Suggests if this behaviour has reached its goals.
		/// </summary>
		protected bool _complete = false;
		public virtual bool Complete
		{
			get => _complete;
			protected set
			{
				if (_complete = value)
					OnComplete?.Invoke();
			}
		}

		/// <summary>
		/// Suggests if this behaviour should be enable-able by the state machine.
		/// </summary>
		public virtual bool Available => !IsOnCooldown;

		public virtual void Awake()
		{
			StateMachine = GetComponentInParent<AIStateMachine>();
		}

		private void Update()
		{
			if (duration > 0f && Time.time - activationTime > duration)
				Complete = true;

			// Normally, disabled components won't call Update, but better be sure.
			if (!Active || StateMachine.Controller == null)
				return;
			
			UpdateControl(StateMachine.Controller);
		}

		protected abstract void UpdateControl(AIController controller);

		protected virtual void ActivateControl(AIController controller)
		{
		}

		protected virtual void DeactivateControl(AIController controller)
		{
		}

		protected void OnDestroy()
		{
			// <TODO> Хавчик посмотри, вот этот ритуал ваще имеет смысл?
			if (OnComplete != null)
				foreach (Delegate subscription in OnComplete.GetInvocationList())
					OnComplete -= subscription as Action;
		}

		// That's an awful pile of spaghetti but you can't invoke events from children classes.
		// UnityEvents could be used as a workaround but they're not as much performant.
		protected void MoveDirect(Vector3 direction) => OnMoveDirect?.Invoke(direction);
		protected void MoveTo(Vector3 pos) => OnMoveTo?.Invoke(pos);
		protected void TurnTo(Vector3 pos) => OnTurnTo?.Invoke(pos);
		protected void Aim(Vector3 target) => OnAim?.Invoke(target);
		protected void SetTrigger(bool trigger) => OnTriggerStateChanged?.Invoke(trigger);
		protected void Dodge() => OnDodge?.Invoke();
		protected void Throw() => OnThrow?.Invoke();
	}
}
