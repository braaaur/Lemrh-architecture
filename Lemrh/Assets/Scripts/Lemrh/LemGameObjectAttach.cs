/*
 * Jakub Bukała
 * 
 * Created: 25.10.2020 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemGameObjectAttach]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    //public class LemGameObjectAttach : SerializedMonoBehaviour
    public class LemGameObjectAttach : MonoBehaviour
{
        public LemGameObject attachedSO;

        private void OnEnable()
        {
            AttachGameObject();
        }

        private void OnDisable()
        {
            DetachGameObject();
        }

        
        private void AttachGameObject()
        {
            attachedSO.GameObjectValue = gameObject;
        }

        private void DetachGameObject()
        {
            attachedSO.GameObjectValue = null;
        }

    #if UNITY_EDITOR
        //[Button("Manually attach gameObject")]
        private void ManuallyAttachGameObject()
        {
            AttachGameObject();
        }
    #endif
    }
}
