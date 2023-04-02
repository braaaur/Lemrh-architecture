/*
 * Jakub Bukała
 * 
 * Created: 25.09.2020 r.
 * 
 * Edited: 8.04.2021 r.
 *  
 * [LemColorHibridToTextObject]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Lemrh
{
    public class LemColorHibridToTextObject : MonoBehaviour
    {
        public LemColorHybridEvent lemColorHybrid;

        public Text attachedText;
        public TextMeshProUGUI attachedTextTMP;

        public bool setOnEnable;

        private void OnEnable()
        {
            lemColorHybrid.RegisterDelegate(ColorChanged);

            if (setOnEnable)
            {
                ColorChanged(false);
            }
        }

        private void OnDisable()
        {
            lemColorHybrid.UnregisterDelegate(ColorChanged);
        }

        private void ColorChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("ValueChangeDelegate ColorChanged() in LemColorHibridToTextObject.cs- object: " + gameObject.name);
            }

            if (attachedText != null)
            {
                attachedText.color = lemColorHybrid.ColorValue;
            }

            if (attachedTextTMP != null)
            {
                attachedTextTMP.color = lemColorHybrid.ColorValue;
            }
        }
    }
}
