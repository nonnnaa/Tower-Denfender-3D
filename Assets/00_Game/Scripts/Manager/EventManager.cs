using System;
using System.Collections.Generic;


public enum EventKey
{
    TriggerAnim
}

public class EventManager : SingletonMono<EventManager>
{
    private static Dictionary<EventKey, Action<object[]>> eventDictionary = new();

    public void Subscribe(EventKey eventKey, Action<object[]> listener)
    {
        if (!eventDictionary.TryAdd(eventKey, listener))
        {
            eventDictionary[eventKey] += listener;
        }
    }

    public void Unsubscribe(EventKey eventKey, Action<object[]> listener)
    {
        if (eventDictionary.ContainsKey(eventKey))
        {
            eventDictionary[eventKey] -= listener;
            if (eventDictionary[eventKey] == null)
            {
                eventDictionary.Remove(eventKey);
            }
        }
    }

    public void Publish(EventKey eventKey, params object[] parameters)
    {
        if (eventDictionary.ContainsKey(eventKey))
        {
            eventDictionary[eventKey]?.Invoke(parameters);
        }
    }
}