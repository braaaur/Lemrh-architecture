/*
 * Jakub Bukała
 * 
 * Created: ?
 * 
 * Edited: 11.04.2023 r.
 *  
 * [NewInputToSoController]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    //public class NewInputToSoController : SerializedMonoBehaviour
    public class NewInputToSoController : MonoBehaviour
    {
        public LemFloatHybridEvent attachedLemFloatSO;
        public LemVector3 attachedLemVector3SO;

        public UnityEvent performedEvent;

        public UnityEvent vectorUp;
        public UnityEvent vectorDown;
        public UnityEvent vectorRight;
        public UnityEvent vectorLeft;

        public LemCurve proportionCurve;

        public bool isHoldFeature;
        public float holdTime;
        private float currentHoldTime;
        public LemFloatHybridEvent currentHoldRatio;
        public UnityEvent holdAction;
        private bool isHold;
        private bool isHoldRisen;

        private static float vectorTreshold = 0.75f;
        private bool ticked;

        [SerializeField] private bool isDebugOn;

 

        public void SetSoValue(InputAction.CallbackContext givenValue)
        {
            if (isDebugOn)
            {
                Debug.Log("SetSoValue() - givenValue.ReadValue<float>() = " + givenValue.ReadValue<float>().ToString());
            }
            
            if (attachedLemFloatSO != null)
            {
                if (proportionCurve != null)
                {
                    attachedLemFloatSO.FloatValue = LemCalc.ReadCurve(proportionCurve.Curve, givenValue.ReadValue<float>());
                }
                else
                {
                    attachedLemFloatSO.FloatValue = givenValue.ReadValue<float>();
                }
            }
        }

        public void RiseEvent(InputAction.CallbackContext givenValue)
        {
            if (givenValue.performed)
            {
                performedEvent.Invoke();

                if (isHoldFeature)
                {
                    isHold = true;
                    isHoldRisen = false;
                    CurrentHoldTime = 0f;
                }
            }
            else
            {
                if (isHoldFeature)
                {
                    isHold = false;
                    CurrentHoldTime = 0f;
                }
            }
        }

        public void SetVector2Value(InputAction.CallbackContext givenValue)
        {
            if (givenValue.performed)
            {
                Vector2 givenVector = givenValue.ReadValue<Vector2>();

                if (attachedLemVector3SO != null)
                {
                    attachedLemVector3SO.Vector3Value = givenVector;
                }

                if (givenVector.x >= vectorTreshold)
                {
                    if (!ticked)
                    {
                        vectorRight.Invoke();
                        ticked = true;
                    }
                }
                else if (givenVector.x <= -vectorTreshold)
                {
                    if (!ticked)
                    {
                        vectorLeft.Invoke();
                        ticked = true;
                    }
                }
                else if (givenVector.y >= vectorTreshold)
                {
                    if (!ticked)
                    {
                        vectorUp.Invoke();
                        ticked = true;
                    }
                }
                else if (givenVector.y <= -vectorTreshold)
                {
                    if (!ticked)
                    {
                        vectorDown.Invoke();
                        ticked = true;
                    }
                }
                else
                {
                    ticked = false;
                }
            }
            else if (givenValue.canceled)
            {
                ticked = false;
            }
        }

        //hold
        private void Update()
        {
            if (isHoldFeature)
            {
                if (isHold)
                {
                    CurrentHoldTime = Mathf.Clamp(currentHoldTime + Time.deltaTime, 0f, holdTime);    

                    if (currentHoldTime >= holdTime && !isHoldRisen)
                    {
                        Debug.Log("Hold feature Rise!");
                        holdAction.Invoke();

                        isHoldRisen = true;
                    }
                }
            }
        }

        public float CurrentHoldTime
        {
            set
            {
                currentHoldTime = value;
                currentHoldRatio.FloatValue = currentHoldTime / holdTime;
            }
        }
    }
}
