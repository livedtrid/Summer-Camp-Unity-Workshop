using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SmartCities.Utils
{
    public static class ExtensionMethods
    {
        public static void ResetTransform(this Transform t)
        {
            t.position = Vector3.zero;
            t.rotation = Quaternion.identity;
            t.localScale = new Vector3(1, 1, 1);
        }

        public static void DebugTransform(this Transform t)
        {
            Debug.Log(t.gameObject.name + " position " + t.position);
            Debug.Log(t.gameObject.name + " rotation " + t.rotation);
            Debug.Log(t.gameObject.name + " localScale " + t.localScale);
        }

        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

        public static bool IsValidJson<T>(this string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput))
            {
                return false;
            }

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    return true;
                }
                catch // not valid
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsPointOverUIObject(this Vector2 pos)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return false;
            }

            PointerEventData eventPosition = new PointerEventData(EventSystem.current);
            eventPosition.position = new Vector2(pos.x, pos.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventPosition, results);

            return results.Count > 0;
        }
    }
}