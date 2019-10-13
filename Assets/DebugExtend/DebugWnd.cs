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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugWnd : MonoBehaviour
{
    struct Log
    {
        public string msg;
        public string stackTrace;
        public LogType type;
    }


    public float openSpeed = 3f;
    public bool isRestricLogCount = false;
    public int maxLogs = 1000;

    readonly List<Log> logs = new List<Log>();
    Vector2 scrollPos;
    bool isVisble = true;
    bool collapse;

    static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color> {
            {LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
    };

    //set wnd component
    const string wndTitle = "Console";
    public float margin = 2;
    static readonly GUIContent clearLable = new GUIContent("Clear", "Clear the contents of the console.");
    static readonly GUIContent collapseLable = new GUIContent("Collapse", "Hide repeated messages.");

    Rect titleBarRect = new Rect(0, 0, 1000, 20);
    Rect wndRect;

    string marginText;

    private void OnEnable()
    {
        wndRect = new Rect(margin, margin, Screen.width / margin, Screen.height / margin);
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        logs.Add(new Log
        {
            msg = condition,
            stackTrace = stackTrace,
            type = type,
        });
        TrimExcessLogs();
    }

    void TrimExcessLogs()
    {
        if (!isRestricLogCount)
            return;

        var amountToRemove = Mathf.Max(logs.Count - maxLogs, 0);

        if (amountToRemove == 0)
            return;

        logs.RemoveRange(0, amountToRemove);
    }

    void OnGUI()
    {
        if (!isVisble)
            return;

        wndRect = GUILayout.Window(123456, wndRect, DrawConsoleWnd, wndTitle);

    }

    void DrawConsoleWnd(int wndID)
    {
        DrawLogsList();
        DrawToolBar();

        GUI.DragWindow(titleBarRect);
    }

    void DrawLogsList()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < logs.Count; i++)
        {
            Log log = logs[i];
            if (collapse && i > 1)
            {
                var previosuMsg = logs[i - 1].msg;

                if (log.msg == previosuMsg)
                    continue;
            }
            GUI.contentColor = logTypeColors[log.type];
            GUILayout.Label($"{log.msg}\n\r{log.stackTrace}");
        }
        GUILayout.EndScrollView();
    }

    void DrawToolBar()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(clearLable))
        {
            logs.Clear();
        }
        collapse = GUILayout.Toggle(collapse, collapseLable, GUILayout.ExpandWidth(false));
        GUILayout.Label("zoom", GUILayout.Width(40), GUILayout.Height(40));
        marginText = GUILayout.TextField(marginText, GUILayout.Width(30), GUILayout.Height(30));

        if (marginText != null && marginText != "" && marginText != "0")
            margin = float.Parse(marginText);
        wndRect = new Rect(margin, margin, Screen.width / margin, Screen.height / margin);
        GUILayout.EndHorizontal();
    }


}
