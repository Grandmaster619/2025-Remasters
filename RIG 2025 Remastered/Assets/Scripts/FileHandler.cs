using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class FileHandler : MonoBehaviour
{
    public static void SaveToJSON<T> (List<T> toSave, string filename)
    {
        Debug.Log (GetPath(filename));
        string content = JsonHelper.ToJson<T>(toSave.ToArray());
        WriteFile(GetPath(filename), content);
    }

    public static T ReadFromJSON<T> (string filename)
    {
        string content = ReadFile(GetPath(filename));
            
        if(string.IsNullOrEmpty(content) || content == "{}")
        {
            return default (T);
        }

        T res = JsonUtility.FromJson<T>(content);

        return res;
    }

    private static string GetPath(string filename)
    {
        return Application.persistentDataPath + "/" + filename;
    }

    private static void WriteFile(string path, string content)
    {
        FileStream filestream = new FileStream(path, FileMode.Create);

        using (StreamWriter Writer = new StreamWriter(filestream))
        {
            Writer.Write(content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using(StreamReader Reader = new StreamReader(path))
            {
                string content = Reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T> (string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }
    
    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
