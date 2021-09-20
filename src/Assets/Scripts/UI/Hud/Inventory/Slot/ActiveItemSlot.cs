namespace UI.HUD
{
	public class ActiveItemSlot : ItemSlotWithText
	{
		public override Mob Player
		{
			get => base.Player;
			protected set
			{
				if (Player)
					Player.OnActiveItemChanged -= SetItem;

				base.Player = value;

				value.OnActiveItemChanged += SetItem;

				UpdateText();
			}
		}
	}
}
