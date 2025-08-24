using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData 
{
    [SerializeField] private UserInfo userInfo;
    [SerializeField] private int currentMission;
    [SerializeField] private List<MissionData> missionDatas;
    [SerializeField] private UserInventory userInventory; 

    // Getter - Setter
    public UserInfo GetUserInfo() => userInfo;
    public void SetUserInfo(UserInfo value) => userInfo = value;

    public int GetCurMission() => currentMission;
    public void SetCurMission(int value) => currentMission = value;

    public List<MissionData> GetMissionDatas() => missionDatas;
    public void SetMissionDatas(List<MissionData> value) => missionDatas = value;

    public UserInventory GetUserInventory() => userInventory;
    public void SetUserInventory(UserInventory value) => userInventory = value;
}

[Serializable] 
public class UserInfo
{
    [SerializeField] private string id;
    [SerializeField] private string name;
    [SerializeField] private int exp;
    [SerializeField] private int level;

    // Getter - Setter
    public string GetUuid() => id;
    public void SetUuid(string value) => id = value;

    public string GetName() => name;
    public void SetName(string value) => name = value;

    public int GetExp() => exp;
    public void SetExp(int value) => exp = value;

    public int GetLevel() => level;
    public void SetLevel(int value) => level = value;
}

[Serializable]
public class UserInventory
{
    [SerializeField] private int gold;
    [SerializeField] private SerializableDictionary<string, UnitData> unitDictionary = new SerializableDictionary<string, UnitData>();

    // Getter - Setter
    public int GetGold() => gold;
    public void SetGold(int value) => gold = value;

    public SerializableDictionary<string, UnitData> GetDicUnit() => unitDictionary;
    public void SetDicUnit(SerializableDictionary<string, UnitData> value) => unitDictionary = value;
}

[Serializable]
public class UnitData
{
    [SerializeField] private int id;
    [SerializeField] private int level;

    // Getter - Setter
    public int GetId() => id;
    public void SetId(int value) => id = value;

    public int GetLevel() => level;
    public void SetLevel(int value) => level = value;
}

[Serializable]
public class MissionData
{
    [SerializeField] private int id;
    [SerializeField] private int star;

    // Getter - Setter
    public int GetId() => id;
    public void SetId(int value) => id = value;

    public int GetStar() => star;
    public void SetStar(int value) => star = value;
}

public static class DataPath // Name Path must be same variable name
{
    public const string INFO = "userInfo"; 
    public const string INVENTORY = "userInventory";
    public const string GOLD = "userInventory/gold";
    public const string DIC_UNIT = "userInventory/unitDictionary";
    public const string USER_NAME = "userInfo/name";
    public const string USER_CURRENTMISSION = "currentMission";
}


