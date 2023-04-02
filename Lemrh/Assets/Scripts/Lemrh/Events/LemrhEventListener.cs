/*
 * Jakub Bukała
 * 
 * Created: 13.01.2018 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemrhEventListener]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.Events;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    public class LemrhEventListener : MonoBehaviour
    {
        public ILemrhEvent lemrhEvent;
        public UnityEvent Response;

        public void OnEventRised(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("OnEventRised() in LemrhEventListener.cs - object: " + gameObject.name);
            }
            Response.Invoke();
        }

        private void OnEnable()
        {
            lemrhEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            lemrhEvent.UnregisterListener(this);
        }
    }
}
