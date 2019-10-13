/**
*Copyright(C) 2019 by #COMPANY#
*All rights reserved.
*FileName:     #SCRIPTFULLNAME#
*Author:       #AUTHOR#
*Version:      #VERSION#
*UnityVersion:#UNITYVERSION#
*Date:         #DATE#
*Description:   overrideDebug
*History:
*/
using System.Diagnostics;
public class Debug
{
    const string logColor = "white";
    const string errorColor = "red";
    const string warningColor = "yellow";

    //Conditional("LOG")]
    public static void Log(string msg)
    {
        UnityEngine.Debug.Log(GetColor(msg, logColor));
    }

    //Conditional("LOG")]
    public static void LogError(string msg)
    {
        UnityEngine.Debug.LogError(GetColor(msg, errorColor));
    }

    //Conditional("LOG")]
    public static void LogWarning(string msg)
    {
        UnityEngine.Debug.LogWarning(GetColor(msg, warningColor));
    }

    public static void LogException(System.Exception exc)
    {
        UnityEngine.Debug.LogException(exc);
    }
    static string GetColor(string msg, string color)
    {
        return $"<color={color}>{msg}</color>";
    }
}