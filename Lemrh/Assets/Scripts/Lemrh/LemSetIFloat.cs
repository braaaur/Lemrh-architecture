/*
 * Jakub Buka³a
 * 
 * Created: 25.01.2022 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemSetIFloat]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    //public class LemSetIFloat : SerializedMonoBehaviour
    public class LemSetIFloat : MonoBehaviour
{
        [SerializeField] private ILemFloat attachedILemFloat;
        [SerializeField] private float setMultiplier = 1f;

        public void SetIFloat(float givenValue)
        {
            attachedILemFloat.FloatValue = givenValue * setMultiplier;
        }
    }
}
