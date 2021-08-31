[System.Serializable]
public struct Damage
{
	public float Amount { get; private set; }
	public DamageType Type { get; private set; }

	public Damage(float amount, DamageType type = DamageType.Kinetic)
	{
		Amount = amount;
		Type = type;
	}
}
