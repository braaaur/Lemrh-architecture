/*
 * Jakub Buka³a
 * 
 * Created: 19.05.2022 r.
 * 
 * Edited: -
 *  
 * [LemEvents]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine.Events;
using UnityEngine;

namespace Lemrh
{
    [System.Serializable]
    public class LemEventInt : UnityEvent<int>
    {
    }

    [System.Serializable]
    public class LemEventVector : UnityEvent<Vector3>
    {
    }
}
