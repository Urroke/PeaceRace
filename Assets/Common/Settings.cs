using System;
using System.IO;
using Assets.Maps.Generation.Options;
using UnityEditor;
using UnityEngine;

namespace Assets.Common
{
    public class Settings : ScriptableObject
    {
        public class GeneratorOptions
        {
            private static string DISK_PATH = "./Settings/Generation/GenerationSettings.json";

            public static void SaveToDisk(GeneratorOptions options)
            {
                Directory.CreateDirectory(DISK_PATH.Remove(DISK_PATH.LastIndexOf("/", StringComparison.Ordinal)));
                using (var stream = new StreamWriter(DISK_PATH))
                {
                    stream.WriteLine(JsonUtility.ToJson(options));
                }

            }

            public static GeneratorOptions LoadFromDisk()
            {
                using (var stream = new StreamReader(DISK_PATH))
                {
                   return JsonUtility.FromJson<GeneratorOptions>(stream.ReadToEnd());
                }

            }

            public TerrainGenerationOptions TerrainGenerationOptions;
            public ReliefGenerationOptions ReliefGenerationOptions;
            public ResourceGenerationOptions ResourceGenerationOptions;

            public GeneratorOptions()
            {
                TerrainGenerationOptions = new TerrainGenerationOptions();
                ReliefGenerationOptions = new ReliefGenerationOptions();
                ResourceGenerationOptions= new ResourceGenerationOptions();
                
            }


        }



    }
}