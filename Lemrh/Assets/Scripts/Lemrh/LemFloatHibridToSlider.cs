/*
 * Jakub Bukała
 * 
 * Created: 7.10.2020 r.
 * 
 * Edited: 20.11.2022 r.
 *  
 * [LemFloatHibridToSlider]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;

namespace Lemrh
{
    public class LemFloatHibridToSlider : MonoBehaviour
    {
        [SerializeField] private LemFloatHybridEvent lemFloatHybrid;
        [SerializeField] private LemFloat lemFloat;
        [SerializeField] private LemIntHybridEvent lemIntHybrid;

        [SerializeField] private Slider attachedSlider;

        [SerializeField] private bool setOnEnable;
        [SerializeField] private bool setOnEnableOnly;
        [SerializeField] private bool setNegative;

        [SerializeField] private float addThis = 0f;
        [SerializeField] private float multiplyThis = 1f;
        [SerializeField] private LemCurve optionalProportionCurve;

        [SerializeField] private bool onlyPlusValues;
        [SerializeField] private bool onlyMinusValues;

        private float currentValue;

        [SerializeField] private bool setValueInUpdate; //for LemFloat (not hybrid events)

        [SerializeField] private bool expandMaxValue;

        private void OnEnable()
        {
            if (lemFloatHybrid != null)
            {
                lemFloatHybrid.RegisterDelegate(FloatChanged);
            } 
            else if (lemIntHybrid != null)
            {
                lemIntHybrid.RegisterDelegate(FloatChanged);
            }

            if (setOnEnable)
            {
                SetSlider();
            }
        }

        private void OnDisable()
        {
            if (lemFloatHybrid != null)
            {
                lemFloatHybrid.UnregisterDelegate(FloatChanged);
            }
            else if (lemIntHybrid != null)
            {
                lemIntHybrid.UnregisterDelegate(FloatChanged);
            }
        }

        private void Update()
        {
            if (setValueInUpdate)
            {
                SetSlider();
            }
        }

        public void SetSlider()
        {
            if (lemFloatHybrid != null)
            {
                currentValue = lemFloatHybrid.FloatValue;
            }
            else if (lemFloat != null)
            {
                currentValue = lemFloat.FloatValue;
            }
            else if (lemIntHybrid != null)
            {
                currentValue = lemIntHybrid.IntValue;
            }
            else
            {
                Debug.LogError("No avaiable values form slider!");
                return;
            }

            if (setNegative)
            {
                currentValue = -currentValue;
            }

            currentValue *= multiplyThis;

            currentValue += addThis;

            if (onlyMinusValues)
            {
                currentValue = Mathf.Clamp(currentValue, Mathf.NegativeInfinity, 0f);
            }

            if (onlyPlusValues)
            {
                currentValue = Mathf.Clamp(currentValue, 0f, Mathf.Infinity);
            }

            if (expandMaxValue)
            {
                if (attachedSlider.maxValue < currentValue)
                {
                    //Debug.Log("Set max value to: " + (setNegative ? -currentValue : currentValue).ToString());
                    attachedSlider.maxValue = currentValue;
                }
            }

            //finally
            attachedSlider.value = currentValue;
        }

        private void FloatChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("ValueChangeDelegate FloatChanged() in LemFloatHibridToSlider.cs- object: " + gameObject.name);
            }

            if (setOnEnableOnly)
            {
                return;
            }

            SetSlider();
        }
    }
}
