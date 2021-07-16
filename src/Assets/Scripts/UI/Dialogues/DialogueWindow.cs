using System;
using System.Collections.Generic;
using DialogueEditor;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Dialogue
{
	[RequireComponent(typeof(AudioSource))]
	public class DialogueWindow : MonoSingleton<DialogueWindow>
	{
		[SerializeField]
		private OptionButton optionPrefab;
		[SerializeField]
		private ContinueButton continueButton;

		[SerializeField]
		private RectTransform optionsHolder;

		[SerializeField]
		private TextMeshProUGUI characterLine;
		[SerializeField]
		private TextMeshProUGUI characterName;

		[SerializeField]
		private RectTransform divider;

		private Conversation conversation;
		private SpeechNode currentSpeech;

		private AudioSource audioSource;

		public bool typewritingEnabled = true;
		/// <summary>
		/// How much seconds passes between two letters' appearance.
		/// </summary>
		private float charTypingDelay = .05f;  // <TODO> most probably will be contained inside the character's portrait info.
		/// <summary>
		/// The "voice" of the speaker.
		/// </summary>
		private AudioClip typewritingSound;  // <TODO> most probably will be contained inside the character's portrait info.
		private float typewriteNext = 0f;
		private bool typewriting = false;

		private readonly List<OptionButton> optionButtons = new List<OptionButton>();
		private OptionButton selectedOption;
		public OptionButton SelectedOption
		{
			get => selectedOption;
			set
			{
				foreach (OptionButton button in optionButtons)
					button.ToggleSelect(false);
				if (selectedOption = value)
					selectedOption.ToggleSelect(true);
			}
		}
		private int optionIndex = 0;
		public void SetOptionIndex(int index)
		{
			if (optionButtons.Count <= 0)
				return;

			// heehoo pajeet math
			optionIndex = (optionButtons.Count + (index % optionButtons.Count)) % optionButtons.Count;
			SelectedOption = optionButtons[optionIndex];
		}

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		private void Start() => Setup();

		protected virtual void Setup()
		{
			gameObject.SetActive(false);
			continueButton.OnClick += (buttonNode) => SetupSpeech(buttonNode as SpeechNode);
		}

		private void Update()
		{
			if (typewritingEnabled && typewriting && Time.time > typewriteNext)
				Typewrite();
		}

		private void OnClickAnywhere(InputAction.CallbackContext ctx)
		{
			if (typewriting)
				FinishSpeech();
		}

		private void OnCycle(InputAction.CallbackContext ctx)
		{
			bool backwards = ctx.ReadValue<Vector2>().y > 0;
			SetOptionIndex(optionIndex + (backwards ? -1 : 1));
		}

		private void OnSubmitPressed(InputAction.CallbackContext ctx)
		{
			if (typewriting)
				FinishSpeech();
			else if (SelectedOption)
				SelectedOption.OnButtonClicked();
			else if (continueButton.isActiveAndEnabled)
				continueButton.OnButtonClicked();
		}

		private void Typewrite()
		{
			if (++characterLine.maxVisibleCharacters >= currentSpeech.Text.Length - 1)
			{
				FinishSpeech();
				return;
			}

			if (typewritingSound)
				audioSource.PlayOneShot(typewritingSound);

			typewriteNext = Time.time + charTypingDelay;
		}

		private void FinishSpeech()
		{
			typewriting = false;
			CreateUIOptions();
			characterLine.maxVisibleCharacters = currentSpeech.Text.Length;
		}

		public void Open(NPCConversation npcConversation)
		{
			Debug.Log("Opening the dialogue window...");
			gameObject.SetActive(true);
			Game.State = GameState.Paused;
			Hud.Instance.Toggle(false);

			conversation = npcConversation.Deserialize();

			Input.PlayerInput.Actions.UI.Click.performed += OnClickAnywhere;
			Input.PlayerInput.Actions.UI.Submit.performed += OnSubmitPressed;
			Input.PlayerInput.Actions.UI.Navigate.performed += OnCycle;

			SetupSpeech(currentSpeech = conversation.Root);
		}

		public void SetupSpeech(SpeechNode speech)
		{
			if (speech is null)
			{
				Close();
				return;
			}

			currentSpeech = speech;

			ClearOptions();

			// <TODO> We will store the character's name in the portrait info later.
			characterName.text = speech.Name;

			characterLine.font = speech.TMPFont;
			characterLine.text = speech.Text;
			if (typewritingEnabled)
			{
				characterLine.maxVisibleCharacters = 0;
				typewriting = true;
				Typewrite();
			}
			else
			{
				characterLine.maxVisibleCharacters = speech.Text.Length;
				FinishSpeech();
			}

			speech.Event?.Invoke();

			DoParamAction(speech);

			if (speech.Audio)
			{
				audioSource.volume = speech.Volume;
				audioSource.PlayOneShot(speech.Audio);
			}
		}

		public void DoParamAction(ConversationNode node)
		{
			if (node.ParamActions == null) { return; }

			for (int i = 0; i < node.ParamActions.Count; i++)
			{
				string name = node.ParamActions[i].ParameterName;

				switch (node.ParamActions[i].ParamActionType)
				{
				case SetParamAction.eParamActionType.Int:
					conversation.SetInt(
						name,
						(node.ParamActions[i] as SetIntParamAction).Value,
						out eParamStatus _
					);
					break;
				case SetParamAction.eParamActionType.Bool:
					conversation.SetBool(
						name,
						(node.ParamActions[i] as SetBoolParamAction).Value,
						out eParamStatus _
					);
					break;
				}
			}
		}

		private bool ConditionsMet(Connection connection)
		{
			List<Condition> conditions = connection.Conditions;
			foreach (Condition condition in conditions)
			{
				bool conditionMet = false;

				if (condition is IntCondition intCondition)
				{
					string paramName = intCondition.ParameterName;
					int requiredValue = intCondition.RequiredValue;
					int currentValue = conversation.GetInt(paramName, out _);

					switch (intCondition.CheckType)
					{
					case IntCondition.eCheckType.equal:
						conditionMet = (currentValue == requiredValue);
						break;
					case IntCondition.eCheckType.greaterThan:
						conditionMet = (currentValue > requiredValue);
						break;
					case IntCondition.eCheckType.lessThan:
						conditionMet = (currentValue < requiredValue);
						break;
					}
				}

				if (condition is BoolCondition boolCondition)
				{
					string paramName = boolCondition.ParameterName;
					bool requiredValue = boolCondition.RequiredValue;
					bool currentValue = conversation.GetBool(paramName, out _);

					switch (boolCondition.CheckType)
					{
					case BoolCondition.eCheckType.equal:
						conditionMet = (currentValue == requiredValue);
						break;
					case BoolCondition.eCheckType.notEqual:
						conditionMet = (currentValue != requiredValue);
						break;
					}
				}

				if (!conditionMet)
					return false;
			}

			return true;
		}

		private void CreateUIOptions()
		{
			if (currentSpeech.ConnectionType == Connection.eConnectionType.Option)
			{
				bool any = false;
				foreach (OptionConnection connection in currentSpeech.Connections)
				{
					if (ConditionsMet(connection))
					{
						CreateOptionButton(connection.OptionNode);
						any = true;
					}
				}

				if (!any)
					return;
				ToggleOptions(true);
				SetOptionIndex(0);
			}
			else if (!currentSpeech.AutomaticallyAdvance || currentSpeech.AutoAdvanceShouldDisplayOption
				&& currentSpeech.ConnectionType == Connection.eConnectionType.Speech)
			{
				UpdateContinueButton(GetValidSpeech(currentSpeech));
			}
		}

		private SpeechNode GetValidSpeech(ConversationNode parentNode)
		{
			if (parentNode.ConnectionType != Connection.eConnectionType.Speech
				|| parentNode.Connections.Count <= 0
			)
				return null;

			foreach (SpeechConnection connection in parentNode.Connections)
				if (ConditionsMet(connection))
					return connection.SpeechNode;

			return null;
		}

		private OptionButton CreateOptionButton(OptionNode node)
		{
			OptionButton button = Instantiate(optionPrefab, optionsHolder);
			button.Setup(node);
			button.OnClick += (buttonNode) => OptionSelected(buttonNode as OptionNode);

			optionButtons.Add(button);

			return button;
		}

		private void UpdateContinueButton(SpeechNode node)
		{
			continueButton.UpdateNode(node);
			continueButton.Toggle(true);
		}

		private void OptionSelected(OptionNode option)
		{
			if (option is null)
			{
				Debug.LogWarning($"Null option has been selected!");
				return;
			}

			DoParamAction(option);
			if (!(option.Event is null))
				option.Event.Invoke();

			ClearOptions();

			if (currentSpeech.AutomaticallyAdvance && IsAutoAdvance())
				return;

			if (GetValidSpeech(option) is SpeechNode nextSpeech)
				SetupSpeech(nextSpeech);
			else
				Close();
		}

		private bool IsAutoAdvance()
		{
			switch (currentSpeech.ConnectionType)
			{
			case Connection.eConnectionType.Speech:
				if (GetValidSpeech(currentSpeech) is SpeechNode next)
				{
					SetupSpeech(next);
					return true;
				}
				break;
			case Connection.eConnectionType.None:
				Close();
				return true;
			}

			return false;
		}

		private void ClearOptions()
		{
			optionButtons.Clear();
			foreach (RectTransform child in optionsHolder)
				Destroy(child.gameObject);
			continueButton.Toggle(false);
			ToggleOptions(false);
		}

		private void ToggleOptions(bool toggle)
		{
			optionsHolder.gameObject.SetActive(toggle);
			divider.gameObject.SetActive(toggle);
		}

		public void Close()
		{
			Debug.Log("Closing the dialogue window...");
			gameObject.SetActive(false);
			Game.State = GameState.Normal;
			Hud.Instance.Toggle(true);

			Input.PlayerInput.Actions.UI.Click.performed -= OnClickAnywhere;
			Input.PlayerInput.Actions.UI.Submit.performed -= OnSubmitPressed;
			Input.PlayerInput.Actions.UI.Navigate.performed -= OnCycle;

			ClearOptions();
		}
	}
}
