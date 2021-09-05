using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Newtonsoft.Json;

using DialogEventHolder = UI.Dialogue.DialogEventHolder;
using DialogueWindow = UI.Dialogue.DialogueWindow;

namespace DialogueEditor
{
    public enum eSaveVersion
    {
        V1_03 = 103,    // Initial save data
        V1_10 = 110,    // Parameters
    }
    

    //--------------------------------------
    // Conversation Monobehaviour (Serialized)
    //--------------------------------------

    [Serializable]
    [DisallowMultipleComponent]
    public class NPCConversation : MonoBehaviour
    {
        // Consts
        /// <summary> Version 1.10 </summary>
        public const int currentVersion = (int)eSaveVersion.V1_10;
        private const string childName = "ConversationEventInfo";

        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
        {
            Error = JsonUtils.ErrorHandler,
            TypeNameHandling = TypeNameHandling.All
        };

        // Getters
        public int Version { get { return saveVersion; } }

        // Serialized data
        [SerializeField]
        public int CurrentIDCounter = 1;
        [SerializeField]
        private string json;
        [SerializeField]
        private int saveVersion;
        [SerializeField]
        public string DefaultName;
        [SerializeField]
        public Sprite DefaultSprite;
        [SerializeField]
        public TMPro.TMP_FontAsset DefaultFont;
        [FormerlySerializedAs("Events"), SerializeField]
        private List<NodeEventHolder> NodeSerializedDataList;
        [SerializeField]
        private DialogEventHolder dialogSerializedData;
        [SerializeField]
        public TMPro.TMP_FontAsset ContinueFont;
        [SerializeField]
        public TMPro.TMP_FontAsset EndConversationFont;

        // Runtime vars
        public UnityEngine.Events.UnityEvent Event;
        public UnityEngine.Events.UnityEvent OnFinished;
        public List<EditableParameter> ParameterList; // Serialized into the json string

        public DialogEventHolder dialogEventHolder;

        public void OpenDialog() =>
            DialogueWindow.Instance.Open(this);

        //--------------------------------------
        // Util
        //--------------------------------------

        private GameObject GetEventInfo()
        {
            Transform evtInfoTransform = transform.Find(childName);
            if (evtInfoTransform)
                return evtInfoTransform.gameObject;

            GameObject obj = new GameObject(childName);
            obj.transform.SetParent(transform);

            return obj;
        }

        public DialogEventHolder GetDialogData()
        {
            if (dialogSerializedData)
                return dialogSerializedData;

            GameObject evtInfo = GetEventInfo();
            dialogSerializedData = evtInfo.AddComponent<DialogEventHolder>();
            dialogSerializedData.OnFinished = new UnityEngine.Events.UnityEvent();

            return dialogSerializedData;
        }

        public NodeEventHolder GetNodeData(int id)
        {
            // Create list if none
            if (NodeSerializedDataList == null)
                NodeSerializedDataList = new List<NodeEventHolder>();

            // Look through list to find by ID
            NodeEventHolder evtHolder = NodeSerializedDataList.Find(holder => holder.NodeID == id);
            if (evtHolder)
                return evtHolder;

            GameObject evtInfo = GetEventInfo();

            // Add a new Component for this node
            evtHolder = evtInfo.AddComponent<NodeEventHolder>();
            evtHolder.NodeID = id;
            evtHolder.Event = new UnityEngine.Events.UnityEvent();
            NodeSerializedDataList.Add(evtHolder);

            return evtHolder;
        }

        public void DeleteDataForNode(int id)
        {
            if (NodeSerializedDataList == null)
                return;

            int idx = NodeSerializedDataList.FindIndex(node => node.NodeID == id);
            if (idx == -1)
                return;
            
            NodeEventHolder evtHolder = NodeSerializedDataList[idx];
            GameObject.DestroyImmediate(evtHolder);
            NodeSerializedDataList.RemoveAt(idx);
        }

        public EditableParameter GetParameter(string name) =>
            ParameterList.Find(param => param.ParameterName == name);

        //--------------------------------------
        // Serialize and Deserialize
        //--------------------------------------

        public void Serialize(EditableConversation conversation)
        {
            saveVersion = currentVersion;

            conversation.Parameters = this.ParameterList;
            json = ToJson(conversation);
        }

        public Conversation Deserialize()
        {
            // Deserialize an editor-version (containing all info) that 
            // we will use to construct the user-facing Conversation data structure. 
            EditableConversation ec = this.DeserializeForEditor();

            return ConstructConversationObject(ec);
        }

        public EditableConversation DeserializeForEditor()
        {
            EditableConversation conversation = FromJson();
            
            if (conversation != null)
            {
                // Copy the param list
                ParameterList = conversation.Parameters;
                
                if (conversation.SpeechNodes != null)
                    conversation.SpeechNodes.ForEach(node => node.DeserializeAssetData(this));
                if (conversation.Options != null)
                    conversation.Options.ForEach(node => node.DeserializeAssetData(this));
            }
            else
            {
                conversation = new EditableConversation();
            }

            conversation.SaveVersion = saveVersion;

            // Clear our dummy event
            Event = new UnityEngine.Events.UnityEvent();
            OnFinished = new UnityEngine.Events.UnityEvent();

            // Reconstruct
            ReconstructEditableConversation(conversation);

            return conversation;
        }

        private void ReconstructEditableConversation(EditableConversation conversation)
        {
            if (conversation == null)
                conversation = new EditableConversation();

            // Get a list of every node in the conversation
            List<EditableConversationNode> allNodes = (conversation.SpeechNodes as IEnumerable<EditableConversationNode>).Concat(conversation.Options).ToList();

            // For every node: 
            // Find the children and parents by UID
            foreach (EditableConversationNode node in allNodes)
            {
                // New parents list 
                node.parents = new List<EditableConversationNode>();

                // Get parents by UIDs
                //-----------------------------------------------------------------------------
                // UPDATE:  This behaviour has now been removed. Later in this function, 
                //          the child->parent connections are constructed by using the 
                //          parent->child connections. Having both of these behaviours run 
                //          results in each parent being in the "parents" list twice. 
                // 
                // for (int j = 0; j < allNodes[i].parentUIDs.Count; j++)
                // {
                //     allNodes[i].parents.Add(conversation.GetNodeByUID(allNodes[i].parentUIDs[j]));
                // }
                //-----------------------------------------------------------------------------

                // Construct the parent->child connections
                //
                // V1.03
                if (conversation.SaveVersion <= (int)eSaveVersion.V1_03)
                {
                    // Construct Connections from the OptionUIDs and SpeechUIDs (which are now deprecated)
                    // This supports upgrading from V1.03 +

                    node.Connections = new List<EditableConnection>();
                    node.ParamActions = new List<EditableSetParamAction>();

                    if (
                        node.NodeType == EditableConversationNode.eNodeType.Speech &&
                        node is EditableSpeechNode speechNode
                    )
                    {
                        speechNode.Connections = speechNode.OptionUIDs.Select(
                            (uid) => new EditableOptionConnection(conversation.GetOptionByUID(uid))
                        ).ToList<EditableConnection>();

                        // Speech following speech
                        EditableSpeechNode speech = conversation.GetSpeechByUID(speechNode.SpeechUID);
                        if (speech != null)
                            speechNode.Connections.Add(new EditableSpeechConnection(speech));
                    }
                    else if (node is EditableOptionNode optionNode)
                    {
                        EditableSpeechNode speech = conversation.GetSpeechByUID(optionNode.SpeechUID);
                        if (speech != null)
                            node.Connections.Add(new EditableSpeechConnection(speech));
                    }
                }
                //
                // V1.10 +
                else
                {
                    // For each node..  Reconstruct the connections
                    foreach (EditableConnection connection in node.Connections)
                    {
                        if (connection is EditableSpeechConnection speechConnection)
                            speechConnection.Speech = conversation.GetSpeechByUID(speechConnection.NodeUID);
                        else if (connection is EditableOptionConnection optionConnection)
                            optionConnection.Option = conversation.GetOptionByUID(optionConnection.NodeUID);;
                    }
                }
            }

            // For every node: 
            // Tell any of the nodes children that the node is the childs parent
            foreach (EditableConversationNode node in allNodes)
            {
                foreach (EditableConnection connection in node.Connections)
                {
                    if (connection is EditableSpeechConnection speechConnection)
                        speechConnection.Speech.parents.Add(node);
                    else if (connection is EditableOptionConnection optionConnection)
                        optionConnection.Option.parents.Add(node);
                }
            }
        }

        private string ToJson(EditableConversation conversation)
        {
            if (conversation == null || conversation.Options == null)
                return String.Empty;

            return JsonConvert.SerializeObject(
                conversation,
                Debug.isDebugBuild ? Formatting.Indented : Formatting.None,
                serializerSettings
            );
        }

        private EditableConversation FromJson()
        {
            if (json?.Length == 0)
                return null;

            return JsonConvert.DeserializeObject<EditableConversation>(json, serializerSettings);
        }

        //--------------------------------------
        // Construct User-Facing Conversation Object and Nodes
        //--------------------------------------

        private Conversation ConstructConversationObject(EditableConversation ec)
        {
            // Create a conversation object
            Conversation conversation = new Conversation();

            // Construct the parameters
            CreateParameters(ec, conversation);

            // Construct the Conversation-Based variables (not node-based)
            conversation.ContinueFont = this.ContinueFont;
            conversation.EndConversationFont = this.EndConversationFont;

            // Create a dictionary to store our created nodes by UID
            Dictionary<int, SpeechNode> speechByID = new Dictionary<int, SpeechNode>();
            Dictionary<int, OptionNode> optionsByID = new Dictionary<int, OptionNode>();

            // Create a Dialogue and Option node for each in the conversation
            // Put them in the dictionary
            for (int i = 0; i < ec.SpeechNodes.Count; i++)
            {
                SpeechNode node = CreateSpeechNode(ec.SpeechNodes[i]);
                speechByID.Add(ec.SpeechNodes[i].ID, node);
            }

            for (int i = 0; i < ec.Options.Count; i++)
            {
                OptionNode node = CreateOptionNode(ec.Options[i]);
                optionsByID.Add(ec.Options[i].ID, node);
            }

            // Now that we have every node in the dictionary, reconstruct the tree 
            // And also look for the root
            ReconstructTree(ec, conversation, speechByID, optionsByID);

            return conversation;
        }

        private void CreateParameters(EditableConversation ec, Conversation conversation)
        {
            for (int i = 0; i < ec.Parameters.Count; i++)
            {
                if (ec.Parameters[i].ParameterType == EditableParameter.eParamType.Bool)
                {
                    EditableBoolParameter editableParam = ec.Parameters[i] as EditableBoolParameter;
                    BoolParameter boolParam = new BoolParameter(editableParam.ParameterName, editableParam.BoolValue);
                    conversation.Parameters.Add(boolParam);
                }
                else if (ec.Parameters[i].ParameterType == EditableParameter.eParamType.Int)
                {
                    EditableIntParameter editableParam = ec.Parameters[i] as EditableIntParameter;
                    IntParameter intParam = new IntParameter(editableParam.ParameterName, editableParam.IntValue);
                    conversation.Parameters.Add(intParam);
                }
            }
        }

        private SpeechNode CreateSpeechNode(EditableSpeechNode editableNode)
        {
            SpeechNode speech = new SpeechNode();
            speech.Text = editableNode.Text;
            speech.AutomaticallyAdvance = editableNode.AdvanceDialogueAutomatically;
            speech.AutoAdvanceShouldDisplayOption = editableNode.AutoAdvanceShouldDisplayOption;
            speech.TimeUntilAdvance = editableNode.TimeUntilAdvance;
            speech.TMPFont = editableNode.TMPFont;
            speech.Icon = editableNode.Icon;
            speech.Audio = editableNode.Audio;
            speech.Volume = editableNode.Volume;
            speech.AddTypewritingTime = editableNode.AddTypewritingTime;
            speech.Skippable = editableNode.Skippable;
            speech.MobTraits = editableNode.MobTraits;

            CopyParamActions(editableNode, speech);

            NodeEventHolder holder = this.GetNodeData(editableNode.ID);
            if (holder != null)
            {
                speech.Event = holder.Event;
            }

            return speech;
        }

        private OptionNode CreateOptionNode(EditableOptionNode editableNode)
        {
            OptionNode option = new OptionNode();
            option.Text = editableNode.Text;
            option.TMPFont = editableNode.TMPFont;

            CopyParamActions(editableNode, option);

            NodeEventHolder holder = this.GetNodeData(editableNode.ID);
            if (holder != null)
            {
                option.Event = holder.Event;
            }

            return option;
        }

        public void CopyParamActions(EditableConversationNode editable, ConversationNode node)
        {
            node.ParamActions = new List<SetParamAction>();

            for (int i = 0; i < editable.ParamActions.Count; i++)
            {
                if (editable.ParamActions[i].ParamActionType == EditableSetParamAction.eParamActionType.Int)
                {
                    EditableSetIntParamAction setIntEditable = editable.ParamActions[i] as EditableSetIntParamAction;

                    SetIntParamAction setInt = new SetIntParamAction();
                    setInt.ParameterName = setIntEditable.ParameterName;
                    setInt.Value = setIntEditable.Value;
                    node.ParamActions.Add(setInt);
                }
                else if (editable.ParamActions[i].ParamActionType == EditableSetParamAction.eParamActionType.Bool)
                {
                    EditableSetBoolParamAction setBoolEditable = editable.ParamActions[i] as EditableSetBoolParamAction;

                    SetBoolParamAction setBool = new SetBoolParamAction();
                    setBool.ParameterName = setBoolEditable.ParameterName;
                    setBool.Value = setBoolEditable.Value;
                    node.ParamActions.Add(setBool);
                }
            }
        }

        private void ReconstructTree(EditableConversation ec, Conversation conversation, Dictionary<int, SpeechNode> dialogues, Dictionary<int, OptionNode> options)
        {
            // Speech nodes
            List<EditableSpeechNode> editableSpeechNodes = ec.SpeechNodes;
            for (int i = 0; i < editableSpeechNodes.Count; i++)
            {
                EditableSpeechNode editableNode = editableSpeechNodes[i];
                SpeechNode speechNode = dialogues[editableNode.ID];

                // Connections
                List<EditableConnection> editableConnections = editableNode.Connections;
                for (int j = 0; j < editableConnections.Count; j++)
                {

                    int childID = editableConnections[j].NodeUID;

                    // Construct node->Speech
                    if (editableConnections[j].ConnectionType == EditableConnection.eConnectiontype.Speech)
                    {
                        SpeechConnection connection = new SpeechConnection(dialogues[childID]);
                        CopyConnectionConditions(editableConnections[j], connection);
                        speechNode.Connections.Add(connection);
                    }
                    // Construct node->Option
                    else if (editableConnections[j].ConnectionType == EditableConnection.eConnectiontype.Option)
                    {
                        OptionConnection connection = new OptionConnection(options[childID]);
                        CopyConnectionConditions(editableConnections[j], connection);
                        speechNode.Connections.Add(connection);
                    }
                }

                // Root?
                if (editableNode.EditorInfo.isRoot)
                {
                    conversation.Root = dialogues[editableNode.ID];
                }
            }


            // Option nodes
            List<EditableOptionNode> editableOptionNodes = ec.Options;
            for (int i = 0; i < editableOptionNodes.Count; i++)
            {
                EditableOptionNode editableNode = editableOptionNodes[i];
                OptionNode optionNode = options[editableNode.ID];

                // Connections
                List<EditableConnection> editableConnections = editableNode.Connections;
                for (int j = 0; j < editableConnections.Count; j++)
                {
                    int childID = editableConnections[j].NodeUID;

                    // Construct node->Speech
                    if (editableConnections[j].ConnectionType == EditableConnection.eConnectiontype.Speech)
                    {
                        SpeechConnection connection = new SpeechConnection(dialogues[childID]);
                        CopyConnectionConditions(editableConnections[j], connection);
                        optionNode.Connections.Add(connection);
                    }
                }
            }
        }

        private void CopyConnectionConditions(EditableConnection editableConnection, Connection connection)
        {
            List<EditableCondition> editableConditions = editableConnection.Conditions;
            for (int i = 0; i < editableConditions.Count; i++)
            {
                if (editableConditions[i].ConditionType == EditableCondition.eConditionType.BoolCondition)
                {
                    EditableBoolCondition ebc = editableConditions[i] as EditableBoolCondition;

                    BoolCondition bc = new BoolCondition();
                    bc.ParameterName = ebc.ParameterName;
                    switch (ebc.CheckType)
                    {
                        case EditableBoolCondition.eCheckType.equal:
                            bc.CheckType = BoolCondition.eCheckType.equal;
                            break;
                        case EditableBoolCondition.eCheckType.notEqual:
                            bc.CheckType = BoolCondition.eCheckType.notEqual;
                            break;
                    }
                    bc.RequiredValue = ebc.RequiredValue;

                    connection.Conditions.Add(bc);
                }
                else if (editableConditions[i].ConditionType == EditableCondition.eConditionType.IntCondition)
                {
                    EditableIntCondition eic = editableConditions[i] as EditableIntCondition;

                    IntCondition ic = new IntCondition();
                    ic.ParameterName = eic.ParameterName;
                    switch (eic.CheckType)
                    {
                        case EditableIntCondition.eCheckType.equal:
                            ic.CheckType = IntCondition.eCheckType.equal;
                            break;
                        case EditableIntCondition.eCheckType.greaterThan:
                            ic.CheckType = IntCondition.eCheckType.greaterThan;
                            break;
                        case EditableIntCondition.eCheckType.lessThan:
                            ic.CheckType = IntCondition.eCheckType.lessThan;
                            break;
                    }
                    ic.RequiredValue = eic.RequiredValue;

                    connection.Conditions.Add(ic);
                }
            }
        }
    }
}