using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

namespace AnimalEvolution
{

    public static class Methods {
        private static StreamWriter logWriter;

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

        /// <summary>
        /// Calculate a direction paralel to the surface aiming towards the target.
        /// </summary>
        /// <param name="targetToSetDirectionTo">Target's position</param>
        /// <param name="newNormal">Normal of the surface</param>
        /// <returns></returns>
        public static Vector3 CalculateDirection(this Transform transform, Vector3 targetToSetDirectionTo, Vector3 newNormal)
        {
            //return target - position;
            Plane surface = new Plane(newNormal, transform.position);
            Vector3 normalOfCrossplane = Vector3.Cross((targetToSetDirectionTo - transform.position).normalized, newNormal);
            Vector3 result = Vector3.Cross(newNormal, normalOfCrossplane);
            return result;
        }

        public static void SetUpLog()
        {
            string path = "gamelog.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("File deleted.");
            }
            FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            logWriter = new StreamWriter(fs);
            Debug.Log("File setup.");
            logWriter.WriteLine($"Start log {System.DateTime.Now.ToLongDateString()}");
        }
        public static void Log(string logText)
        {
            logWriter.WriteLine($"{System.DateTime.Now.ToLongTimeString()} : {logText}");
            Debug.Log(logText);
        }

        public static void FlushLog()
        {
            logWriter.Flush();
            Debug.Log("Log flushed.");
            logWriter.Dispose();
        }
    }
}