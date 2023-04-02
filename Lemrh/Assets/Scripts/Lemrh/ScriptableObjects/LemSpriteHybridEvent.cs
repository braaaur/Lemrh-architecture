/*
 * Jakub Bukała
 * 
 * Created: 7.10.2020 r.
 * 
 * Edited: 8.04.2021 r.
 *  
 * [LemSpriteHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemSpriteHybridEvent")]
    public class LemSpriteHybridEvent : ScriptableObject, ILemrhEvent
    {
        private Sprite spriteValue;

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public bool everyChangeRisesEvents = false;

        public bool debugChange;

        public Sprite SpriteValue
        {
            set
            {
                bool isDifferent = false;

                if (spriteValue != value)
                {
                    isDifferent = true;
                }

                spriteValue = value;

                if (isDifferent || everyChangeRisesEvents)
                {
                    ValueChanged();
                }
            }
            get
            {
                return spriteValue;
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
    }
}
