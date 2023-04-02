/*
 * Jakub Buka³a
 * 
 * Created: 16.07.2020 r.
 * 
 * Edited: -
 *  
 * [LemFloatToMaterialProperty]
 * 
 * in Lemrh Framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;

namespace Lemrh
{
    public class LemFloatToMaterialProperty : MonoBehaviour
    {
        [SerializeField] private LemFloatHybridEvent attachedFloat;
        [SerializeField] private Graphic attachedGraphic;
        [SerializeField] private string materialProperty;

       private void OnEnable()
        {
            if (attachedFloat != null)
            {
                attachedFloat.RegisterDelegate(FloatValueChanged);
            }
            else
            {
                Debug.LogError("attachedFloat == null in LemFloatToMaterialProperty.cs, gameObject = " + gameObject.name);
            }
        }

        private void OnDisable()
        {
            if (attachedFloat != null)
            {
                attachedFloat.UnregisterDelegate(FloatValueChanged);
            }
            else
            {
                Debug.LogError("attachedFloat == null in LemFloatToMaterialProperty.cs, gameObject = " + gameObject.name);
            }
        }

        private void FloatValueChanged(bool isDebug)
        {
            if (attachedGraphic != null)
            {
                if (attachedGraphic.material != null)
                {
                    attachedGraphic.material.SetFloat(materialProperty, attachedFloat.FloatValue);
                }
                else
                {
                    Debug.LogError("attachedGraphic.material == null in LemFloatToMaterialProperty.cs, gameObject = " + gameObject.name);
                }
            }
            else
            {
                Debug.LogError("attachedGraphic == null in LemFloatToMaterialProperty.cs, gameObject = " + gameObject.name);
            }
        }
    }
}
