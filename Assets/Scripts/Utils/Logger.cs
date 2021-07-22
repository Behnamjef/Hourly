using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hourly.Utils
{
    public static class Logger
    {
        public static void Log(string log, LogColor color = LogColor.white)
        {
            Debug.Log($"<color={color}> {log}</color>");
        }
        
        public static void LogError(string log)
        {
            Debug.LogError($"Error! {log}");
        }
    }

    public enum LogColor
    {
        red,
        yellow,
        blue,
        orange,
        black,
        white,
    }
}