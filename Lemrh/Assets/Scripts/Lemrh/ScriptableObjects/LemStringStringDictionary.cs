/*
 * Micha³ Owsianko
 * 
 * Created: 13.02.2022 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemStringIntDictionary]
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
    [CreateAssetMenu(menuName = "LemVariables/LemStringStringDictionary")]
    //public class LemStringStringDictionary : SerializedScriptableObject, ILemStringStringDictionary, ISavable, IResetable
    public class LemStringStringDictionary : ScriptableObject, ILemStringStringDictionary, ISavable, IResetable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        [SerializeField] private Dictionary<string, string> dictionaryValue;
        //[SerializeField] private Dictionary<string, string> defaultValue;

        public Dictionary<string, string> DictionaryValue
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

            //if (defaultValue != null)
            //{
            //    DictionaryValue = defaultValue;
            //}
            //else
            {
                DictionaryValue = new Dictionary<string, string>();
            }
        }

        public ISave Save()
        {
            return new LemStringStringDictionarySave(DictionaryValue, name);
        }

        public void Load(ISave givenValue)
        {
            try
            {
                DictionaryValue = ((LemStringStringDictionarySave)givenValue).dictionaryValue; //hardcore casting
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

    public interface ILemStringStringDictionary
    {
        Dictionary<string, string> DictionaryValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemStringStringDictionarySave : ISave
    {
        public LemStringStringDictionarySave(Dictionary<string, string> givenValue, string givenId)
        {
            dictionaryValue = givenValue;
            id = givenId;
        }

        public string id;
        public Dictionary<string, string> dictionaryValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
