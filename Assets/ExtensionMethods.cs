using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{



    public static class ExtensionMethods {

        public static float Distance(this Color color, Color other)
        {
            return Mathf.Abs(color.r - other.r) + Mathf.Abs(color.g - other.g) + Mathf.Abs(color.b - other.b);
        }

        public static Color Average(this Color color, Color other)
        {
            return new Color((color.r + other.r) / 2, (color.g + other.g) / 2, (color.b + other.b) / 2);
        }
    }
}