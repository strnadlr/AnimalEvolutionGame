using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace AnimalEvolution
{
    /// <summary>
    /// Provides useful extension methods as well as general purpose methods used by other parts of the code.
    /// </summary>
    public static class Methods {
        private static StreamWriter logWriter;

        /// <summary>
        /// Calculates the manhatton distance between two colors by summing the difference of their RGB values.
        /// The order of the variables does not matter.
        /// </summary>
        /// <param name="color"> is called by a Color</param>
        /// <param name="other"> the color to be comparred to the calling color</param>
        /// <returns>A manhatton distance between the two colors.</returns>
        public static float Distance(this Color color, Color other)
        {
            return Mathf.Abs(color.r - other.r) + Mathf.Abs(color.g - other.g) + Mathf.Abs(color.b - other.b);
        }

        /// <summary>
        /// Calculates a weighed average of two colors based on provided ratios.
        /// Multiplies each color by their respective ratio divided by the sum of the ratios
        /// and adds the two results.
        /// </summary>
        /// <param name="color">First color</param>
        /// <param name="other">Second color</param>
        /// <param name="ratioOfColor">Ratio of first color to include</param>
        /// <param name="ratioOfOtherColor">Ratio of second color to include</param>
        /// <returns>weighed average of the two colors provided</returns>
        public static Color Average(this Color color, Color other, float ratioOfColor, float ratioOfOtherColor)
        {
            if (ratioOfColor == 0) return other;
            Color result = new Color();
            float multColor = ratioOfColor / (ratioOfColor + ratioOfOtherColor);
            float multOther = ratioOfOtherColor / (ratioOfColor + ratioOfOtherColor);
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

        /// <summary>
        /// Creates the log file.
        /// </summary>
        public static void SetUpLog()
        {
            string path = $"gamelog{DateTime.Now.Day.ToString("00")}{DateTime.Now.Month.ToString("00")}{(DateTime.Now.Year % 100).ToString("00")}{DateTime.Now.Hour.ToString("00")}{DateTime.Now.Minute.ToString("00")}.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("File deleted.");
            }
            FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            logWriter = new StreamWriter(fs);
            Debug.Log("File setup.");
            logWriter.WriteLine($"Start log {DateTime.Now.ToLongDateString()}");
        }

        /// <summary>
        /// Writes a log into the log file.
        /// </summary>
        /// <param name="logText"></param>
        public static void Log(string logText)
        {
            logWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} : {logText}");
            Debug.Log(logText);
        }

        /// <summary>
        /// Flushes and closes the log file.
        /// </summary>
        public static void FinalizeLog()
        {
            logWriter.Flush();
            Debug.Log("Log flushed.");
            logWriter.Close();
            logWriter.Dispose();
        }
    }
}