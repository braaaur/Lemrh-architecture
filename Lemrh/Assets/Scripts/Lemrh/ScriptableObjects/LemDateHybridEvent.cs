/*
 * Jakub Bukała
 * 
 * Created: 11.02.2021 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemDateHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;
using System;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhDateHybridEvent")]
    //public class LemDateHybridEvent : SerializedScriptableObject, ILemDate, ILemrhEvent, IResetable, ISavable
    public class LemDateHybridEvent : ScriptableObject, ILemDate, ILemrhEvent, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        private DateTime dateValue;

        [SerializeField]
        private int defaultYear = 1970;
        [SerializeField]
        public int defaultMonth = 9;
        [SerializeField]
        public int defaultDay = 4;
        [SerializeField]
        public int defaultHour = 16;
        [SerializeField]
        public int defaultMinute = 13;
        [SerializeField]
        public int defaultSecond = 0;

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public bool debugChange;

        public DateTime DateValue
        {
            set
            {
                dateValue = value;

                ValueChanged();
            }
            get
            {
                return dateValue;
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

        public void RegisterDelegate(ValueChangeDelegate givenDelegate)
        {
            valueChangeDelegate += givenDelegate;
        }

        public void UnregisterDelegate(ValueChangeDelegate givenDelegate)
        {
            valueChangeDelegate -= givenDelegate;
        }
        
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
            //Debug.Log("LemDateHybridEvent - Reset()");

            DateValue = GetDefaultDateTime();
        }

        public DateTime GetDefaultDateTime()
        {
            return new DateTime(defaultYear, defaultMonth, defaultDay, defaultHour, defaultMinute, defaultSecond);
        }

        public ISave Save()
        {
            return new LemDoubleSave((DateValue - epoch).TotalSeconds, name);
        }

        public void Load(ISave givenValue)
        {
            DateValue = epoch.AddSeconds(((LemDoubleSave)givenValue).doubleValue);
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface ILemDate
    {
        DateTime DateValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemDoubleSave : ISave
    {
        public LemDoubleSave(double givenValue, string givenId)
        {
            doubleValue = givenValue;
            id = givenId;
        }

        public string id;
        public double doubleValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
