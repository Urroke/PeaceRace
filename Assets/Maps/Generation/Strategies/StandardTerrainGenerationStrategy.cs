using System;
using System.Collections.Generic;
using System.Numerics;
using Assets.Maps.Generation.Options;
using Assets.Maps.Generation.Strategies.Base;
using Assets.Terrain;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;



namespace Assets.Maps.Generation.Strategies
{
    public class StandardTerrainGenerationStrategy : TerrainGenerationStrategy
    {


        private Map map;
        private int[] hexNeighborEvenX = { 0, 1, 0, -1, -1, -1 };
        private int[] hexNeighborEvenY = { 1, 0, -1, -1, 0, 1 };
        private int[] hexNeighborOddX = { 1, 1, 1, 0, -1, 0 };
        private int[] hexNeighborOddY = { 1, 0, -1, -1, 0, 1 };
        private const float gexConst = 0.866f;

        int GetDryNeighbor(int x, int y)
        {
            int res = 0;
            for (int k = 0; k < 6; k++)
            {
                int x_, y_;
                if (y % 2 == 0)
                {
                    x_ = (map.width + x + hexNeighborEvenX[k]) % map.width;
                    y_ = (map.height + y + hexNeighborEvenY[k]) % map.height;
                }
                else
                {
                    x_ = (map.width + x + hexNeighborOddX[k]) % map.width;
                    y_ = (map.height + y + hexNeighborOddY[k]) % map.height;
                }
                if (map.terrain[x_, y_].Properties.isDry) res++;
            }

            return res;
        }

        void GetHexIndex(float x, float y, ref int _x, ref int _y)
        {
            x += 0.5f * gexConst;
            y += 0.5f;
            _y = (int)(y / 0.75f);
            _x = (int)((x - 0.5f * (_y % 2)) / gexConst);
            _x = (_x + map.width) % map.width;
            _y = (_y + map.height) % map.height;
        }

        void MakeRiver(int x, int y, float directnessC, float bendingC)
        {
            float direct, oldDirect;
            float pi = 3.141592f;
            float ofset = pi / 2;
            Vector3 initPosition;
            int _x = 0, _y = 0;
            Vector3 neighborHexPosition;
            bool waterReached = false;
            int bend = 1;
            direct = pi * Random.Range(0, 6) / 3 + pi / 6;
            initPosition = map.terrain[x, y].transform.position + new Vector3(0, 0, -1);
            map.terrain[x, y].GetComponent<SpriteRenderer>().sprite = null;
            initPosition += new Vector3(Mathf.Cos(direct + ofset), Mathf.Sin(direct + ofset), 0) * gexConst / 2;
            oldDirect = direct;
            int collision = 3;
            while (true)
            {
                waterReached = false;
                for (int k = 0; k < 2; k++)
                {
                    neighborHexPosition = initPosition +
                                          new Vector3(Mathf.Cos(direct + (pi) * k - pi/2), Mathf.Sin(direct + (pi) * k - pi/2), 0) * (gexConst + (1 - Mathf.Abs(k)) * (1.5f - gexConst)) / 2;
                    GetHexIndex(neighborHexPosition.x, neighborHexPosition.y, ref _x, ref _y);
                    if (map.terrain[_x, _y].Properties.isDry)
                        map.terrain[_x, _y].Properties.river[(int)(3.0f*(direct + (pi) * k - pi / 2 + 2*pi) /(pi))] = true;
                    else
                        waterReached = true;
                }

                if (waterReached) break;
                if (Math.Abs(direct - oldDirect) > float.Epsilon && Random.Range(0.0f, 1.0f) < directnessC)
                {
                    if (Random.Range(0.0f, 1.0f) < bendingC)
                    {
                        oldDirect = direct;
                        direct += bend * pi / 3;
                        bend *= -1;
                    }
                    else
                    {
                        float forSwap = direct;
                        direct = oldDirect;
                        oldDirect = forSwap;
                    }
                }
                else
                {
                    oldDirect = direct;
                    direct += pi * (1 - 2 * Random.Range(0, 2)) / 3;
                }

                initPosition += (new Vector3(Mathf.Cos(direct), Mathf.Sin(direct), 0)
                                + new Vector3(Mathf.Cos(oldDirect), Mathf.Sin(oldDirect), 0)) / 4;
                initPosition.y += map.height * 0.75f;
                initPosition.x += map.width * gexConst;
                initPosition.y = Mathf.Repeat(initPosition.y, map.height * 0.75f);
                initPosition.x = Mathf.Repeat(initPosition.x, map.width * gexConst);

                int confluence = 0;
                for (int k = -1; k < 2; k++)
                {
                    neighborHexPosition = initPosition +
                                          new Vector3(Mathf.Cos(direct + (pi / 2) * k), Mathf.Sin(direct + (pi / 2) * k), 0) * (gexConst + (1 - Mathf.Abs(k)) * (1.5f - gexConst)) / 2;
                    GetHexIndex(neighborHexPosition.x, neighborHexPosition.y, ref _x, ref _y);
                    if (!map.terrain[_x, _y].Properties.IsRiver())
                        confluence++;
                }

                direct = Mathf.Repeat(direct + 2 * pi, pi * 2);


                if (confluence == 3) collision--;
                else collision = 3;
                if (collision == 0) break;
            }
        }

        void Smoothing(int initSuperiority, float smoothingC, int degree)
        {
            for (int d = 0; d < degree; d++)
            {
                for (int i = 0; i < map.width; i++)
                    for (int j = 0; j < map.height; j++)
                        if (!map.terrain[i, j].Properties.isDry)
                        {
                            int neighbor = GetDryNeighbor(i, j);
                            if (neighbor >= initSuperiority)
                                if (Random.Range(0.0f, 1.0f) < 1.0 - Mathf.Pow(1.0f - smoothingC, neighbor - initSuperiority))
                                    map.terrain[i, j].Properties.isDry = true;
                        }
            }
        }

        private void EarthGenerate(Vector2Int point, float distributionC, float distributionFriction)
        {
            List<int> xStack = new List<int>();
            List<int> yStack = new List<int>();
            bool[,] notChosen = new bool[map.width, map.height];

            for (int i = 0; i < map.width; i++)
                for (int j = 0; j < map.height; j++)
                    notChosen[i, j] = true;

            xStack.Add(point.x);
            yStack.Add(point.y);
            notChosen[point.x, point.y] = false;

            for (int i = 0; i < xStack.Count; i++)
            {
                for (int k = 0; k < 6; k++)
                {
                    int x_, y_;
                    if (yStack[i] % 2 == 0)
                    {
                        x_ = (map.width + xStack[i] + hexNeighborEvenX[k]) % map.width;
                        y_ = (map.height + yStack[i] + hexNeighborEvenY[k]) % map.height;
                    }
                    else
                    {
                        x_ = (map.width + xStack[i] + hexNeighborOddX[k]) % map.width;
                        y_ = (map.height + yStack[i] + hexNeighborOddY[k]) % map.height;
                    }

                    if (!notChosen[x_, y_]) continue;
                    if (!(distributionC > Random.Range(0.0f, 1.0f))) continue;
                    xStack.Add(x_);
                    yStack.Add(y_);
                    notChosen[x_, y_] = false;
                    distributionC *= 1.0f - distributionFriction;
                }
            }

            for (int i = 0; i < xStack.Count; i++)
                map.terrain[xStack[i], yStack[i]].Properties.isDry = true;
        }

        private Map AddContinent(int risePoints, Vector2Int point, float procent)
        {
            int linearSize = (map.height + map.width) / 2;
            int error = (int)((float)linearSize * procent);
            for (int i = 0; i < risePoints; i++)
            {
                EarthGenerate(point, 0.8f, 0.02f);
                point.x += Random.Range(-error, error);
                point.y += Random.Range(-error, error);
            }
            return map;
        }

        public override Map Generate(Map map, TerrainGenerationOptions options)
        {
            this.map = map;
            if (options.type == 0)
                AddContinent(3, map.GetRandomPoint(), 0.06f);
            return map;
        }
    }
}
