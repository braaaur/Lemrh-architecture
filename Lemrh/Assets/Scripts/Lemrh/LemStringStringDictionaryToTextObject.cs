/*
 * Jakub Buka³a
 * 
 * Created: 17.05.2022 r.
 * 
 * Edited: 26.10.2022 r.
 *  
 * [LemStringStringDictionaryToTextObject]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;
using TMPro;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    public class LemStringStringDictionaryToTextObject : MonoBehaviour
    {
        [SerializeField] private LemStringStringDictionary attachedDictionary;
        [SerializeField] private string attachedKey;
        [SerializeField] private LemString attachedKeyLem;

        [SerializeField] private Text attachedText;
        [SerializeField] private TextMeshProUGUI attachedTextTMP;
        [SerializeField] private bool setOnEnable = true;

        [SerializeField] private bool addSufix;
        [SerializeField] private string givenSufix;

        [SerializeField] private bool capsAll;

        private void OnEnable()
        {
            if (setOnEnable)
            {
                SetText();
            }
        }

        private void SetText()
        {
            if (attachedText != null)
            {
                attachedText.text = StringToSet;
            }

            if (attachedTextTMP != null)
            {
                attachedTextTMP.text = StringToSet;
            }
        }

        private string StringToSet
        {
            get
            {
                string toReturn = "";

                if (attachedDictionary.DictionaryValue.ContainsKey(AttachedKey))
                {
                    toReturn = attachedDictionary.DictionaryValue[AttachedKey];

                    if (addSufix)
                    {
                        toReturn += givenSufix;
                    }

                    if (capsAll)
                    {
                        toReturn = toReturn.ToUpper();
                    }
                }
                else
                {
                    Debug.LogError("attachedKey (" + AttachedKey + ") not found in Dictionary attachedDictionary in LemStringStringDictionaryToTextObject.cs !");
                }

                return toReturn;
            }
        }

        private string AttachedKey
        {
            get
            {
                if (attachedKeyLem != null)
                {
                    return attachedKeyLem.StringValue;
                }
                else
                {
                    return attachedKey;
                }
            }
        }
    }
}
