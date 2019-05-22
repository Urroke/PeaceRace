using System;
using Assets.Maps.Generation.Strategies;
using Assets.Terrain;
using Assets.Terrain.Enums;
using UnityEngine;

namespace Assets.Maps.Generation
{
    public class MapGenerator : ScriptableObject
    {
        
        
        [Serializable]
        public struct TerrainGenerationOptions
        {
            public MapType type;
            public int riverCount;
            public float smoothness;
        }

        [SerializeField]
        private TerrainGenerationOptions terrainOptions;

        [Serializable]
        public struct ReliefGenerationOptions
        {
        }

        [Serializable]
        public struct ClimatGenerationOptions
        {
        }

        [SerializeField]
        private ReliefGenerationOptions reliefOptions;

        [SerializeField]
        private ClimatGenerationOptions climatfOptions;


        public struct ResourceGenerationOptions
        {

        }

        [SerializeField] private ResourceGenerationOptions resourceOptions;



        public TerrainGenerationStrategy TerrainStrategy;
        public ReliefGenerationStrategy ReliefStrategy;
        public ResourceGenerationStrategy ResourceStrategy;
        public ClimatGenerationStrategy ClimatStrategy;


        public MapGenerator()
        {
            TerrainStrategy = new StandardTerrainGenerationStrategy();
            ReliefStrategy = new StandardReliefGenerationStrategy();
            ResourceStrategy = new StandardResourceGenerationStrategy();
            ClimatStrategy = new StandardClimatGenerationStrategy();
        }

        public MapGenerator(TerrainGenerationStrategy terrainStrategy, ReliefGenerationStrategy reliefStrategy,
            ResourceGenerationStrategy resourceStrategy, ClimatGenerationStrategy climatStrategy)
        {
            TerrainStrategy = terrainStrategy;
            ReliefStrategy = reliefStrategy;
            ResourceStrategy = resourceStrategy;
            ClimatStrategy = climatStrategy;
        }

        public Map Generate(int width, int height)
        {
            var map = new Map(width, height);
            map = TerrainStrategy.Generate(map, terrainOptions);
            map = ReliefStrategy.Generate(map, reliefOptions);
            map = ClimatStrategy.Generate(map, climatfOptions);
            //map = ResourceStrategy.Generate(map, resourceOptions);
            return map;
        }
    

    }
}