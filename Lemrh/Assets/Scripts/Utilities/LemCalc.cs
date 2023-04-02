/*
 * Jakub Bukała
 * 
 * Created: 4.09.2020 r.
 * 
 * Edited: 22.03.2023 r.
 *  
 * [LemCalc]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;

namespace Lemrh
{
    public static class LemCalc
    {
        [Header("Public values")]
        public static readonly float msToMiles = 2.23693629f;
        public static readonly float milesToMs = 0.44704f;
        public static readonly float metersToMiles = 0.00062137f;
        public static readonly float kphToMph = 0.62137f;

        [Header("Internal values")]
        private static Keyframe[] tempKeys;
        private static int lastSeed;
        private static int tempSeed;
        private static float stableFrameTime = 1f / 30f;

        private static float curveReadResult;

        public static int RandomRange(int minInclusive, int maxExclusive)
        {
            ResetSeed();

            return Random.Range(minInclusive, maxExclusive);
        }

        public static float RandomRange(float minInclusive, float maxInclusive)
        {
            ResetSeed();

            return Random.Range(minInclusive, maxInclusive);
        }

        private static void ResetSeed() //since some 3rd party plugins set seed, it has to be reseted, but not multiple times in the same Tick
        {
            tempSeed = (int)System.DateTime.Now.Ticks;

            if (lastSeed != tempSeed)
            {
                lastSeed = tempSeed;
                Random.InitState(lastSeed);
            }
        }

        public static float ReadCurve(AnimationCurve givenCurve, float givenValue) //should be used everywhere!
        {
            /* //this approach is safe and kinda neat but sadly involves GC from taking keys
            tempKeys = givenCurve.keys;

            if (givenValue > tempKeys[tempKeys.Length - 1].time)
            {
                return tempKeys[tempKeys.Length - 1].value;
            }
            else if (givenValue < tempKeys[0].time)
            {
                return tempKeys[0].value;
            }
            else
            {
                return givenCurve.Evaluate(givenValue);
            }
            */

            givenCurve.preWrapMode = WrapMode.ClampForever;
            givenCurve.postWrapMode = WrapMode.ClampForever;

            curveReadResult = givenCurve.Evaluate(givenValue);

            if (float.IsNaN(curveReadResult))
            {
                //Debug.LogWarning("approachSpeed NaN fixed");
                curveReadResult = 0f;
            }

            return curveReadResult;
        }

        public static float ReadCurve(LemCurve givenCurve, float givenValue) //should be used everywhere!
        {
            return ReadCurve(givenCurve.Curve, givenValue);
        }

        public static float CurveEnd(AnimationCurve givenCurve)
        {
            tempKeys = givenCurve.keys;

            return tempKeys[tempKeys.Length - 1].time;
        }

        /*
        public static float GetCurveMaxTime(AnimationCurve givenCurve)
        {
            tempKeys = givenCurve.keys;

            return tempKeys[tempKeys.Length - 1].time;
        }
        */

        public static bool CheckTreshold(float newValue, float oldValue, float tresholdValue, bool isRising)
        {
            if (isRising && newValue >= tresholdValue && oldValue < tresholdValue)
            {
                return true;
            }

            if (!isRising && newValue <= tresholdValue && oldValue > tresholdValue)
            {
                return true;
            }

            return false;
        }

        public static bool CheckTresholdConst(float newValue, float oldValue, float tresholdConst)
        {
            if (Mathf.FloorToInt(newValue / tresholdConst) != Mathf.FloorToInt(oldValue / tresholdConst))
            {
                return true;
            }

            return false;
        }

        public static string FormatSecondsToMinsAndSecs(float givenValue, string givenSeparator, bool allowMinusTime = false)
        {
            int minutes = Mathf.FloorToInt(givenValue / 60f);
            int seconds = Mathf.FloorToInt(givenValue % 60f);

            if (!allowMinusTime)
            {
                minutes = Mathf.Clamp(minutes, 0, 9999);
                seconds = Mathf.Clamp(seconds, 0, 9999);
            }

            return minutes.ToString("00") + givenSeparator + seconds.ToString("00");
        }

        //not tested
        public static string FormatSecondsToHoursAndMins(float givenValue, string givenSeparator)
        {
            int hours = Mathf.FloorToInt(givenValue / 3600f);
            int mins = Mathf.FloorToInt(givenValue % 3600f) / 60;

            return hours.ToString("00") + givenSeparator + mins.ToString("00");
        }

        public static string GetPercentageString(float givenValue)
        {
            return Mathf.RoundToInt(givenValue * 100f).ToString() + "%";
        }

        public static bool Interval(ref float currentInterval, float givenInterval)
        {
            currentInterval = Mathf.Clamp(currentInterval + Time.deltaTime, 0f, givenInterval);

            if (currentInterval >= givenInterval)
            {
                currentInterval = 0f;
                return true;
            }

            return false;
        }

        public static bool IntervalUnscaled(ref float currentInterval, float givenInterval)
        {
            currentInterval = Mathf.Clamp(currentInterval + Time.unscaledDeltaTime, 0f, givenInterval);

            if (currentInterval >= givenInterval)
            {
                currentInterval = 0f;
                return true;
            }

            return false;
        }

        public static Vector3 GetNearPoint(Vector3 targetPosition, Vector3 startPosition, float proxValue)
        {
            return targetPosition + ((startPosition - targetPosition).normalized * proxValue);
        }

        //layers
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            //faster version
            foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layer;
            }
        }

        public static LayerMask AddLayerToMask(LayerMask baseLayerMask, string layerName)
        {
            return baseLayerMask | (1 << LayerMask.NameToLayer(layerName));
        }

        public static LayerMask AddLayerToMask(LayerMask baseLayerMask, LayerMask layerToAdd)
        {
            return baseLayerMask | (1 << layerToAdd);
        }

        public static LayerMask RemoveLayerFromMask(LayerMask baseLayerMask, string layerName)
        {
            return baseLayerMask & ~(1 << LayerMask.NameToLayer(layerName));
        }

        public static LayerMask RemoveLayerFromMask(LayerMask baseLayerMask, LayerMask layerToRemove)
        {
            return baseLayerMask & ~(1 << layerToRemove);
        }

        public static bool IsLayerInLayerMask(LayerMask givenLayermask, int givenLayer)
        {
            if (givenLayermask == (givenLayermask | (1 << givenLayer)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckTimeSecoundChange(float newPlayerTime, float oldPlayerTime)
        {
            if (Mathf.Floor(newPlayerTime) != Mathf.Floor(oldPlayerTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {

            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parallel
            if (Mathf.Abs(planarFactor) < 0.0001f
                    && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                        / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineVec1 * s);
                return true;
            }
            else
            {
                intersection = Vector3.zero;
                return false;
            }
        }

        public static Vector3 SickLineLineIntersection(Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
        {
            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                        / crossVec1and2.sqrMagnitude;

            return linePoint1 + (lineVec1 * s);
        }

        public static Vector3 Brazier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return (1.0f - t) * (1.0f - t) * p0
            + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
        }

        public static bool IsBehind(Transform caller, Transform other)
        {
            if (Vector3.Angle((other.position - caller.position), caller.forward) > 90f)
            {
                return true;
            }

            return false;
        }

        private static float ScaleFromOne(float givenValue, float givenScale)
        {
            float difference = givenValue - 1f;

            difference *= givenScale;

            return Mathf.Clamp(1f + difference, 0.001f, 1f);
        }

        public static string FormatHourFloatToHoursAndMins(float givenValue, string givenSeparator, bool roundMinsToTens = false)
        {
            int hours = Mathf.FloorToInt(givenValue);
            int mins = Mathf.FloorToInt((givenValue % 1f) * 60f);

            if (roundMinsToTens)
            {
                mins = Mathf.RoundToInt((float)mins / 10f) * 10;
            }

            return hours.ToString("00") + givenSeparator + mins.ToString("00");
        }

        public static string ConvertToRoman(int givenValue)
        {
            if ((givenValue < 0) || (givenValue > 3999))
            {
                Debug.Log("LemCalc.cs - ConvertToRoman() value must be between 1 and 3999");
                return string.Empty;
            }

            if (givenValue < 1) return string.Empty;
            if (givenValue >= 1000) return "M" + ConvertToRoman(givenValue - 1000);
            if (givenValue >= 900) return "CM" + ConvertToRoman(givenValue - 900);
            if (givenValue >= 500) return "D" + ConvertToRoman(givenValue - 500);
            if (givenValue >= 400) return "CD" + ConvertToRoman(givenValue - 400);
            if (givenValue >= 100) return "C" + ConvertToRoman(givenValue - 100);
            if (givenValue >= 90) return "XC" + ConvertToRoman(givenValue - 90);
            if (givenValue >= 50) return "L" + ConvertToRoman(givenValue - 50);
            if (givenValue >= 40) return "XL" + ConvertToRoman(givenValue - 40);
            if (givenValue >= 10) return "X" + ConvertToRoman(givenValue - 10);
            if (givenValue >= 9) return "IX" + ConvertToRoman(givenValue - 9);
            if (givenValue >= 5) return "V" + ConvertToRoman(givenValue - 5);
            if (givenValue >= 4) return "IV" + ConvertToRoman(givenValue - 4);
            if (givenValue >= 1) return "I" + ConvertToRoman(givenValue - 1);

            Debug.Log("LemCalc.cs - ConvertToRoman() Impossible state reached!");
            return string.Empty;
        }

        public static bool IsStableFrame()
        {
            if (Time.unscaledDeltaTime <= stableFrameTime)
            {
                return true;
            }
            else
            {
                return false;
            }
		}

	}
}
