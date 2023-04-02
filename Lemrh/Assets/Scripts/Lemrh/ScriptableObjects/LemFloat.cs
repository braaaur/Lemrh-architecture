/*
 * Jakub Bukała
 * 
 * Created: ?
 * 
 * Edited: 30.01.2022 r.
 *  
 * [LemFloat]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhFloat")]
    public class LemFloat : ScriptableObject, ILemFloat, ISavable, IResetable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        [SerializeField] private float floatValue; //important to be serializable
        [SerializeField] private float defaultValue;

        public float FloatValue
        {
            set
            {
                floatValue = value;
            }
            get
            {
                return floatValue;
            }
        }
        
        public void Reset()
        {
            //Debug.Log("LemFloat - Reset()");

            FloatValue = defaultValue;
        }

        public ISave Save()
        {
            return new LemFloatSave(FloatValue, name);
        }

        public void Load(ISave givenValue)
        {
            FloatValue = ((LemFloatSave)givenValue).floatValue; //this works, seriously?
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface ILemFloat
    {
        float FloatValue
        {
            get;
            set;
        }
    }

    [System.Serializable]
    public class LemFloatSave : ISave
    {
        public LemFloatSave(float givenValue, string givenId)
        {
            floatValue = givenValue;
            id = givenId;
        }

        public string id;
        public float floatValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
