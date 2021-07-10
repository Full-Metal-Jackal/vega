using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorTools.DialogueGraph
{
	public class DialogueGraph : EditorWindow
	{
		private DialogueGraphView view;
		private string fileName;

		[MenuItem("Tools/Dialogue Graph")]
		public static void Open()
		{
			DialogueGraph window = GetWindow<DialogueGraph>();
			window.titleContent = new GUIContent("Dialogue Graph");
		}

		private void OnEnable()
		{
			SetupView();
			SetupToolbar();
		}

		private void SetupView()
		{
			view = new DialogueGraphView();
			view.StretchToParentSize();
			rootVisualElement.Add(view);
		}

		private void SetupToolbar()
		{
			Toolbar toolbar = new Toolbar();

			TextField fileNameInput = new TextField("File Name:");
			fileNameInput.SetValueWithoutNotify(fileName);
			fileNameInput.MarkDirtyRepaint();
			fileNameInput.RegisterValueChangedCallback(evt => fileName = evt.newValue);
			toolbar.Add(fileNameInput);

			toolbar.Add(new Button(() => view.AddNode())
			{
				text = "Add Node"
			});
			toolbar.Add(new Button(() => view.AddBranchingNode())
			{
				text = "Add Branching Node"
			});
			toolbar.Add(new Button(() => view.AddEntryNode())
			{
				text = "Add Entry"
			});
			toolbar.Add(new Button(() => view.AddExitNode())
			{
				text = "Add Exit"
			});

			toolbar.Add(new Button(() => DialogueAssetManager.Save(view, fileName))
			{
				text = "Save"
			});
			toolbar.Add(new Button(() => DialogueAssetManager.Load(view, fileName))
			{
				text = "Load"
			});

			rootVisualElement.Add(toolbar);
		}

		private void OnDisable()
		{
			rootVisualElement.Add(view);
		}
	}
}
