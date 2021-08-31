[System.Serializable]
public struct Damage
{
	public float amount;
	public DamageType type;

	public Damage(float amount, DamageType type = DamageType.Kinetic)
	{
		this.amount = amount;
		this.type = type;
	}
}
