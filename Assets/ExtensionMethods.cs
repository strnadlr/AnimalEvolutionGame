using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{



    public static class ExtensionMethods {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static float Distance(this Color color, Color other)
        {
            return Mathf.Abs(color.r - other.r) + Mathf.Abs(color.g - other.g) + Mathf.Abs(color.b - other.b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Color Average(this Color color, Color other, float lastMealsNutriValue, float nutritionalValue)
        {
            if (lastMealsNutriValue == 0) return other;
            Color result = new Color();
            float multColor = lastMealsNutriValue / (lastMealsNutriValue + nutritionalValue);
            float multOther = nutritionalValue / (lastMealsNutriValue + nutritionalValue);
            result.r = color.r * multColor + other.r * multOther;
            result.g = color.g * multColor + other.g * multOther;
            result.b = color.b * multColor + other.b * multOther;
            return result;
        }
    }
}