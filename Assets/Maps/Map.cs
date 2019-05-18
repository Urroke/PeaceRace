using System;
using Assets.Terrain;

namespace Assets.Maps
{
    [Serializable]
    public class Map
    {
        public int Width { get; }
        public int Height { get; }

        public Hexagon[,] terrain;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            terrain = new Hexagon[width,height];
        }

    }
}
