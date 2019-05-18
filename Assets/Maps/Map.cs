using System;
using Assets.Terrain;

namespace Assets.Maps
{
    [Serializable]
    public class Map
    {
        public int Width { get; }
        public int Height { get; }

        public Hexagon[,] Terrain;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Terrain = new Hexagon[width,height];
        }

    }
}
