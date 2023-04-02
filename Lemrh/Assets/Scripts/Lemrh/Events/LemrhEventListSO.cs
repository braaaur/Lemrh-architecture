/*
 * Jakub Buka³a
 * 
 * Created: 29.07.2022 r.
 * 
 * Edited: -
 *  
 * [LemrhEventListSO]
 * 
 * in LEMRH framework
 * 
 */

using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemEvents/LemrhEventListSO")]
    public class LemrhEventListSO : ScriptableObject //SerializedScriptableObject
    {
        [SerializeField] private List<LemrhEvent> events = new List<LemrhEvent>();

        public List<LemrhEvent> Events
        {
            get
            {
                return events;
            }
            set
            {
                events = value;
            }
        }
    }
}
