/*
 * Jakub Bukała
 * 
 * Created: ?
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemCurve]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemCurve")]
    public class LemCurve : ScriptableObject
    {
        public AnimationCurve curve;

        public AnimationCurve Curve
        {
            set
            {
                curve = value;
            }
            get
            {
                return curve;
            }
        }

    }
}
