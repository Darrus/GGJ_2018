using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// simper subscription system that is good enough for gamejam lol
/// </summary>
public class SubscriptionSystem {
    static SubscriptionSystem m_Instance;
    static public SubscriptionSystem Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new SubscriptionSystem();
            }
            return m_Instance;
        }
    }

    public class MyUnityEvent<T> : UnityEvent<T>
    {
    }

    Dictionary<string, UnityEventBase> m_FunctionWithNoParamDictionary = new Dictionary<string, UnityEventBase>();

    public void SubscribeEvent(string _eventName, UnityAction _functionCall)
    {
        UnityEventBase eventListener;
        if (m_FunctionWithNoParamDictionary.TryGetValue(_eventName, out eventListener))
        {
            UnityEvent unityEvent = eventListener as UnityEvent;
            unityEvent.AddListener(_functionCall);
            }
        else
        {
            UnityEvent unityEvent = new UnityEvent();
            unityEvent.AddListener(_functionCall);
            m_FunctionWithNoParamDictionary.Add(_eventName, unityEvent);
        }
    }

    public void SubscribeEvent<T>(string _eventName, UnityAction<T> _functionCall)
    {
        UnityEventBase eventListener;
        if (m_FunctionWithNoParamDictionary.TryGetValue(_eventName, out eventListener))
        {
            UnityEvent<T> unityEvent = eventListener as UnityEvent<T>;
            unityEvent.AddListener(_functionCall);
        }
        else
        {
            UnityEvent<T> unityEvent = new MyUnityEvent<T>();
            unityEvent.AddListener(_functionCall);
            m_FunctionWithNoParamDictionary.Add(_eventName, unityEvent);
        }
    }

    public void UnsubcribeEvent(string _eventName, UnityAction _functionCall)
    {
        UnityEventBase eventListener;
        if (m_FunctionWithNoParamDictionary.TryGetValue(_eventName, out eventListener))
        {
            (eventListener as UnityEvent).RemoveListener(_functionCall);
        }
    }

    public void UnsubcribeEvent<T>(string _eventName, UnityAction<T> _functionCall)
    {
        UnityEventBase eventListener;
        if (m_FunctionWithNoParamDictionary.TryGetValue(_eventName, out eventListener))
        {
            (eventListener as UnityEvent<T>).RemoveListener(_functionCall);
        }
    }

    public void TriggerEvent(string _eventName)
    {
        UnityEventBase eventListener;
        if (m_FunctionWithNoParamDictionary.TryGetValue(_eventName, out eventListener))
        {
            (eventListener as UnityEvent).Invoke();
        }
    }

    public void TriggerEvent<T>(string _eventName, T _param1)
    {
        UnityEventBase eventListener;
        if (m_FunctionWithNoParamDictionary.TryGetValue(_eventName, out eventListener))
        {
            (eventListener as UnityEvent<T>).Invoke(_param1);
        }
    }
}
