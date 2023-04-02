/*
 * Marcin Krzeszowiec
 * 
 * Created: 17.11.2020 r.
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
    public class LemVector3ToPosition : MonoBehaviour
    {
        [SerializeField] private Vector3 Offset = Vector3.zero;
        public LemVector3 objectPosition;
        private Vector3 offsetLS = Vector3.zero;
        
        void Update()
        {            
            offsetLS =transform.right * Offset.x + transform.up * Offset.y +transform.forward * Offset.z;
            transform.position = objectPosition.Vector3Value + offsetLS;
        }
    }
}
