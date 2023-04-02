/*
 * Jakub Buka³a
 * 
 * Created: 9.03.2022 r.
 * 
 * Edited: -
 *  
 * [LemColor]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemColor")]
    public class LemColor : ScriptableObject
    {
        [SerializeField]
        private Color colorValue;

        public Color ColorValue
        {
            set
            {
                colorValue = value;
            }
            get
            {
                return colorValue;
            }
        }
    }
}
