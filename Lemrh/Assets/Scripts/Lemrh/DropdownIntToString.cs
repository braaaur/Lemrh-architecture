/*
 * Jakub Bukała
 * 
 * Created: 7.07.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [DropdownIntToString]
 * 
 * in Lemrh framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    //public class DropdownIntToString : SerializedMonoBehaviour
    public class DropdownIntToString : MonoBehaviour
    {
        public ILemString attachedLemrhString;

        private Dropdown myDropdown;

        private void Awake()
        {
            myDropdown = GetComponent<Dropdown>();
        }

        public void SetLemrhString(int givenIndex)
        {
            if (myDropdown != null)
            {
                attachedLemrhString.StringValue = myDropdown.options[givenIndex].text;
            }
            else
            {
                Debug.LogError("myDropdown == null in DropdownIntToString");
            }
        }
    }
}
