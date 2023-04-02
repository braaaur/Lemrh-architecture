/*
 * Jakub Bukała
 * 
 * Created: 2.10.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemBoolSwitchController]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    //public class LemBoolSwitchController : SerializedMonoBehaviour
    public class LemBoolSwitchController : MonoBehaviour
    {
        public ILemBool attachedLemBool;

        public ILemBool optionalCondition;

        public void SwitchLemBool()
        {
            if (optionalCondition != null)
            {
                if (!optionalCondition.BoolValue)
                {
                    return;
                }
            }

            attachedLemBool.BoolValue = !attachedLemBool.BoolValue;
        }
    }
}
