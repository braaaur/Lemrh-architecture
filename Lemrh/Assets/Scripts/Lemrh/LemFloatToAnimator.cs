/*
 * Jakub Bukała
 * 
 * Created: 20.07.2020 r.
 * 
 * Edited: 8.04.2021 r.
 *  
 * [LemFloatToAnimator]
 * 
 * in Lemrh Framework
 * 
 */

using UnityEngine;
using UnityEngine.Events;

namespace Lemrh
{
    public class LemFloatToAnimator : MonoBehaviour
    {
        public LemFloatHybridEvent attachedFloat;
        public LemBoolHybridEvent optionalCondition;
        public float defaultValue = 0f;

        public Animator attachedAnimator;
        public string attachedParameter;

        public float idleTime;
        private float currentIdleTime;
        private float newCurrentIdleTime;

        public UnityEvent valueChanged;
        public UnityEvent valueIdle;

        public LemBoolHybridEvent isLockedChange;

        private void OnEnable()
        {
            attachedFloat.RegisterDelegate(FloatValueChanged);
        }

        private void OnDisable()
        {
            attachedFloat.UnregisterDelegate(FloatValueChanged);
        }

        private void FloatValueChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("ValueChangeDelegate FloatValueChanged() in LemFloatToAnimator.cs- object: " + gameObject.name);
            }

            if (optionalCondition != null)
            {
                if (!optionalCondition.BoolValue)
                {
                    attachedAnimator.SetFloat(attachedParameter, defaultValue);
                    return;
                }
            }

            attachedAnimator.SetFloat(attachedParameter, attachedFloat.FloatValue);

            valueChanged.Invoke();

            currentIdleTime = 0f;
        }

        private void Update()
        {
            CheckIdleTime();
        }

        private void CheckChange()
        {
            if (isLockedChange != null)
            {
                if (!isLockedChange.BoolValue)
                {
                    valueChanged.Invoke();
                }
            }
            else
            {
                valueChanged.Invoke();
            }
        }

        private void CheckIdleTime()
        {
            newCurrentIdleTime = Mathf.Clamp(currentIdleTime + Time.deltaTime, 0f, idleTime);

            if (currentIdleTime < idleTime && newCurrentIdleTime >= idleTime)
            {
                valueIdle.Invoke();
            }

            currentIdleTime = newCurrentIdleTime;
        }
    }
}
