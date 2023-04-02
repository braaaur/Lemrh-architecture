/*
 * Jakub Buka³a
 * 
 * Created: 23.07.2022 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemIntIntDictionary]
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
    [CreateAssetMenu(menuName = "LemVariables/LemIntIntDictionary")]
    //public class LemIntIntDictionary : SerializedScriptableObject, ISavable, IResetable
    public class LemIntIntDictionary : ScriptableObject, ISavable, IResetable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]

        [SerializeField] private Dictionary<int, int> dictionaryValue;

        public Dictionary<int, int> DictionaryValue
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
            //Debug.Log("LemFloat - Reset()");

            DictionaryValue = new Dictionary<int, int>();
        }

        public ISave Save()
        {
            return new LemIntIntDictionarySave(DictionaryValue, name);
        }

        public void Load(ISave givenValue)
        {
            try
            {
                DictionaryValue = ((LemIntIntDictionarySave)givenValue).dictionaryValue; //hardcore casting
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
    public class LemIntIntDictionarySave : ISave
    {
        public LemIntIntDictionarySave(Dictionary<int, int> givenValue, string givenId)
        {
            dictionaryValue = givenValue;
            id = givenId;
        }

        public string id;
        public Dictionary<int, int> dictionaryValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
