using System.IO;
using Assets.Maps.Generation;
using UnityEngine;

namespace Assets.Maps
{
    public static class FileSystemSaverLoader 
    {
        public static void save(Map map,string savePath)
        {
            var json = JsonUtility.ToJson(map);
            using (var writer = new StreamWriter(savePath))
            {
                writer.Write(json);
            }
        }

        public static Map load(string loadPath)
        {
            using (var writer = new StreamReader(loadPath))
            {
                return JsonUtility.FromJson<Map>(writer.ReadToEnd());
            }
        }

        public static void saveToFile(object options, string path)
        {
            var json = JsonUtility.ToJson(options);
            using (var writer = new StreamWriter(path))
            {   
                writer.Write(json);
            }
        }

    }
}
