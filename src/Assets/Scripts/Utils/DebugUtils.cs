using UnityEngine;
using UnityEngine.InputSystem;

public class DebugUtils : MonoSingleton<DebugUtils>
{
	[field: SerializeField, EditorEx.Prop(ReadOnly = true, Name = "Mouse inpud disabled")]
	public bool MouseInputDisabled { get; private set; }

	protected override void Awake()
	{
		base.Awake();

		EnableMouse();
	}

	[ContextMenu("Disable mouse")]
	public void DisableMouse()
	{
		InputSystem.DisableDevice(Mouse.current);
		MouseInputDisabled = true;
	}

	[ContextMenu("Enable mouse")]
	public void EnableMouse()
	{
		InputSystem.EnableDevice(Mouse.current);
		MouseInputDisabled = false;
	}
}
