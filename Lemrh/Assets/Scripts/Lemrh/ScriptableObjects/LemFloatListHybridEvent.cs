/*
 * Jakub Buka³a
 * 
 * Created: 2.02.2022 r.
 * 
 * Edited: -
 *  
 * [LemFloatListHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhFloatListHybridEvent")]
    public class LemFloatListHybridEvent : ScriptableObject, ILemFloatList, ILemrhEvent, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        private List<float> floatListValue;

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public List<float> defaultValue;

        public bool debugChange;

        [SerializeField]
        [TextArea]
        private string info;

        public List<float> FloatListValue
        {
            set
            {
                floatListValue = value;

                ValueChanged();
            }
            get
            {
                return floatListValue;
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
            FloatListValue = new List<float>(defaultValue);
        }

        public ISave Save()
        {
            return new LemFloatListSave(FloatListValue, name);
        }

        public void Load(ISave givenValue)
        {
            FloatListValue = ((LemFloatListSave)givenValue).floatListValue; //aggresive casting!
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface ILemFloatList
    {
        List<float> FloatListValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemFloatListSave : ISave
    {
        public LemFloatListSave(List<float> givenValue, string givenId)
        {
            floatListValue = givenValue;
            id = givenId;
        }

        public string id;
        public List<float> floatListValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
