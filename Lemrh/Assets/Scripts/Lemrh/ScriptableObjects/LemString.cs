/*
 * Jakub Bukała
 * 
 * Created: 7.07.2020 r.
 * 
 * Edited: 27.01.2022 r.
 *  
 * [LemString]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhString")]
    public class LemString : ScriptableObject, ILemString, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")] 
        public string stringValue;
        public string defaultValue;

        public string StringValue
        {
            set
            {
                stringValue = value;
            }
            get
            {
                return stringValue;
            }
        }

        public void Reset()
        {
            //Debug.Log("LemString - Reset()");

            StringValue = defaultValue;
        }

        public ISave Save()
        {
            return new LemStringSave(StringValue, name);
        }

        public void Load(ISave givenValue)
        {
            StringValue = ((LemStringSave)givenValue).stringValue; //this works, seriously?
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface ILemString
    {
        string StringValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemStringSave : ISave
    {
        public LemStringSave(string givenValue, string givenId)
        {
            stringValue = givenValue;
            id = givenId;
        }

        public string id;
        public string stringValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
