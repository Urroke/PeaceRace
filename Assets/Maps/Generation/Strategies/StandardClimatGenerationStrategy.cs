using Assets.Maps.Generation.Strategies.Base;
using Assets.Terrain.Enums;
using UnityEngine;

namespace Assets.Maps.Generation.Strategies
{
    public class StandardClimatGenerationStrategy : ClimatGenerationStrategy
    {
        private Map map;

        private float[] climatBariers = { -20f, -6f, 4f, 17f, 32f, 40f, 45f };

        int TemperatureDistribution(float x, float y, float error, ref float value)
        {
            value += 130 * Mathf.Exp(-0.00008f * y * y) - 90 + 1.7f * Mathf.Sin(x / 20)
                                                             + 1.2f * Mathf.Sin(x / 10) + 0.9f * Mathf.Sin(x / 25) +
                                                             2.2f * Mathf.Sin(x / 15) + Random.Range(-error, error);
            for (int i = 0; i < climatBariers.Length; i++)
                if (value < climatBariers[i])
                    return i;
            return climatBariers.Length - 1;
        }

        void ClimatAreaGenerate(float clarityBoundariesC, float ofset)
        {
            for (int i = 0; i < map.width; i++)
            for (int j = 0; j < map.height; j++)
            {
                float temperature = ofset;
                int index = TemperatureDistribution((i * 2 - map.width) * 100f / map.width, (j * 2 - map.height) * 100f / map.height, clarityBoundariesC, ref temperature);
                map.terrain[i, j].Properties.LandType = (EClimatType)(index);
                map.terrain[i, j].Properties.Temperature = temperature;
            }
        }

        public override Map Generate(Map map, MapGenerator.ClimatGenerationOptions options)
        {
            this.map = map;
            return map;
        }
    }
}