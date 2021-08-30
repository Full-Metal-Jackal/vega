public class PickableThrowable : Pickable<Throwable>
{
	public override void Setup(Throwable item)
	{
		base.Setup(item);
		print($"ooga booga {item}");
	}
}
