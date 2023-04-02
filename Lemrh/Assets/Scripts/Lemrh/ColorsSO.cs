/*
 * Jakub Bukała
 * 
 * Created: 23.07.2020 r.
 * 
 * Edited: -
 *  
 * [ColorsSO]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/ColorsSO")]
    public class ColorsSO : ScriptableObject
    {
        public Color[] colors;
    }
}
