/*
 * Jakub Bukała
 * 
 * Created: 7.10.2020 r.
 * 
 * Edited: 8.04.2021 r.
 *  
 * [LemSpriteHibridToImageObject]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;

namespace Lemrh
{
    public class LemSpriteHibridToImageObject : MonoBehaviour
    {
        public LemSpriteHybridEvent lemSpriteHybrid;

        public Image attachedImage;

        public bool setOnEnable;
        public bool setNativeSize;

        private void OnEnable()
        {
            lemSpriteHybrid.RegisterDelegate(SpriteChanged);

            if (setOnEnable)
            {
                SpriteChanged(false);
            }
        }

        private void OnDisable()
        {
            lemSpriteHybrid.UnregisterDelegate(SpriteChanged);
        }

        private void SpriteChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("ValueChangeDelegate SpriteChanged() in LemSpriteHibridToImageObject.cs- object: " + gameObject.name);
            }

            attachedImage.sprite = lemSpriteHybrid.SpriteValue;

            if (setNativeSize)
            {
                attachedImage.SetNativeSize();
            }
        }
    }
}
