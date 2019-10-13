/**
 *Copyright(C) 2019 by #COMPANY#
 *All rights reserved.
 *FileName:     #SCRIPTFULLNAME#
 *Author:       #AUTHOR#
 *Version:      #VERSION#
 *UnityVersion:#UNITYVERSION#
 *Date:         #DATE#
 *Description:   
 *History:
*/
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using System.Reflection;
using UnityEditorInternal;

public class DebugSetting
{
    readonly static string Debug_Path = $"{Application.dataPath}/Debug.cs";

    [MenuItem("Tools/ChangeDebug/Off", false, 0)]
    static void DebugOff()
    {
        PlayerSettings.usePlayerLog = false;
        ChangeDebug(false);
    }

    [MenuItem("Tools/ChangeDebug/On", false, 0)]
    static void DebugOn()
    {
        PlayerSettings.usePlayerLog = true;
        ChangeDebug(true);
    }

    /// <summary>
    ///更改特性
    /// </summary>
    /// <param name="isOn"></param>
    static void ChangeDebug(bool isOn)
    {
        string files = FileTools.ReadFileString(Debug_Path);
        if (isOn)
        {
            Regex r = new Regex(@"\[");
            files = r.Replace(files, "//");
        }
        else
        {
            Regex r = new Regex("//");
            files = r.Replace(files, "[");
        }

        FileTools.CreateFileString(Debug_Path, files);
    }

    [OnOpenAsset(-1)]
    static bool OnOpenAsset(int instanceID, int line)
    {
       
        var statckTrack = GetStackTrace();
        var fileNames = statckTrack?.Split('\n');
        for (int i = 0; i < fileNames.Length; i++)
        {   
            if (fileNames[i].IndexOf("at")!=-1&&fileNames[i].IndexOf("Debug:Log")==-1)
            {               
                //从两个字符串中间取得路径 并去除空格 再去除多余Assets
                var classPath = fileNames[i].GetLimitStr("(at", ":").Replace(" ","").Replace("Assets","");
                var logLine= fileNames[i].GetLimitStr(":", ")").ToInt();

                InternalEditorUtility.OpenFileAtLineExternal(Application.dataPath+ classPath, logLine);
                return true;                          
            }
           
        }
        return false;
    }


    static string GetStackTrace()
    {
        //UnityEditor.ConsoleWindow
        var consoleWndType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
        //找到成员
        var fieldInfo = consoleWndType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
        var consoleWnd = fieldInfo.GetValue(null);
        if (consoleWnd == null)
            return "";
        //// 如果console窗口时焦点窗口的话，获取stacktrace
        if ((object)consoleWnd == EditorWindow.focusedWindow)
        {
            fieldInfo = consoleWndType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
            return fieldInfo.GetValue(consoleWnd).ToString();
        }
        return "";
    }




}
