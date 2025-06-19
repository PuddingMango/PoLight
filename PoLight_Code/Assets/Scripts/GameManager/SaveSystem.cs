using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Application.persistentDataPath + "/save.json";

    [System.Serializable]
    private class SaveData
    {
        public List<string> talkedNPCs = new List<string>();
    }

    public static void Save(Dictionary<string, bool> talkedMap)
    {
        SaveData data = new SaveData();
        foreach (var pair in talkedMap)
        {
            if (pair.Value) data.talkedNPCs.Add(pair.Key);
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
    }

    public static Dictionary<string, bool> Load()
    {
        if (!File.Exists(SavePath)) return new Dictionary<string, bool>();

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        Dictionary<string, bool> map = new Dictionary<string, bool>();
        foreach (string id in data.talkedNPCs)
        {
            map[id] = true;
        }

        return map;
    }
}
