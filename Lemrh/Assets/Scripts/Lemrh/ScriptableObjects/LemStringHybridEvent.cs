/*
 * Jakub Bukała
 * 
 * Created: 7.07.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemStringHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhStringHybridEvent")]
    //public class LemStringHybridEvent : SerializedScriptableObject, ILemString, ILemrhEvent, IResetable, ISavable
    public class LemStringHybridEvent : ScriptableObject, ILemString, ILemrhEvent, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")] 
        private string stringValue;
        public string defaultValue;

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public bool everyChangeRisesEvents = false;

        public bool debugChange;

        public string StringValue
        {
            set
            {
                bool isDifferent = false;

                if (stringValue != value)
                {
                    isDifferent = true;
                }

                stringValue = value;

                if (isDifferent || everyChangeRisesEvents)
                {
                    ValueChanged();
                }
            }
            get
            {
                return stringValue;
            }
        }

        public void Reset()
        {
            //Debug.Log("LemString - Reset()");

            StringValue = defaultValue;
        }

        private void ValueChanged()
        {
            if (valueChangeDelegate != null)
            {
                valueChangeDelegate(debugChange);
            }

            RiseListeners();
        }

        //delegate part
        public void RegisterDelegate(ValueChangeDelegate givenDelegate)
        {
            valueChangeDelegate += givenDelegate;
        }

        public void UnregisterDelegate(ValueChangeDelegate givenDelegate)
        {
            valueChangeDelegate -= givenDelegate;
        }

        //event part
        private void RiseListeners()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRised(debugChange);
            }
        }

        public void RegisterListener(LemrhEventListener givenListener)
        {
            listeners.Add(givenListener);
        }

        public void UnregisterListener(LemrhEventListener givenListener)
        {
            listeners.Remove(givenListener);
        }

        public ISave Save()
        {
            return new LemStringSave(StringValue, name);
        }

        public void Load(ISave givenValue)
        {
            StringValue = ((LemStringSave)givenValue).stringValue; //this works, seriously?
        }

        public string Id
        {
            get
            {
                return name;
            }
        }

        //editor debug
        //[Button("SetStrinValue")]
        private void SetStrinValue(string givenValue)
        {
            StringValue = givenValue;
        }
    }
}
