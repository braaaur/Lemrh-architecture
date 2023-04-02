/*
 * Jakub Bukała
 * 
 * Created: 21.08.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemStringHibridToTextObject]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    //public class LemStringHibridToTextObject : SerializedMonoBehaviour
    public class LemStringHibridToTextObject : MonoBehaviour
    {
        [SerializeField] private LemStringHybridEvent lemStringHybrid;

        [SerializeField] private Text attachedText;
        [SerializeField] private TextMeshProUGUI attachedTextTMP;

        [SerializeField] private bool setOnEnable;

        [SerializeField] private UnityEvent setToEmptyString;
        [SerializeField] private UnityEvent setToNotEmptyString;
        [SerializeField] private UnityEvent setToNotEmptyNotTheSameString;
        [SerializeField] private ILemBool optionalEventBlockingBool;

        [SerializeField] private bool addSufix;
        [SerializeField] private string givenSufix;        
        [SerializeField] private bool addPrefix;
        [SerializeField] private string givenPrefix;

        [SerializeField] private bool capsAll;

        [SerializeField] private LemFloat optionalAutoEraseTime;
        private float currentOptionalAutoEraseTime;
        private bool isWaitingForAutoErase;

        private void OnEnable()
        {
            lemStringHybrid.RegisterDelegate(StringChanged);

            if (setOnEnable)
            {
                StringChanged(false);
            }
        }

        private void OnDisable()
        {
            lemStringHybrid.UnregisterDelegate(StringChanged);
        }

		private void Update()
		{
            if (optionalAutoEraseTime != null)
            {
                UpdateAutoEraseTime();
            }
        }

		public void AssignText()
        {
            StringChanged(false);
        }

        private void StringChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("ValueChangeDelegate StringChanged() in LemStringHibridToTextObject.cs- object: " + gameObject.name);
            }

            SetString(lemStringHybrid.StringValue);
        }

        private void SetString(string givenString)
        {
            string stringToSet = "";
            bool isTheSame = false;

            if (addPrefix)
            {
                stringToSet += givenPrefix;
            }

            stringToSet += givenString;

            if (addSufix)
            {
                stringToSet += givenSufix;
            }

            if (capsAll)
            {
                stringToSet = stringToSet.ToUpper();
            }

            if (attachedText != null)
            {
                if (attachedText.text == stringToSet)
                {
                    isTheSame = true;
                }

                attachedText.text = stringToSet;
            }

            if (attachedTextTMP != null)
            {
                if (attachedTextTMP.text == stringToSet)
                {
                    isTheSame = true;
                }

                attachedTextTMP.text = stringToSet;
            }

            if (givenString != null && givenString.Equals(""))
            {
                if (optionalEventBlockingBool == null || !optionalEventBlockingBool.BoolValue)
                {
                    setToEmptyString.Invoke();
                }
            }
            else
            {
                //Debug.Log("setToNotEmptyString (" + givenString + ") in StringChanged() in LemStringHibridToTextObject.cs");
                if (optionalEventBlockingBool == null || !optionalEventBlockingBool.BoolValue)
                {
                    setToNotEmptyString.Invoke();

                    if (!isTheSame)
                    {
                        setToNotEmptyNotTheSameString.Invoke();
                    }

                    //optional only
                    ResetAutoEraseTime(); //best place?
                }
            }
        }

        private void ResetAutoEraseTime()
        {
            isWaitingForAutoErase = true;
            currentOptionalAutoEraseTime = 0f;
        }

        private void UpdateAutoEraseTime()
        {
            if (isWaitingForAutoErase)
            {
                currentOptionalAutoEraseTime = Mathf.Clamp(currentOptionalAutoEraseTime + Time.unscaledDeltaTime, 0f, optionalAutoEraseTime.FloatValue);

                if (currentOptionalAutoEraseTime >= optionalAutoEraseTime.FloatValue)
                {
                    SetString("");

                    isWaitingForAutoErase = false;
                }
            }
        }
    }
}
