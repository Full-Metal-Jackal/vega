namespace UI.HUD
{
	public class ThrowableItemSlot : ItemSlotWithText
	{
		public override Mob Player
		{
			get => base.Player;
			protected set
			{
				if (Player)
					Player.OnThrowableItemChanged -= SetItem;

				base.Player = value;
				
				value.OnThrowableItemChanged += SetItem;

				UpdateText();
			}
		}
	}
}
