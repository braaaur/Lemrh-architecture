/*
 * Jakub Bukała
 * 
 * Created: 13.01.2018 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemrhEvent]
 * 
 * in LEMRH framework
 * 
 */

using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemEvents/LemrhEvent")]
    //public class LemrhEvent : SerializedScriptableObject, ILemrhEvent
    public class LemrhEvent : ScriptableObject, ILemrhEvent
    {
        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public bool debugRise;

        //[Button("Rise!")]
        public void Rise ()
        {
            for (int i = listeners.Count -1; i >= 0; i--)
            {
                listeners[i].OnEventRised(debugRise);
            }
        }

        public void RegisterListener (LemrhEventListener givenListener)
        {
            listeners.Add(givenListener);
        }

        public void UnregisterListener(LemrhEventListener givenListener)
        {
            listeners.Remove(givenListener);
        }
    }

    public interface ILemrhEvent
    {
        void RegisterListener(LemrhEventListener givenListener);
        void UnregisterListener(LemrhEventListener givenListener);
    }
}
