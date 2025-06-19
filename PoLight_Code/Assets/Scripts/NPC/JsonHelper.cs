using System;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string wrapped = "{\"array\":" + json + "}";
        return JsonUtility.FromJson<Wrapper<T>>(wrapped).array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
