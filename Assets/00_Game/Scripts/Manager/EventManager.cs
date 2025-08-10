using System;
using System.Collections.Generic;

public class EventManager : SingletonMono<EventManager>
{
    private static Dictionary<string, Action<object[]>> eventDictionary = new();

    public static void Subscribe(string eventName, Action<object[]> listener)
    {
        if (!eventDictionary.TryAdd(eventName, listener))
        {
            eventDictionary[eventName] += listener;
        }
    }

    public static void Unsubscribe(string eventName, Action<object[]> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
            if (eventDictionary[eventName] == null)
            {
                eventDictionary.Remove(eventName);
            }
        }
    }

    public static void Publish(string eventName, params object[] parameters)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke(parameters);
        }
    }
}