/*
 * Jakub Bukała
 * 
 * Created: 11.08.2020 r.
 * 
 * Edited: 5.02.2022 r.
 *  
 * [LemVector3]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhVector3")]
    public class LemVector3 : ScriptableObject, ILemVector3
    {
        [SerializeField] private Vector3 vector3Value;

        public Vector3 Vector3Value
        {
            set
            {
                vector3Value = value;
            }
            get
            {
                return vector3Value;
            }
        }
    }

    public interface ILemVector3
    {
        Vector3 Vector3Value
        {
            get;
            set;
        }
    }
}
