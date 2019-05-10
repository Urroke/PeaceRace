using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class MapGenerator : MonoBehaviour
    {
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

        [SerializeField]
        public GenerationType continent, island;
        private int[] hexNeighborEvenX = {0, 1, 0, -1, -1, -1}; 
        private int[] hexNeighborEvenY = {1, 0, -1, 1, 0, -1}; 
        private int[] hexNeighborOddX = {1, 1, 1, 0, -1, 0}; 
        private int[] hexNeighborOddY = {1, 0, -1, 1, 0, -1}; 
        private const float gexConst = 0.866f;
        public int height;
        public float smothingDepth;
        public int smothingStep;
        public int width;
        public Sprite[] Gexes;
        public GameObject Gex;
        private GameObject[,] terrain;
        private bool[,] isDry;

        void putGex(int x, int y, GameObject Gex)
        {
            Gex.transform.position = new Vector3(gexConst * ((x % width) + 0.5f * (y % 2)), 0.75f * (y % height));
            terrain[x, y] = Gex;
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
        private void EarhtGenerate(GenerationType type, int count = 0)
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
                for(int k = 0;k < 6;k++)
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
                    /*if (i % type.periodJump == 0)
                {
                    x_ += (1 - 2 * Random.Range(-1, 1)) * Random.Range(type.rangeJump - type.errorJump,
                              type.rangeJump + type.errorJump);
                    y_ += (1 - 2 * Random.Range(-1, 1)) * Random.Range(type.rangeJump - type.errorJump,
                              type.rangeJump + type.errorJump);
                    x_ = (width + x_) % width;
                    y_ = (height + y_) % height;
                }*/
                    Debug.Log(x_);
                    Debug.Log(y_);
                    if (notChosen[x_, y_])
                        if (distributionC_ > Random.Range(0.0f, 1.0f))
                        {
                            xStack.Add(x_);
                            yStack.Add(y_);
                            notChosen[x_, y_] = false;
                            distributionC_ *= 1.0f - type.distributionFriction;
                            isDry[x_, y_] = true;
                        }
                }
            }
            for (int i = 0; i < xStack.Count; i++)
                terrain[xStack[i], yStack[i]].GetComponent<SpriteRenderer>().sprite = Gexes[1];   
        }



        void Start()
        {
            int count = 0;
            GenerationType continent_ = continent;
            GenerationType island_ = island;
            terrain = new GameObject[width, height];
            isDry = new bool[width, height];
            for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                putGex(j, i, Instantiate(Gex));
                isDry[j, i] = false;
            }
            EarhtGenerate(continent_);
            Smoothing(3, smothingDepth, smothingStep);
            //EarhtGenerate(island_);   
        }

        void Update()
        {

        }
    }
}
