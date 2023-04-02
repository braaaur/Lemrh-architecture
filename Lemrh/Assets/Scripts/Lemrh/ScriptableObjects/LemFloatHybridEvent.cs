/*
 * Jakub Bukała
 * 
 * Created: 7.07.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemFloatHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhFloatHybridEvent")]
    public class LemFloatHybridEvent : ScriptableObject, ILemFloat, ILemrhEvent, ISavable, IResetable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")] 
        private float floatValue;
        [SerializeField]
        private float defaultValue;

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        [SerializeField] private bool everyChangeRisesEvents = false;

        [SerializeField] private bool debugChange;
        private bool isDifferent;
        [SerializeField] private bool debugMode;

        public float FloatValue
        {
            set
            {
                if (debugMode)
                {
                    Debug.LogError("LemFloatHybridEvent.cs - FloatValue { set } to " + value.ToString() + " in object " + name);
                }

                isDifferent = false;

                if (floatValue != value)
                {
                    isDifferent = true;
                }

                floatValue = value;

                if (isDifferent || everyChangeRisesEvents)
                {
                    ValueChanged();
                }
            }
            get
            {
                return floatValue;
            }
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

        public void ForceValueChanged()
        {
            ValueChanged();
        }

        public void Reset()
        {
            //Debug.Log("LemFloatHybridEvent - Reset()");

            FloatValue = defaultValue;
        }

        public ISave Save()
        {
            return new LemFloatSave(FloatValue, name);
        }

        public void Load(ISave givenValue)
        {
            FloatValue = ((LemFloatSave)givenValue).floatValue; //this works, seriously?
        }

        public string Id
        {
            get
            {
                return name;
            }
        }

        //[Button("SetFloatValue")]
        private void SetStrinValue(float givenValue)
        {
            FloatValue = givenValue;
        }
    }
}
