/*
 * Jakub Buka³a
 * 
 * Created: 23.07.2022 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemIntFloatDictionary]
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
    [CreateAssetMenu(menuName = "LemVariables/LemIntFloatDictionary")]
    //public class LemIntFloatDictionary : SerializedScriptableObject, ISavable, IResetable
    public class LemIntFloatDictionary : ScriptableObject, ISavable, IResetable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        
        [SerializeField] private Dictionary<int, float> dictionaryValue;

        public Dictionary<int, float> DictionaryValue
        {
            set
            {
                dictionaryValue = value;
            }
            get
            {
                return dictionaryValue;
            }
        }

        public void Reset()
        {
            //Debug.Log("LemIntFloatDictionary - Reset()");

            DictionaryValue = new Dictionary<int, float>();
        }

        public ISave Save()
        {
            return new LemIntFloatDictionarySave(DictionaryValue, name);
        }

        public void Load(ISave givenValue)
        {
            try
            {
                DictionaryValue = ((LemIntFloatDictionarySave)givenValue).dictionaryValue; //hardcore casting
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

    [System.Serializable]
    public class LemIntFloatDictionarySave : ISave
    {
        public LemIntFloatDictionarySave(Dictionary<int, float> givenValue, string givenId)
        {
            dictionaryValue = givenValue;
            id = givenId;
        }

        public string id;
        public Dictionary<int, float> dictionaryValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
