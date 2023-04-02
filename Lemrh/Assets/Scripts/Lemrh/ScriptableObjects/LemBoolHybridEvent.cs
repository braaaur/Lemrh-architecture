/*
 * Jakub Bukała
 * 
 * Created: 5.07.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemBoolHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhBoolHybridEvent")]
    //public class LemBoolHybridEvent : SerializedScriptableObject, ILemBool, ILemrhEvent, IResetable, ISavable
    public class LemBoolHybridEvent : ScriptableObject, ILemBool, ILemrhEvent, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        [SerializeField] private bool defaultValue; 
        
        private bool boolValue;
        
        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        [SerializeField] private bool everyChangeRisesEvents = true;

        [SerializeField] private bool debugChange;

        public bool BoolValue
        {
            set
            {
                bool isDifferent = false;

                if (boolValue != value)
                {
                    isDifferent = true;
                }

                boolValue = value;

                if (isDifferent || everyChangeRisesEvents)
                {
                    ValueChanged();
                }
            }
            get
            {
                return boolValue;
            }
        }
        
        public void ToggleValue()
        {
            BoolValue = !BoolValue;
        }

        private void ValueChanged()
        {
            if (debugChange)
            {
                Debug.Log("ValueChanged() in LemBoolHybridEvent");
            }
            
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
            BoolValue = defaultValue;
        }

        //[Button("Set True")]
        private void SetTrue()
        {
            BoolValue = true;
        }

        //[Button("Set False")]
        private void SetFalse()
        {
            BoolValue = false;
        }

        public ISave Save()
        {
            return new LemBoolSave(BoolValue, name);
        }

        public void Load(ISave givenValue)
        {
            BoolValue = ((LemBoolSave)givenValue).boolValue; //this works, seriously?
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface IResetable
    {
        void Reset();
    }
}
