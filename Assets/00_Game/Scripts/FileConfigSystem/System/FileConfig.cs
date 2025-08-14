using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class FileConfig<T> : ConfigBase where T : class, new()
{
    [SerializeField] protected List<T> records = new List<T>();
    protected ConfigCompare<T> configCompare;
    
    
    public abstract void DefineConfigCompare();
    public override void HandleDataFromTextAsset(TextAsset file)
    {
        base.HandleDataFromTextAsset(file);
        records.Clear();
        DefineConfigCompare();
        HandleData(file);
    }

    private void HandleData(TextAsset file)
    {
        string[] lines = file.text.Split('\n');
        Type type = typeof(T);
        FieldInfo[] fieldInfors = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if(string.IsNullOrEmpty(line)) continue;
            T record = new T();
            string[] fields = line.Split(',');
            for(int j = 0; j < fields.Length && j < fieldInfors.Length; j++)
            {
                string fieldCleaned = CleanString(fields[j]);
                if (fieldInfors[j].FieldType.IsEnum)
                {
                    EnumHandle(fieldInfors[j], fieldCleaned, record);
                }
                else
                {
                    object convertedValue = Convert.ChangeType(fieldCleaned, fieldInfors[j].FieldType);
                    fieldInfors[j].SetValue(record, convertedValue);
                }
            }
            records.Add(record);
        }
    }

    private void EnumHandle(FieldInfo fieldInfor, string value, object record)
    {
        if (int.TryParse(value, out int intValue))
        {
            object convertedValue = Enum.ToObject(fieldInfor.FieldType, intValue);
            fieldInfor.SetValue(record, convertedValue);
        }
        else
        {
            object convertedValue = Enum.Parse(fieldInfor.FieldType, value);
            fieldInfor.SetValue(record, convertedValue);
        }
    }

    private string CleanString(string word)
    {
        if(string.IsNullOrEmpty(word)) return word;
        word = word.Trim();
        if (word.StartsWith("\"") && word.EndsWith("\""))
        {
            if(word.Length >= 2)
                word = word.Substring(1, word.Length - 2);
            else return word;
        }
        word = word.Replace("\"\"", "\"");
        word = Regex.Replace(word, @"[\n\t\r]", "");
        return word;
    }
}
