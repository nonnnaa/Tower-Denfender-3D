using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ConfigCompare<T> : IComparer where T : class, new()
{
    List<FieldInfo> fields;
    public ConfigCompare(params string[] keysS)
    {
        Type type = typeof(T);
        foreach (string key in keysS)
        {
            fields.Add(type.GetField(key));
        }
    }
    public int Compare(object x, object y)
    {
        int result = 0;
        foreach (FieldInfo field in fields)
        {
            object value1 = field.GetValue(x);
            object value2 = field.GetValue(y);
            result = ((IComparable)value1).CompareTo(value2);
            if (result != 0) break;
        }
        return result;
    }
}
