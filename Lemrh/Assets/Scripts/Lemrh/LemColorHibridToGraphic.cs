/*
 * Jakub Buka³a
 * 
 * Created: 5.04.2022 r.
 * 
 * Edited: -
 *  
 * [LemColorHibridToGraphic]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;

namespace Lemrh
{
    public class LemColorHibridToGraphic : MonoBehaviour
{
        public LemColorHybridEvent lemColorHybrid;

        public Graphic attachedGraphic;

        public bool setOnEnable;

        private void OnEnable()
        {
            lemColorHybrid.RegisterDelegate(ColorChanged);

            if (setOnEnable)
            {
                ColorChanged(false);
            }
        }

        private void OnDisable()
        {
            lemColorHybrid.UnregisterDelegate(ColorChanged);
        }

        private void ColorChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("ValueChangeDelegate ColorChanged() in LemColorHibridToGraphic.cs- object: " + gameObject.name);
            }

            if (attachedGraphic != null)
            {
                attachedGraphic.color = lemColorHybrid.ColorValue;
            }
        }
    }
}
