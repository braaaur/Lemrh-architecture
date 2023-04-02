/*
 * Jakub Buka³a
 * 
 * Created: 30.03.2022 r.
 * 
 * Edited: -
 *  
 * [LemEnableSetController]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    public class LemEnableSetController : MonoBehaviour
    {
        public LemIntHybridEvent attachedIndex;

        public GameObject[] attachedSets;

        private void OnEnable()
        {
            attachedIndex.RegisterDelegate(IntChanged);
        }

        private void OnDisable()
        {
            attachedIndex.UnregisterDelegate(IntChanged);
        }

        private void IntChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("ValueChangeDelegate IntChanged() in LemEnableSetController.cs- object: " + gameObject.name);
            }

            Set(attachedIndex.IntValue);
        }

        private void Set(int givenIndex)
        {
            if (attachedSets != null && attachedSets.Length > givenIndex)
            {
                foreach(GameObject set in attachedSets)
                {
                    set.SetActive(false);
                }

                attachedSets[givenIndex].SetActive(true);
            }
        }
    }
}
