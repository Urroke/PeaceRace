using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    [UsedImplicitly]
    public class StandardMapGenerator : MonoBehaviour
    {
        [Serializable]
        public class ArrayGameObjects
        {
            public GameObject[] arrayGameObjects;
        }

        [Serializable]
        public struct GenerationType
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

        [SerializeField] public GenerationType continent, island;
        public GameObject river;
        [SerializeField]
        public ArrayGameObjects[] mountains;
        private int[] hexNeighborEvenX = { 0, 1, 0, -1, -1, -1 };
        private int[] hexNeighborEvenY = { 1, 0, -1, -1, 0, 1 };
        private int[] hexNeighborOddX = { 1, 1, 1, 0, -1, 0 };
        private int[] hexNeighborOddY = { 1, 0, -1, -1, 0, 1 };
        private const float gexConst = 0.866f;
        public int height;
        public float smothingDepth;
        public int smothingStep;
        public float directness;
        public float clarityBoundaries;
        public float bending;
        public float mountainWidth;
        public int width;
        public Sprite[] rivers;
        public Sprite[] Gexes;
        public GameObject Gex;
        private GameObject[,] terrain;
        private GameObject[,] mountTerrain;
        private bool[,] isDry;
        private float[] climatBariers = { -20f, -6f, 4f, 17f, 32f, 40f, 45f };
        void putGex(int x, int y, GameObject Gex)
        {
            Gex.transform.position = new Vector3(gexConst * ((x % width) + 0.5f * (y % 2)), 0.75f * (y % height), 0);
            terrain[x, y] = Gex;
        }
        void putMountain(int x, int y, GameObject mount)
        {
            mount.transform.position = new Vector3(gexConst * ((x % width) + 0.5f * (y % 2)), 0.75f * (y % height), -2);
            mountTerrain[x, y] = mount;
        }
        void GetHexNeighbor(int neighbor, int x, int y, ref int x_, ref int y_)
        {
            if (y % 2 == 0)
            {
                x_ = x + hexNeighborEvenX[neighbor];
                y_ = y + hexNeighborEvenY[neighbor];
            }
            else
            {
                x_ = x + hexNeighborOddX[neighbor];
                y_ = y + hexNeighborOddY[neighbor];
            }
        }

        int TemperatureDistribution(float x, float y, float error)
        {
            float value = 130 * Mathf.Exp(-0.00008f * y * y) - 90 + 1.7f * Mathf.Sin(x / 20)
                                                                  + 1.2f * Mathf.Sin(x / 10) + 0.9f * Mathf.Sin(x / 25) +
                                                                  2.2f * Mathf.Sin(x / 15) + Random.Range(-error, error);
            for (int i = 0; i < climatBariers.Length; i++)
                if (value < climatBariers[i])
                    return i;
            return climatBariers.Length - 1;
        }

        private void distribute(int x, int y, int mountainHeight, float mountainWidthC)
        {
            if (mountainHeight == 0)
                return;
            if (isDry[x, y] && mountTerrain[x, y] == null)
                putMountain(x, y, Instantiate(mountains[mountainHeight - 1].arrayGameObjects[terrain[x, y].GetComponent<HexesProp>().ClimatZone - 1], transform.position, Quaternion.identity));
            mountainHeight = (int)Mathf.Floor(mountainHeight - mountainWidth * Random.Range(0.0f, mountainHeight));
            for (int k = 0; k < 6; k++)
            {
                int _x = 0, _y = 0;
                GetHexNeighbor(k, x, y, ref _x, ref _y);
                _x = (width + _x) % width;
                _y = (height + _y) % height;
                if (isDry[_x, _y] && mountTerrain[_x, _y] == null)
                    distribute(_x, _y, mountainHeight, mountainWidthC);
            }
        }

        void MountainGeneration(int x, int y, int sharpnessC, float mountainWidthC, int length, float sinuosityC)
        {


            float direct = Random.Range(0, 6);
            List<int> xStack = new List<int>();
            List<int> yStack = new List<int>();
            xStack.Add(x);
            yStack.Add(y);
            for (int i = 0; i < length; i++)
            {
                if (isDry[x, y] && mountTerrain[x, y] == null)
                {
                    putMountain(x, y, Instantiate(mountains[sharpnessC - 1].arrayGameObjects[terrain[x, y].GetComponent<HexesProp>().ClimatZone - 1], transform.position, Quaternion.identity));
                }

                direct = Mathf.Repeat(6.0f + (Random.Range(0.0f, 6.0f) - 3.0f) * sinuosityC + direct, 6.0f);
                int x_ = 0, y_ = 0;
                GetHexNeighbor((int)Mathf.Floor(direct), x, y, ref x_, ref y_);
                x_ = (width + x_) % width;
                y_ = (height + y_) % height;
                if (isDry[x_, y_] && mountTerrain[x_, y_] == null)
                {
                    xStack.Add(x_);
                    yStack.Add(y_);
                }
                x = x_;     //Mozhno vinesty i  budet veselo
                y = y_;
            }

            for (int i = 0; i < xStack.Count; i++)
            {
                distribute(xStack[i], yStack[i], sharpnessC, mountainWidthC);
            }
        }
        void ClimatAreaGenerate(float clarityBoundariesC)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    int index = TemperatureDistribution((i * 2 - width) * 100f / width, (j * 2 - height) * 100f / height, clarityBoundariesC);
                    if (isDry[i, j])
                    {
                        if (index == 0) index++;
                        terrain[i, j].GetComponent<SpriteRenderer>().sprite =
                            Gexes[index + 1];
                        terrain[i, j].GetComponent<HexesProp>().ClimatZone = index;
                    }
                    else if (index == 0) terrain[i, j].GetComponent<SpriteRenderer>().sprite = Gexes[1];
                }
        }

        void GetHexIndex(float x, float y, ref int _x, ref int _y)
        {
            x += 0.5f * gexConst;
            y += 0.5f;
            _y = (int)(y / 0.75f);
            _x = (int)((x - 0.5f * (_y % 2)) / gexConst);
            _x = (_x + width) % width;
            _y = (_y + height) % height;
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
            initPosition = terrain[x, y].transform.position + new Vector3(0, 0, -1);
            terrain[x, y].GetComponent<SpriteRenderer>().sprite = Gexes[2];
            initPosition += new Vector3(Mathf.Cos(direct + ofset), Mathf.Sin(direct + ofset), 0) * gexConst / 2;
            oldDirect = direct;
            int collision = 3;
            while (true)
            {
                waterReached = false;
                for (int k = -1; k < 2; k++)
                {
                    neighborHexPosition = initPosition +
                                          new Vector3(Mathf.Cos(direct + (pi / 2) * k), Mathf.Sin(direct + (pi / 2) * k), 0) * (gexConst + (1 - Mathf.Abs(k)) * (1.5f - gexConst)) / 2;
                    GetHexIndex(neighborHexPosition.x, neighborHexPosition.y, ref _x, ref _y);
                    if (isDry[_x, _y])
                        terrain[_x, _y].GetComponent<SpriteRenderer>().sprite = Gexes[2];
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
                initPosition.y += height * 0.75f;
                initPosition.x += width * gexConst;
                initPosition.y = Mathf.Repeat(initPosition.y, height * 0.75f);
                initPosition.x = Mathf.Repeat(initPosition.x, width * gexConst);

                int confluence = 0;
                for (int k = -1; k < 2; k++)
                {
                    neighborHexPosition = initPosition +
                                          new Vector3(Mathf.Cos(direct + (pi / 2) * k), Mathf.Sin(direct + (pi / 2) * k), 0) * (gexConst + (1 - Mathf.Abs(k)) * (1.5f - gexConst)) / 2;
                    GetHexIndex(neighborHexPosition.x, neighborHexPosition.y, ref _x, ref _y);
                    if (terrain[_x, _y].GetComponent<SpriteRenderer>().sprite == Gexes[2])
                        confluence++;
                }

                Instantiate(river, initPosition, Quaternion.Euler(new Vector3(0, 0, 180 * direct / pi)));
                river.GetComponent<SpriteRenderer>().sprite = rivers[Random.Range(0, rivers.Length)];
                direct = Mathf.Repeat(direct + 2 * pi, pi * 2);


                if (confluence == 3) collision--;
                else collision = 3;
                if (collision == 0) break;
            }
        }

        int GetDryNeighbor(int x, int y)
        {
            int res = 0;
            for (int k = 0; k < 6; k++)
            {
                int x_, y_;
                if (y % 2 == 0)
                {
                    x_ = (width + x + hexNeighborEvenX[k]) % width;
                    y_ = (height + y + hexNeighborEvenY[k]) % height;
                }
                else
                {
                    x_ = (width + x + hexNeighborOddX[k]) % width;
                    y_ = (height + y + hexNeighborOddY[k]) % height;
                }
                if (isDry[x_, y_]) res++;
            }

            return res;
        }

        void Smoothing(int initSuperiority, float smoothingC, int degree)
        {
            for (int d = 0; d < degree; d++)
            {
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                        if (!isDry[i, j])
                        {
                            int neighbor = GetDryNeighbor(i, j);
                            if (neighbor >= initSuperiority)
                                if (Random.Range(0.0f, 1.0f) < 1.0 - Mathf.Pow(1.0f - smoothingC, neighbor - initSuperiority))
                                {
                                    isDry[i, j] = true;
                                    terrain[i, j].GetComponent<SpriteRenderer>().sprite = Gexes[1];
                                }
                        }
            }
        }
        private void EarthGenerate(GenerationType type)
        {
            float distributionC_ = type.distributionC;
            List<int> xStack = new List<int>();
            List<int> yStack = new List<int>();
            bool[,] notChosen = new bool[width, height];

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    notChosen[i, j] = true;

            xStack.Add(type.x);
            yStack.Add(type.y);
            notChosen[type.x, type.y] = false;

            for (int i = 0; i < xStack.Count; i++)
            {
                for (int k = 0; k < 6; k++)
                {
                    int x_, y_;
                    if (yStack[i] % 2 == 0)
                    {
                        x_ = (width + xStack[i] + hexNeighborEvenX[k]) % width;
                        y_ = (height + yStack[i] + hexNeighborEvenY[k]) % height;
                    }
                    else
                    {
                        x_ = (width + xStack[i] + hexNeighborOddX[k]) % width;
                        y_ = (height + yStack[i] + hexNeighborOddY[k]) % height;
                    }

                    if (!notChosen[x_, y_]) continue;
                    if (distributionC_ > Random.Range(0.0f, 1.0f))
                    {
                        xStack.Add(x_);
                        yStack.Add(y_);
                        notChosen[x_, y_] = false;
                        distributionC_ *= 1.0f - type.distributionFriction;
                    }
                }
            }

            for (int i = 0; i < xStack.Count; i++)
            {
                terrain[xStack[i], yStack[i]].GetComponent<SpriteRenderer>().sprite = Gexes[1];
                isDry[xStack[i], yStack[i]] = true;
            }
        }



        void Start()
        {
            
            
            mountTerrain = new GameObject[width, height];
            int count = 0;
            GenerationType continent_ = continent;
            GenerationType island_ = island;
            GenerationType smallIsland = island_;
            terrain = new GameObject[width, height];
            isDry = new bool[width, height];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    putGex(j, i, Instantiate(Gex));
                    isDry[j, i] = false;
                }
            EarthGenerate(continent_);
            for (int k = 0; k < 8; k++)
            {
                island_.x = Random.Range(width / 3, width);
                island_.y = Random.Range(height / 9, 8 * height / 9);
                EarthGenerate(island_);
            }

            smallIsland.distributionC = 0.4f;
            smallIsland.distributionFriction = 0.1f;

            for (int k = 0; k < 40; k++)
            {
                island_.x = Random.Range(width / 4, width);
                island_.y = Random.Range(height / 9, 8 * height / 9);
                EarthGenerate(island_);
            }

            Smoothing(3, smothingDepth, smothingStep);

            for (int k = 0; k < 150; k++)
            {
                int _x = Random.Range(width / 3, width);
                int _y = Random.Range(0, height);
                if (isDry[_x, _y])
                    MakeRiver(_x, _y, directness, bending);
            }
            ClimatAreaGenerate(clarityBoundaries);
            for (int k = 0; k < 5; k++)
            {
                int _x = Random.Range(0, width);
                int _y = Random.Range(0, height);
                MountainGeneration(_x, _y, Random.Range(3, 5), Random.Range(0.0f, 0.5f), Random.Range(120, 180), Random.Range(0.05f, 0.12f));
            }
            for (int k = 0; k < 200; k++)
            {
                int _x = Random.Range(0, width);
                int _y = Random.Range(0, height);
                MountainGeneration(_x, _y, Random.Range(1, 3), Random.Range(0.0f, 0.7f), Random.Range(1, 10), Random.Range(0.5f, 0.2f));
            }






        }

        void Update()
        {            
        }
    }
}
