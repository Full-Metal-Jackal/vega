using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
namespace EditorEx
{
	[CustomPropertyDrawer(typeof(PropAttribute))]
	public class PropDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
		{
			bool prevGuiState = GUI.enabled;
			
			PropAttribute propAttr = attribute as PropAttribute;

			if (propAttr.ReadOnly)
				GUI.enabled = !propAttr.ReadOnly;

			if (propAttr.Name != null)
				label.text = propAttr.Name;
			
			EditorGUI.PropertyField(pos, prop, label);

			GUI.enabled = prevGuiState;
		}
	}
}
#endif
