//Copyright (c) 2022 Serious Sim. All rights reserved
/*
 * Created by: #USER#
 * 
 * Created on: 13/12/2022 18:29:00#
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemStringVector2IntDictionary]
 * 
 * ScriptDescription: 
 */

using UnityEngine;
using System.Collections.Generic;
using System;
//using Sirenix.OdinInspector;

namespace Lemrh
{
	[CreateAssetMenu(menuName = "LemVariables/LemStringVector2IntDictionary")]
	//public class LemStringVector2IntDictionary : SerializedScriptableObject, ILemStringVector2IntDictionary, ISavable, IResetable
	public class LemStringVector2IntDictionary : ScriptableObject, ILemStringVector2IntDictionary, ISavable, IResetable
	{
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        [SerializeField] private Dictionary<string, Vector2Int> dictionaryValue;
        //[SerializeField] private Dictionary<string, Vector2Int> defaultValue;

        [SerializeField] private bool debugFromThis;

        public Dictionary<string, Vector2Int> DictionaryValue
        {
            set
            {
                if (debugFromThis)
                {
                    Debug.Log("LemStringVector2IntDictionary DictionaryValue { set } in " + name);
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

            //Debug.Log("LemStringVector2IntDictionary - Reset()");

            //if (defaultValue != null)
            //{
            //    DictionaryValue = defaultValue;
            //}
            //else
            {
                DictionaryValue = new Dictionary<string, Vector2Int>();
            }
        }

        public ISave Save()
        {
            return new LemStringVector2IntDictionarySave(DictionaryValue, name);
        }

        public void Load(ISave givenValue)
        {
            try
            {
                DictionaryValue = ((LemStringVector2IntDictionary)givenValue).dictionaryValue; //hardcore casting
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
    public interface ILemStringVector2IntDictionary
    {
        Dictionary<string, Vector2Int> DictionaryValue
        {
            get;
            set;
        }
    }


    [System.Serializable]
    public class LemStringVector2IntDictionarySave : ISave
    {
        public LemStringVector2IntDictionarySave(Dictionary<string, Vector2Int> givenValue, string givenId)
        {
            dictionaryValue = givenValue;
            id = givenId;
        }

        public string id;
        public Dictionary<string, Vector2Int> dictionaryValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}