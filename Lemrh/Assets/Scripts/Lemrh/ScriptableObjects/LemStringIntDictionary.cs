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
    [CreateAssetMenu(menuName = "LemVariables/LemStringIntDictionary")]
    //public class LemStringIntDictionary : SerializedScriptableObject, ILemStringIntDictionary, ISavable, IResetable
    public class LemStringIntDictionary : ScriptableObject, ILemStringIntDictionary, ISavable, IResetable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        [SerializeField] private Dictionary<string, int> dictionaryValue;

        //[SerializeField] private Dictionary<string, int> defaultValue;

        public Dictionary<string, int> DictionaryValue
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
            //Debug.Log("LemStringIntDictionary - Reset()");

            //if (defaultValue != null)
            //{
            //    DictionaryValue = defaultValue;
            //}
            //else
            {
                DictionaryValue = new Dictionary<string, int>();
            }
        }

        public ISave Save()
        {
            return new LemStringIntDictionarySave(DictionaryValue, name);
        }

        public void Load(ISave givenValue)
        {
            try
            {
                DictionaryValue = ((LemStringIntDictionarySave)givenValue).dictionaryValue; //hardcore casting
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

    public interface ILemStringIntDictionary
    {
        Dictionary<string, int> DictionaryValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemStringIntDictionarySave : ISave
    {
        public LemStringIntDictionarySave(Dictionary<string, int> givenValue, string givenId)
        {
            dictionaryValue = givenValue;
            id = givenId;
        }

        public string id;
        public Dictionary<string, int> dictionaryValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
