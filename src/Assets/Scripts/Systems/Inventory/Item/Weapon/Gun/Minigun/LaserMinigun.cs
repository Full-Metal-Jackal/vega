using UnityEngine;

public class LaserMinigun : EnergyGun
{
	private Transform barrelsBase;

	/// <summary>
	/// Measured in degrees per second.
	/// </summary>
	[SerializeField]
	private float maxSpinningSpeed = 360f;
	/// <summary>
	/// Minimal spinning speed requiredd to start shooting.
	/// </summary>
	[SerializeField]
	private float requiredSpinningSpeed = 180f;
	[SerializeField]
	private float spinningSpeedGain = 90f;
	[SerializeField]
	private float spinningSpeedLoss = 90f;

	/// <summary>
	/// How many seconds of delay are added at the lowest spinning rate.
	/// </summary>
	[SerializeField]
	private float additiveFireDelay = 0.2f;

	private float spinningSpeed;

	public override float FireDelay =>
		base.FireDelay + additiveFireDelay * (1 - (spinningSpeed / maxSpinningSpeed));
	
	public override void SingleUse(Vector3 target)
	{
	}

	protected override void Equip()
	{
		base.Equip();

		if (Model is MinigunModelData minigunModel)
			barrelsBase = minigunModel.BarrelsBase;
	}

	protected override void Update()
	{
		base.Update();

		float delta = Time.deltaTime;

		if (IsTriggerHeld)
		{
			spinningSpeed = Mathf.Min(spinningSpeed + spinningSpeedGain * delta, maxSpinningSpeed);
			if (Owner && currentFireDelay <= 0 && spinningSpeed > requiredSpinningSpeed)
				Fire(Owner.AimPos);
		}
		else
		{
			spinningSpeed = Mathf.Max(spinningSpeed - spinningSpeedLoss * delta, 0f);
		}

		if (barrelsBase)
			barrelsBase.transform.localRotation *= Quaternion.Euler(0, spinningSpeed * delta, 0);
	}
}
