/**
 *Copyright(C) 2019 by #COMPANY#
 *All rights reserved.
 *FileName:     #SCRIPTFULLNAME#
 *Author:       #AUTHOR#
 *Version:      #VERSION#
 *UnityVersion:#UNITYVERSION#
 *Date:         #DATE#
 *Description:   EventCenter 
 *History:
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public delegate void EventCenterCallBack<T> (params T[] args);
public class EventCenter
{ 
    Dictionary<string, Dictionary<string, Delegate>> eventTable = new Dictionary<string, Dictionary<string, Delegate>>();

    void OnListenerAdd(string senceName, string name, Delegate callBack)
    {
        if (!eventTable.ContainsKey(senceName))
            eventTable.Add(senceName, new Dictionary<string, Delegate>());

        if (!eventTable[senceName].ContainsKey(name))
            eventTable[senceName].Add(name, null);

        Delegate d = eventTable[senceName][name];
        if (d != null && d.GetType() != callBack.GetType())
            throw new Exception(string.Format("try to add: {0} the same callBack:{1} with one Delegate,will add Delegate:{2}", StringAdd(senceName, "-", name), d.GetType(), callBack));
    }

    void OnListenerRemove(string senceName, string name, Delegate callBack)
    {
        if (eventTable.ContainsKey(senceName) && eventTable[senceName].ContainsKey(name))
        {
            Delegate d = eventTable[senceName][name];
            if (d == null)
                throw new Exception(string.Format("try to remove: {0} the null callBack", StringAdd(senceName, "-", name)));
            else if (d.GetType() != callBack.GetType())
                throw new Exception(string.Format("try to remove: {0} the different callBack:{1} with one Delegate,will remove Delegate:{2}", StringAdd(senceName, "-", name), d.GetType(), callBack));
        }
        else
            throw new Exception(string.Format("the  {0} senceName or name wasn't have callBack", StringAdd(senceName, "-", name)));
    }

    void OnListenerRemoved(string senceName, string name)
    {
        if (eventTable[senceName][name] != null)
            eventTable[senceName].Remove(name);
    }

    public void AddListener<T>(string senceName, string name, EventCenterCallBack<T> callBack)
    {      
        OnListenerAdd(senceName, name, callBack);
        eventTable[senceName][name] = (EventCenterCallBack<T>)eventTable[senceName][name] + callBack;
    }

    public void RemoveListener<T>(string senceName, string name, EventCenterCallBack<T> callBack)
    {
        OnListenerRemove(senceName, name, callBack);
        eventTable[senceName][name] = (EventCenterCallBack<T>)eventTable[senceName][name] - callBack;
        OnListenerRemoved(senceName, name);
    }

    public void RemoveAllListener(string senceName, string name)
    {
        if (eventTable.ContainsKey(senceName) && eventTable[senceName].ContainsKey(name))
        {
            OnListenerRemoved(senceName, name);
        }
    }

    public void RemoveSenceAllListener(string senceName)
    {
        if (eventTable.ContainsKey(senceName))
            eventTable.Remove(senceName);
    }

    public void Broadcast<T>(string senceName, string name, params T[] args)
    {
        if (eventTable.ContainsKey(senceName) && eventTable[senceName].ContainsKey(name))
        {
            EventCenterCallBack<T> callBack = eventTable[senceName][name] as EventCenterCallBack<T>;
            if (callBack != null)
                callBack(args);
            else
                throw new Exception(string.Format("the senceName:{0} wasn't have callBack,please check{1} ", StringAdd(senceName, "-", name), typeof(T)));
        }
    }

    string StringAdd(params string[] str)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(str);
        return stringBuilder.ToString();
    }
}
