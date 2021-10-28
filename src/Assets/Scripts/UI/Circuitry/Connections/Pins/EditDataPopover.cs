using UnityEngine;
using UnityEngine.UI;

using Circuitry;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(InputField))]
	public class EditDataPopover : MonoBehaviour
	{
		[SerializeField]
		private DataPinWidget dataInput;

		private InputField inputField;

		private void Awake()
		{
			inputField = GetComponent<InputField>();
		}

		private void Start() =>
			inputField.onValueChanged.AddListener(value => dataInput.Pin.Set((Number)value));
	}
}
