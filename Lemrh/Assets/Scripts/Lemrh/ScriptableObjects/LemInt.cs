/*
 * Jakub Bukała
 * 
 * Created: ?
 * 
 * Edited: 27.01.2022 r.
 *  
 * [LemInt]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhInt")]
    public class LemInt : ScriptableObject, ILemInt, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")] 
        public int intValue; //important to be public (serialization)
        [SerializeField]
        private int defaultValue;

        public int IntValue
        {
            set
            {
                intValue = value;
            }
            get
            {
                return intValue;
            }
        }

        public void Reset()
        {
            //Debug.Log("LemInt - Reset()"); 
            
            IntValue = defaultValue;
        }

        public ISave Save()
        {
            return new LemIntSave(IntValue, name);
        }

        public void Load(ISave givenValue)
        {
            IntValue = ((LemIntSave)givenValue).intValue; //this works, seriously?
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface ILemInt
    {
        int IntValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemIntSave : ISave
    {
        public LemIntSave (int givenValue, string givenId)
        {
            intValue = givenValue;
            id = givenId;
        }

        public string id;
        public int intValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
