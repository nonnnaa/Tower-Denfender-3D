using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Local data manager that handles CRUD operations (Create, Read, Update, Delete)
/// for UserData stored in JSON format inside Application.persistentDataPath.
/// </summary>
public class DataModelLocal
{
    private UserData userData;  // The main data container for the game
    private string savePath => Path.Combine(Application.persistentDataPath, "UserData.json");

    /// <summary>
    /// Creates new UserData if it does not already exist.
    /// Initializes UserInfo, UserInventory, and sample UnitData.
    /// </summary>
    public void CreateData(Action callback)
    {
        if (!CheckData()) // Only create if no data file exists
        {
            Debug.Log("[CreateData] No existing data found. Creating new user data...");

            // --- Create initial UserInfo ---
            UserInfo info = new UserInfo();
            info.SetUuid("1");
            info.SetName("Brayang");
            info.SetLevel(1);
            info.SetExp(0);

            // --- Create initial UserInventory ---
            UserInventory userInventory = new UserInventory();
            userInventory.SetGold(1000);

            // --- Add some Units into dictionary ---
            SerializableDictionary<string, UnitData> dic = new SerializableDictionary<string, UnitData>();

            UnitData unit1 = new UnitData();
            unit1.SetId(1);
            unit1.SetLevel(1);
            dic.Add("K_1", unit1);

            UnitData unit2 = new UnitData();
            unit2.SetId(2);
            unit2.SetLevel(1);
            dic.Add("K_2", unit2);

            userInventory.SetDicUnit(dic);

            // --- Assign everything to UserData ---
            userData = new UserData();
            userData.SetUserInfo(info);
            userData.SetCurMission(1);
            userData.SetUserInventory(userInventory);
            userData.SetMissionDatas(new List<MissionData>());

            SaveData();
        }
        else
        {
            Debug.Log("[CreateData] Data already exists, skipping creation.");
        }

        callback?.Invoke();
    }

    /// <summary>
    /// Checks if the data file exists and loads it into memory if available.
    /// </summary>
    private bool CheckData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            userData = JsonUtility.FromJson<UserData>(json);
            return true;
        }
        Debug.LogWarning("[CheckData] No save file found.");
        return false;
    }

    /// <summary>
    /// Saves current userData into JSON file.
    /// </summary>
    private void SaveData()
    {
        if (userData == null)
        {
            Debug.LogError("[SaveData] userData is null! Nothing to save.");
            return;
        }
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(savePath, json);
    }

    #region C-R-U-D

    /// <summary>
    /// Reads a value from userData using reflection and path navigation.
    /// Example path: "userInventory/dicUnit"
    /// </summary>
    public T Read<T>(string path)
    {
        if (userData == null)
        {
            Debug.LogError("[Read] userData is null! Make sure data is created or loaded first.");
            return default;
        }

        object data = null;
        string[] s = path.Split('/');
        List<string> paths = new List<string>(s);
        ReadDataByPath(paths, userData, out data);

        if (data == null)
        {
            Debug.LogWarning($"[Read] No data found at path: {path}");
            return default;
        }
        
        return (T)data;
    }

    /// <summary>
    /// Reads a dictionary entry by key from userData.
    /// Example path: "userInventory/dicUnit" + key "K_1"
    /// </summary>
    public T ReadDataKey<T>(string path, string key)
    {
        if (userData == null)
        {
            Debug.LogError("[ReadDataKey] userData is null!");
            return default;
        }

        string[] s = path.Split('/');
        List<string> paths = new List<string>(s);
        ReadDataByPath(paths, userData, out var data);

        if (data == null)
        {
            Debug.LogWarning($"[ReadDataKey] No dictionary found at path: {path}");
            return default;
        }

        SerializableDictionary<string, T> unitDictionary = (SerializableDictionary<string, T>)data;
        if (unitDictionary.TryGetValue(key, out T outData))
        {
            return outData;
        }

        Debug.LogWarning($"[ReadDataKey] Key '{key}' not found in dictionary at path: {path}");
        return default;
    }

    /// <summary>
    /// Recursive helper for reading nested data fields using reflection.
    /// </summary>
    private void ReadDataByPath(List<string> paths, object data, out object dataOut)
    {
        if (data == null)
        {
            Debug.LogError("[ReadDataByPath] Data object is null.");
            dataOut = null;
            return;
        }

        string p = paths[0];
        Type t = data.GetType();
        FieldInfo field = t.GetField(p, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        if (field == null)
        {
            Debug.LogError($"[ReadDataByPath] Field not found: {p}");
            dataOut = null;
            return;
        }

        if (paths.Count == 1)
        {
            dataOut = field.GetValue(data);
        }
        else
        {
            paths.RemoveAt(0);
            ReadDataByPath(paths, field.GetValue(data), out dataOut);
        }
    }

    /// <summary>
    /// Updates a field value inside userData and saves the file.
    /// Example path: "userInfo/name"
    /// </summary>
    public void UpdateData(string path, object dataNew, Action callback = null)
    {
        if (userData == null)
        {
            Debug.LogError("[UpdateData] userData is null!");
            return;
        }

        string[] s = path.Split('/');
        List<string> paths = new List<string>(s);
        UpdateDataByPath(paths, userData, dataNew, callback);
        SaveData();
    }

    /// <summary>
    /// Recursive helper for updating nested fields via reflection.
    /// </summary>
    private void UpdateDataByPath(List<string> paths, object data, object dataNew, Action callback = null)
    {
        if (data == null)
        {
            Debug.LogError("[UpdateDataByPath] Data object is null.");
            return;
        }

        string p = paths[0];
        Type t = data.GetType();
        FieldInfo field = t.GetField(p, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        if (field == null)
        {
            Debug.LogError($"[UpdateDataByPath] Field not found: {p}");
            return;
        }

        if (paths.Count == 1)
        {
            field.SetValue(data, dataNew);
            callback?.Invoke();
        }
        else
        {
            object nestedData = field.GetValue(data);
            paths.RemoveAt(0);
            UpdateDataByPath(paths, nestedData, dataNew, callback);
        }
    }

    /// <summary>
    /// Updates a dictionary entry by key inside userData.
    /// Example path: "userInventory/dicUnit" + key "K_1"
    /// </summary>
    public void UpdateDataKey<T>(string path, T dataNew, string key, Action callback = null)
    {
        if (userData == null)
        {
            Debug.LogError("[UpdateDataKey] userData is null!");
            return;
        }

        string[] s = path.Split('/');
        List<string> paths = new List<string>(s);
        UpdateDataByPathKey(paths, userData, dataNew, key, callback);
        SaveData();
    }

    /// <summary>
    /// Recursive helper for updating dictionary values.
    /// </summary>
    private void UpdateDataByPathKey<T>(List<string> paths, object data, T dataNew, string key, Action callback = null)
    {
        if (data == null)
        {
            Debug.LogError("[UpdateDataByPathKey] Data object is null.");
            return;
        }

        string p = paths[0];
        Type t = data.GetType();
        FieldInfo field = t.GetField(p, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        if (field == null)
        {
            Debug.LogError($"[UpdateDataByPathKey] Field not found: {p}");
            return;
        }

        if (paths.Count == 1)
        {
            object dic = field.GetValue(data);
            if (dic is SerializableDictionary<string, T> unitDictionary)
            {
                unitDictionary[key] = dataNew;
                field.SetValue(data, unitDictionary);
                callback?.Invoke();
            }
            else
            {
                Debug.LogError($"[UpdateDataByPathKey] Field '{p}' is not a SerializableDictionary.");
            }
        }
        else
        {
            object nestedData = field.GetValue(data);
            paths.RemoveAt(0);
            UpdateDataByPathKey(paths, nestedData, dataNew, key, callback);
        }
    }
    #endregion
}
