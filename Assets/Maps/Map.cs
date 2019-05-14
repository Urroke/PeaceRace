using System;
using UnityEngine;
using System.Collections;
using Assets.Terrain;

[Serializable]
public class Map
{
    public int width;
    public int height;

    public Hexagon[,] terrain;


}
