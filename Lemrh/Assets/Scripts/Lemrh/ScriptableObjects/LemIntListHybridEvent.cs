/*
 * Jakub Bukała
 * 
 * Created: 25.04.2021 r.
 * 
 * Edited: 26.04.2021 r.
 *  
 * [LemIntListHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhIntListHybridEvent")]
    public class LemIntListHybridEvent : ScriptableObject, ILemIntList, ILemrhEvent, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        private List<int> intListValue;

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public List<int> defaultValue;

        public bool debugChange;

        [SerializeField]
        [TextArea]
        private string info;

        public List<int> IntListValue
        {
            set
            {
                intListValue = value;

                ValueChanged();
            }
            get
            {
                return intListValue;
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

        public void Reset()
        {
            IntListValue = new List<int>(defaultValue);
        }

        public ISave Save()
        {
            return new LemIntListSave(IntListValue, name);
        }

        public void Load(ISave givenValue)
        {
            IntListValue = ((LemIntListSave)givenValue).intListValue; //this works, seriously?
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface ILemIntList
    {
        List<int> IntListValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemIntListSave : ISave
    {
        public LemIntListSave(List<int> givenValue, string givenId)
        {
            intListValue = givenValue;
            id = givenId;
        }

        public string id;
        public List<int> intListValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
