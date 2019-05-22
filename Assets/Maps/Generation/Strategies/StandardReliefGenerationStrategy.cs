using System.Collections.Generic;
using Assets.Maps.Generation.Options;
using Assets.Maps.Generation.Strategies.Base;
using Assets.Terrain.Enums;
using UnityEngine;

namespace Assets.Maps.Generation.Strategies
{
    public class StandardReliefGenerationStrategy : ReliefGenerationStrategy
    {

        private Map map;

        private int[] hexNeighborEvenX = { 0, 1, 0, -1, -1, -1 };
        private int[] hexNeighborEvenY = { 1, 0, -1, -1, 0, 1 };
        private int[] hexNeighborOddX = { 1, 1, 1, 0, -1, 0 };
        private int[] hexNeighborOddY = { 1, 0, -1, -1, 0, 1 };

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

        private void distribute(int x, int y, float mountainHeight, float mountainWidthC)
        {
            if (map.terrain[x, y].Properties.isDry && map.terrain[x, y].Properties.ReliefType == 0)
                map.terrain[x, y].Properties.ReliefType = (EReliefType)((int)mountainHeight);

            mountainHeight -= Random.Range(0.0f, 4.0f) / Mathf.Pow(mountainWidthC, 1.5f);
            if (mountainHeight < 0) return;
            if (Random.Range(0.0f, 1.0f) < Mathf.Pow(0.5f, 1 / mountainWidthC)) return;
            for (int k = 0; k < 6; k++)
            {
                int _x = 0, _y = 0;
                GetHexNeighbor(k, x, y, ref _x, ref _y);
                _x = (map.width + _x) % map.width;
                _y = (map.height + _y) % map.height;
                if (map.terrain[_x, _y].Properties.isDry && map.terrain[_x, _y].Properties.ReliefType == 0)
                    distribute(_x, _y, mountainHeight, mountainWidthC);
            }
        }

        void MountainGeneration(int x, int y, float sharpnessC, float mountainWidthC, int length, float sinuosityC)
        {


            float direct = Random.Range(0, 6);
            List<int> xStack = new List<int>();
            List<int> yStack = new List<int>();
            xStack.Add(x);
            yStack.Add(y);
            for (int i = 0; i < length; i++)
            {
                if (map.terrain[x, y].Properties.isDry && map.terrain[x, y].Properties.ReliefType == 0)
                    map.terrain[x, y].Properties.ReliefType = (EReliefType)((int)sharpnessC);

                direct = Mathf.Repeat(6.0f + (Random.Range(0.0f, 6.0f) - 3.0f) * sinuosityC + direct, 6.0f);
                int x_ = 0, y_ = 0;
                GetHexNeighbor((int)Mathf.Floor(direct), x, y, ref x_, ref y_);
                x_ = (map.width + x_) % map.width;
                y_ = (map.height + y_) % map.height;
                if (map.terrain[x_, y_].Properties.isDry && map.terrain[x_, y_].Properties.ReliefType == 0)
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

        public override Map Generate(Map map, ReliefGenerationOptions options)
        {
            this.map = map;
            return map;
        }
    }
}