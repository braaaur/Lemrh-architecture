/*
 * Jakub Bukała
 * 
 * Created: 25.09.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemColorHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemColorHybridEvent")]
    //public class LemColorHybridEvent : SerializedScriptableObject, ILemrhEvent
    public class LemColorHybridEvent : ScriptableObject, ILemrhEvent
    {
        private Color colorValue;

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public bool everyChangeRisesEvents = false;

        public bool debugChange;

        public Color ColorValue
        {
            set
            {
                bool isDifferent = false;

                if (colorValue != value)
                {
                    isDifferent = true;
                }

                colorValue = value;

                if (isDifferent || everyChangeRisesEvents)
                {
                    ValueChanged();
                }
            }
            get
            {
                return colorValue;
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

        //[Button("Set Color")]
        private void SetColor(Color givenColor)
        {
            ColorValue = givenColor;
        }
    }
}

