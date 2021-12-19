using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenario
{
	public class ScriptedEmerging : ScenarioScript
	{
		[SerializeField]
		private Transform emergingTransform;

		[SerializeField]
		private float speed = 1f;

		[SerializeField]
		private float depth;

		private Vector3 targetPosition;

		private bool activated = false;

		private void Start()
		{
			targetPosition = emergingTransform.position;
			emergingTransform.position += Vector3.down * depth;
		}

		public void Perform()
		{
			activated = true;
		}

		private void Update()
		{
			if (!activated)
				return;

			emergingTransform.position += speed * Time.deltaTime * Vector3.up;
			if (emergingTransform.position.y > targetPosition.y)
				Finish();
		}

		protected override void Finish()
		{
			emergingTransform.position = targetPosition;
			activated = false;

			base.Finish();
		}
	}
}
