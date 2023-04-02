/*
 * Jakub Bukała
 * 
 * Created: 19.07.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemIntHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhIntHybridEvent")]
    //public class LemIntHybridEvent : SerializedScriptableObject, ILemInt, ILemrhEvent, IResetable, ISavable
    public class LemIntHybridEvent : ScriptableObject, ILemInt, ILemrhEvent, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")] 
        private int intValue;

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public bool everyChangeRisesEvents = false;

        public int defaultValue;

        public bool debugChange;

        [SerializeField]
        [TextArea]
        private string info; 

        public int IntValue
        {
            set
            {
                bool isDifferent = false;

                if (intValue != value)
                {
                    isDifferent = true;
                }

                intValue = value;

                if (isDifferent || everyChangeRisesEvents)
                {
                    ValueChanged();
                }
            }
            get
            {
                return intValue;
            }
        }

        private void ValueChanged()
        {
            /*
            if (debugChange)
            {
                Debug.Log("ValueChanged() in LemIntHybridEvent");
            }
            */

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

        public void Reset()
        {
            IntValue = defaultValue;
        }

        public void SetFromFloat(float givenValue)
        {
            IntValue = Mathf.RoundToInt(givenValue);
        }

        public ISave Save()
        {
            return new LemIntSave(IntValue, name);
        }

        public void Load(ISave givenValue)
        {
            IntValue = ((LemIntSave)givenValue).intValue; //this works, seriously?
        }

        public string Id
        {
            get
            {
                return name;
            }
        }

        //[Button("Set Value")]
        private void SetValueTest(int givenValue)
        {
            IntValue = givenValue;
        }
    }
}
