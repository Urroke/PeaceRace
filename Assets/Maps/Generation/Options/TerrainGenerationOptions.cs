using System;

namespace Assets.Maps.Generation.Options
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
}