using System;
using Assets.Maps.Generation.Strategies;
using Assets.Maps.Generation.Strategies.Base;
using UnityEngine;

namespace Assets.Maps.Generation
{
    public class MapGenerator : MonoBehaviour
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
        [Serializable]
        public struct ReliefGenerationOptions
        {
            public int field1;
        }
        [Serializable]
        public struct ResourceGenerationOptions
        {
            public int field1;
        }

        [SerializeField]

        private TerrainGenerationOptions terrainOptions;
        [SerializeField]

        private ReliefGenerationOptions reliefOptions;
        [SerializeField]
        private ResourceGenerationOptions resourceOptions;


        public TerrainGenerationStrategy terrainStrategy;
        public ReliefGenerationStrategy reliefStrategy;
        public ResourceGenerationStrategy resourceStrategy;


        public MapGenerator()
        {
            terrainStrategy = new StandardTerrainGenerationStrategy();
            reliefStrategy = new StandardReliefGenerationStrategy();
            resourceStrategy = new StandardResourceGenerationStrategy();
        }

        public MapGenerator(TerrainGenerationStrategy terrainStrategy, ReliefGenerationStrategy reliefStrategy, ResourceGenerationStrategy resourceStrategy)
        {
            this.terrainStrategy = terrainStrategy;
            this.reliefStrategy = reliefStrategy;
            this.resourceStrategy = resourceStrategy;
        }

        public Map Generate(int width, int height)
        {
            var map = new Map(width, height);
            map = terrainStrategy.Generate(map, terrainOptions);
            map = reliefStrategy.Generate(map, reliefOptions);
            map = resourceStrategy.Generate(map, resourceOptions);
            return map;
        }


    }
}