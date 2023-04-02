/*
 * Jakub Bukała
 * 
 * Created: ?
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemBool]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhBool")]
    public class LemBool : ScriptableObject, ILemBool, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")] 
        [SerializeField] private bool boolValue;

        [SerializeField] private bool defaultValue;

        public bool BoolValue
        {
            set
            {
                boolValue = value;
            }
            get
            {
                return boolValue;
            }
        }

        public void Reset()
        {
            //Debug.Log("LemBool - Reset()");

            BoolValue = defaultValue;
        }

        public ISave Save()
        {
            return new LemBoolSave(BoolValue, name);
        }

        public void Load(ISave givenValue)
        {
            BoolValue = ((LemBoolSave)givenValue).boolValue; //this works, seriously?
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
    public class LemBoolSave : ISave
    {
        public LemBoolSave(bool givenValue, string givenId)
        {
            boolValue = givenValue;
            id = givenId;
        }

        public string id;
        public bool boolValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }

    public interface ILemBool
    {
        bool BoolValue
        {
            get;
            set;
        }
    }
}
