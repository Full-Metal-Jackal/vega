using System;
using UnityEngine;

namespace UI.Dialogue
{
    [Serializable]
    public class DialogEventHolder : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent OnFinished;
    }
}