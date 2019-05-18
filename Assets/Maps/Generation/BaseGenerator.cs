using System;
using Assets.Maps.Generation.Strategies;
using UnityEngine;

namespace Assets.Maps.Generation
{
    public class MapGenerator : ScriptableObject
    {
        [Serializable]
        public struct TerrainGenerationOptions
        {
            public int x;
            public int y;
            public float distributionC;
            public float distributionFriction;
            public float coverageArea;
            public int periodJump;
            public int rangeJump;
            public int errorJump;
        }
        [SerializeField]
        private TerrainGenerationOptions options;


        public TerrainGenerationStrategy TerrainStrategy;
        public TerrainGenerationStrategy ReliefStrategy;
        public TerrainGenerationStrategy ResourceStrategy;


        public MapGenerator()
        {
            TerrainStrategy = new StandardTerrainGenerationStrategy();
        }

        public MapGenerator(TerrainGenerationStrategy terrainStrategy, TerrainGenerationStrategy reliefStrategy, TerrainGenerationStrategy resourceStrategy)
        {
            TerrainStrategy = terrainStrategy;
            ReliefStrategy = reliefStrategy;
            ResourceStrategy = resourceStrategy;
        }

        public Map Generate(int width, int height)
        {
            var map = new Map(width, height);
            return null;
        }
    

    }
}