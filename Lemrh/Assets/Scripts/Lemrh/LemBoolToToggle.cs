/*
 * Jakub Bukała
 * 
 * Created: ?
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemBoolToToggle]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    //public class LemBoolToToggle : SerializedMonoBehaviour
    public class LemBoolToToggle : MonoBehaviour
    {
        public ILemBool attachedLemBool;

        public bool setOnEnable;

        private Toggle myToggle;

        [Header("Special")]
        public bool setDefaultOnEnable; 
        public bool defaultValue;
        private bool valueSet;

        void OnEnable()
        {
            if (setOnEnable)
            {
                SetValue();
            }
        }

        public void SetValue()
        {
            //Debug.Log("LemBoolToToggle : SetValue()");

            if (GetToggle())
            {
                if (setDefaultOnEnable && !valueSet)
                {
                    attachedLemBool.BoolValue = defaultValue;
                    valueSet = true;
                }

                myToggle.isOn = attachedLemBool.BoolValue;
            }
        }

        private bool GetToggle()
        {
            if (myToggle == null)
            {
                myToggle = GetComponent<Toggle>();

                if (myToggle == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
