namespace Assets.Maps.Generation.Strategies.Base
{
    public abstract class TerrainGenerationStrategy
    {
        public abstract Map Generate(Map map, MapGenerator.TerrainGenerationOptions options);
    }

  
}