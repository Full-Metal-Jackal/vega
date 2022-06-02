using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedJump : MonoBehaviour
{
	[SerializeField]
	private Rigidbody body;

	[SerializeField]
	private float velocityPerMeter = .65f;

	public void Jump(Vector3 velocity)
	{
		body.AddForce(velocity, ForceMode.VelocityChange);
	}

	public void JumpAt(Transform target) => JumpAt(target.position);
	public void JumpAt(Vector3 pos, float verticalForce = 5f)
	{
		Vector3 velocity = pos - body.position;

		// Doesn't take physics into account,
		// which is probably enough for scenic purposes
		velocity *= velocityPerMeter;

		velocity.y = verticalForce;

		Jump(velocity);
	}
}
