/*
 * Jakub Buka³a
 * 
 * Created: 1.02.2022 r.
 * 
 * Edited: -
 *  
 * [LemStringListHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhStringListHybridEvent")]
    public class LemStringListHybridEvent : ScriptableObject, ILemStringList, ILemrhEvent, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        private List<string> stringListValue;

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public List<string> defaultValue;

        public bool debugChange;

        [SerializeField]
        [TextArea]
        private string info;

        public List<string> StringListValue
        {
            set
            {
                stringListValue = value;

                ValueChanged();
            }
            get
            {
                return stringListValue;
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
            StringListValue = new List<string>(defaultValue);
        }

        public ISave Save()
        {
            return new LemStringListSave(StringListValue, name);
        }

        public void Load(ISave givenValue)
        {
            StringListValue = ((LemStringListSave)givenValue).stringListValue; //aggresive casting!
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface ILemStringList
    {
        List<string> StringListValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemStringListSave : ISave
    {
        public LemStringListSave(List<string> givenValue, string givenId)
        {
            stringListValue = givenValue;
            id = givenId;
        }

        public string id;
        public List<string> stringListValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
