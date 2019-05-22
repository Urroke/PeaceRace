using System;
using System.Numerics;
using Assets.Terrain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Maps
{
    [Serializable]
    public class Map
    {
        public int width { get; }
        public int height { get; }

        public Hexagon[,] terrain;

        public Map(int _width, int _height)
        {
            width = _width;
            height = _height;
            terrain = new Hexagon[width, height];
            for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                terrain[i, j] = new Hexagon();
                terrain[i, j].Properties.isDry = false;
                terrain[i, j].Properties.ReliefType = 0;
            }

        }

        public Vector2Int GetRandomPoint()
        {
            Vector2Int res = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
            return res;
        }
    }
}
