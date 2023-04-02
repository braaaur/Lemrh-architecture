/*
 * Jakub Buka³a
 * 
 * Created: 13.02.2022 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemStringFloatDictionary]
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
    [CreateAssetMenu(menuName = "LemVariables/LemStringFloatDictionary")]
    //public class LemStringFloatDictionary : SerializedScriptableObject, ILemStringFloatDictionary, ISavable, IResetable
    public class LemStringFloatDictionary : ScriptableObject, ILemStringFloatDictionary, ISavable, IResetable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        [SerializeField] private Dictionary<string, float> dictionaryValue;
        //[SerializeField] private Dictionary<string, float> defaultValue;

        [SerializeField] private bool debugFromThis;

        public Dictionary<string, float> DictionaryValue
        {
            set
            {
                if (debugFromThis)
                {
                    Debug.Log("LemStringFloatDictionary DictionaryValue { set } in " + name);
                }


                dictionaryValue = value;
            }
            get
            {
                return dictionaryValue;
            }
        }

        public void Reset()
        {
       
            //Debug.Log("LemStringFloatDictionary - Reset()");

            //if (defaultValue != null)
            //{
            //    DictionaryValue = defaultValue;
            //}
            //else
            {
                DictionaryValue = new Dictionary<string, float>();
            }
        }

        public ISave Save()
        {
            return new LemStringFloatDictionarySave(DictionaryValue, name);
        }

        public void Load(ISave givenValue)
        {
            try
            {
                DictionaryValue = ((LemStringFloatDictionarySave)givenValue).dictionaryValue; //hardcore casting
            }
            catch (InvalidCastException e)
            {
                Debug.LogError("InvalidCastException in " + name + "! " + e.ToString());
            }
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface ILemStringFloatDictionary
    {
        Dictionary<string, float> DictionaryValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemStringFloatDictionarySave : ISave
    {
        public LemStringFloatDictionarySave(Dictionary<string, float> givenValue, string givenId)
        {
            dictionaryValue = givenValue;
            id = givenId;
        }

        public string id;
        public Dictionary<string, float> dictionaryValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
