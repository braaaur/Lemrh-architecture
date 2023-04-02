/*
 * Jakub Bukała
 * 
 * Created: 21.07.2020 r.
 * 
 * Edited: 25.07.2022 r.
 *  
 * [LemIntToSwitchSprite]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Lemrh
{
    public class LemIntToSwitchSprite : MonoBehaviour
    {
        [SerializeField] private LemIntHybridEvent attachedLemInt;

        [SerializeField] private Image attachedImage;

        [SerializeField] private SpriteInt[] availableSprites;

        [SerializeField] private UnityEvent spriteChange;

        [SerializeField] private Animator attachedAnimator;
        [SerializeField] private string spriteChangeTrigger = "change";

        [SerializeField] private bool setOnEnable;

        private void OnEnable()
        {
            attachedLemInt.RegisterDelegate(IntChanged);

            if (setOnEnable)
            {
                SetSprite(attachedLemInt.IntValue);
            }
        }

        private void OnDisable()
        {
            attachedLemInt.UnregisterDelegate(IntChanged);
        }

        private void IntChanged(bool isDebug)
        {
            if (isDebug)
            {
                Debug.Log("ValueChangeDelegate IntChanged() in LemIntToSwitchSprite.cs- object: " + gameObject.name);
            }

            SetSprite(attachedLemInt.IntValue);
        }

        private void SetSprite(int givenIndex)
        {
            foreach (SpriteInt spriteInt in availableSprites)
            {
                if (spriteInt.index == givenIndex)
                {
                    attachedImage.sprite = spriteInt.sprite;

                    spriteChange.Invoke();

                    if (attachedAnimator != null)
                    {
                        attachedAnimator.SetTrigger(spriteChangeTrigger);
                    }

                    return;
                }
            }
        }
    }

    [System.Serializable]
    public struct SpriteInt
    {
        public int index;
        public Sprite sprite;
    }
}
