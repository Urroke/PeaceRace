using Assets.Maps.Generation.Strategies;
using Assets.Maps.Generation.Strategies.Base;
using JetBrains.Annotations;
using UnityEngine;
using static Assets.Common.Settings;

namespace Assets.Maps.Generation
{
    public class MapGenerator : MonoBehaviour
    {

        public GeneratorOptions options;
        public TerrainGenerationStrategy TerrainStrategy;
        public ReliefGenerationStrategy ReliefStrategy;
        public ResourceGenerationStrategy ResourceStrategy;
        public ClimateGenerationStrategy ClimateStrategy;

        public MapGenerator()
        {
            TerrainStrategy = new StandardTerrainGenerationStrategy();
            ReliefStrategy = new StandardReliefGenerationStrategy();
            ResourceStrategy = new StandardResourceGenerationStrategy();
            options = GeneratorOptions.LoadFromDisk();
        }

        
        public MapGenerator(TerrainGenerationStrategy terrainStrategy, ReliefGenerationStrategy reliefStrategy, ResourceGenerationStrategy resourceStrategy)
        {
            TerrainStrategy = terrainStrategy;
            ReliefStrategy = reliefStrategy;
            ResourceStrategy = resourceStrategy;
        }

        public Map Generate(int width, int height)
        {
            var map = new Map(width, height);
            map = TerrainStrategy.Generate(map, options.TerrainGenerationOptions);
            return map;
        }


    }
}