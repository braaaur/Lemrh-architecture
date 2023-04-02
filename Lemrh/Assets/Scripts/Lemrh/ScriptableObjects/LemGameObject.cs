/*
 * Jakub Bukała
 * 
 * Created: 25.10.2020 r.
 * 
 * Edited: -
 *  
 * [LemGameObject]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhGameObject")]
    public class LemGameObject : ScriptableObject
{
        private GameObject gameObjectValue;

        public GameObject GameObjectValue
        {
            set
            {
                gameObjectValue = value;
                //Debug.Log("BoolValue Set");
            }
            get
            {
                return gameObjectValue;
            }
        }
    }
}
