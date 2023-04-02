/*
 * Jakub Bukała
 * 
 * Created: ?
 * 
 * Edited: 5.07.2020 r.
 *  
 * [LemFloatToTextController]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;

namespace Lemrh
{
    public class LemFloatToTextController : MonoBehaviour
    {
        public LemFloat attachedFloat;
        private Text attachedText;
        public string formatter = "n0";
        public string prefixString;

        void Awake()
        {
            attachedText = GetComponent<Text>();
        }

        void Update()
        {
            if (attachedText != null && attachedFloat != null)
            {
                attachedText.text = prefixString + attachedFloat.FloatValue.ToString(formatter);
            }
        }
    }
}
