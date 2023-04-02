/*
 * Jakub Bukała
 * 
 * Created: 20.07.2020 r.
 * 
 * Edited: 8.04.2021 r.
 *  
 * [LemFloatToAlpha]
 * 
 * in Lemrh Framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;

namespace Lemrh
{

    public class LemFloatToAlpha : MonoBehaviour
    {
        public Graphic attachedGraphics;

        public LemFloatHybridEvent alphaValue;

        private Color tempColor;

        public bool useCurve;
        public LemCurve speedToAlphaCurve;

        private void OnEnable()
        {
            alphaValue.RegisterDelegate(AlphaValueChanged);
        }

        private void OnDisable()
        {
            alphaValue.UnregisterDelegate(AlphaValueChanged);
        }

        private void AlphaValueChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("ValueChangeDelegate AlphaValueChanged() in LemFloatToAlpha.cs- object: " + gameObject.name);
            }

            tempColor = attachedGraphics.color;
            tempColor.a = useCurve ? speedToAlphaCurve.curve.Evaluate(alphaValue.FloatValue) : alphaValue.FloatValue;
            attachedGraphics.color = tempColor;
        }
    }
}
