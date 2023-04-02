/*
 * Jakub Bukała
 * 
 * Created: 11.10.2020 r.
 * 
 * Edited: -
 *  
 * [PositionToLemVector3]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using Lemrh;

namespace Lemrh
{
    public class PositionToLemVector3 : MonoBehaviour
    {
        public LemVector3 objectPosition;

        void Update()
        {
            objectPosition.Vector3Value = transform.position;
        }
    }
}
