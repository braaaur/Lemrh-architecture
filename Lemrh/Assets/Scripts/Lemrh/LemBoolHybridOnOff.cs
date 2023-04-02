/*
 * Jakub Bukała
 * 
 * Created: 12.07.2020 r.
 * 
 * Edited: 17.07.2022 r.
 *  
 * [LemBoolHybridOnOff]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.Events;

namespace Lemrh
{
    public class LemBoolHybridOnOff : MonoBehaviour
    {
        [Header("General")] 
        public LemBoolHybridEvent attachedLemBoolHybrid;
        public LemBool optionalLemBool;
        public bool isReverse;

        public GameObject targetObject;

        public bool setOnEnable;
        public bool resetVariableOnStart = false;

        public UnityEvent changedToTrue;
        public UnityEvent changedToFalse;

        [Header("Combo")]
        public LemBoolHybridEvent comboLemBoolHybrid;
        public bool comboOR;
        private void Start()
        {
            if(resetVariableOnStart)
            {
                attachedLemBoolHybrid.Reset();
            }
        }
        private void OnEnable()
        {
            if (attachedLemBoolHybrid != null)
            {
                attachedLemBoolHybrid.RegisterDelegate(BoolValueHasChanged);
            }

            if (comboLemBoolHybrid != null)
            {
                comboLemBoolHybrid.RegisterDelegate(BoolValueHasChanged);
            }

            if (setOnEnable)
            {
                SetCurrentValue();
            }
        }

        private void OnDisable()
        {
            if (attachedLemBoolHybrid != null)
            {
                attachedLemBoolHybrid.UnregisterDelegate(BoolValueHasChanged);
            }

            if (comboLemBoolHybrid != null)
            {
                comboLemBoolHybrid.UnregisterDelegate(BoolValueHasChanged);
            }
        }

        private void BoolValueHasChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("BoolValueHasChanged() in LemBoolHybridOnOff.cs - object: " + gameObject.name);
            }

            SetCurrentValue();
        }

        public void SetCurrentValue()
        {
            if (targetObject != null)
            {
                targetObject.SetActive(GetValue());
            }

            if (attachedLemBoolHybrid != null)
            {
                if (attachedLemBoolHybrid.BoolValue)
                {
                    //Debug.Log("Invoke TRUE");
                    changedToTrue.Invoke();
                }
                else
                {
                    //Debug.Log("Invoke FALSE"); 
                    changedToFalse.Invoke();
                }
            }
        }

        private bool GetValue()
        {
            bool toReturn;

            if (optionalLemBool != null)
            {
                toReturn = optionalLemBool.BoolValue;
                //if so end here
            }
            else
            {
                toReturn = attachedLemBoolHybrid.BoolValue;
            }

            if (comboOR)
            {
                toReturn = toReturn || comboLemBoolHybrid.BoolValue;
            }

            if (isReverse)
            {
                return !toReturn;
            }
            else
            {
                return toReturn;
            }
        }
    }
}
