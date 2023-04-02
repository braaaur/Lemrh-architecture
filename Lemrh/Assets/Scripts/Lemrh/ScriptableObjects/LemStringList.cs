/*
 * Jakub Buka³a
 * 
 * Created: 10.11.2022 r.
 * 
 * Edited: -
 *  
 * [LemStringList]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhStringList")]
    public class LemStringList : ScriptableObject
    {
        //[Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        [SerializeField] private List<string> stringListValue;

        public List<string> StringListValue
        {
            set
            {
                stringListValue = value;
            }
            get
            {
                return stringListValue;
            }
        }
    }
}
