namespace Assets.Maps.Generation.Strategies
{
    public abstract class TerrainGenerationStrategy
    {
        public abstract Map Generate(Map map, MapGenerator.TerrainGenerationOptions options);
    }

  
}