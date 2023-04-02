/*
 * Jakub Buka³a
 * 
 * Created: 9.03.2022 r.
 * 
 * Edited: -
 *  
 * [LemGradient]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemGradient")]
    public class LemGradient : ScriptableObject
    {
        [SerializeField]
        private Gradient gradientValue;

        public Gradient GradientValue
        {
            set
            {
                gradientValue = value;
            }
            get
            {
                return gradientValue;
            }
        }
    }
}
