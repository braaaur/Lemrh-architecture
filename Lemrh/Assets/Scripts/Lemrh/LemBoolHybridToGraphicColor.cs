/*
 * © Jakub Buka³a, Serious Sim
 * 
 * Created: 8.03.2023 r.
 * 
 * Edited: -
 *  
 * [LemBoolHybridToGraphicColor]
 * 
 * in Vanishing Point
 * 
 */

using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

namespace Lemrh
{
	public class LemBoolHybridToGraphicColor : MonoBehaviour
	{
		[Header("Input")]
		[SerializeField] private LemBoolHybridEvent attachedLemBoolHybridEvent;

		[Header("Attachments")]
		[SerializeField] private Graphic attachedGraphic;

		[Header("Parameters")]
		[SerializeField] private bool setOnEnable = true;
		[SerializeField] private LemColor trueColor;
		[SerializeField] private LemColor falseColor;

		private void OnEnable()
		{
			attachedLemBoolHybridEvent.RegisterDelegate(AttachedLemBoolHybridEventChanged);

			if (setOnEnable)
			{
				SetColor();
			}
		}

		private void OnDisable()
		{
			attachedLemBoolHybridEvent.UnregisterDelegate(AttachedLemBoolHybridEventChanged);
		}

		private void AttachedLemBoolHybridEventChanged(bool isDebug)
		{
			SetColor();
		}

		private void SetColor()
		{
			attachedGraphic.color = attachedLemBoolHybridEvent.BoolValue ? trueColor.ColorValue : falseColor.ColorValue;
		}
	}
}
