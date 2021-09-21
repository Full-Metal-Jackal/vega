using System;
using UnityEngine;

namespace DialogueEditor
{
    /// <summary>
    /// This class holds all of the values for a node which 
    /// need to be serialized. 
    /// </summary>
    [Serializable]
    public class NodeEventHolder : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent Event;

        public int NodeID;
        public TMPro.TMP_FontAsset TMPFont;
        public Sprite Icon;
        public AudioClip Audio;
    }
}
