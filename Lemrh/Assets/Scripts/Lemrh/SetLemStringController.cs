/*
 * Jakub Bukała
 * 
 * Created: 22.09.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [SetLemStringController]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    public class SetLemStringController : MonoBehaviour
    {
        public LemStringHybridEvent attachedText;

        public bool setOnEnable;

        public bool timerToEmpty;
        public float givenTime;
        private bool countdownToEmpty;
        private float currentCountdown;
        private string setValue;

        private void OnEnable()
        {
            if (setOnEnable)
            {
                SetMe();
            }
        }

        private void Update()
        {
            if (countdownToEmpty)
            {
                currentCountdown = Mathf.Clamp(currentCountdown - Time.deltaTime, 0f, givenTime);

                if (currentCountdown <= 0f)
                {
                    BackToEmpty();
                    countdownToEmpty = false;
                }
            }
        }

        private void SetMe()
        {
            if (timerToEmpty)
            {
                countdownToEmpty = true;
                currentCountdown = givenTime;
            }
        }

        private void BackToEmpty()
        {
            if (attachedText != null)
            {
                if (attachedText.StringValue == setValue)
                {
                    attachedText.StringValue = "";
                }
            }
            else
            {
                Debug.LogError("attachedText == null in BackToEmpty() in SetLemStringController.cs");
            }
        }
    }
}
