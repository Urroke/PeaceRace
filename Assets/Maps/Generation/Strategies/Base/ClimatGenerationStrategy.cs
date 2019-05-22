namespace Assets.Maps.Generation.Strategies
{
    public abstract class ClimatGenerationStrategy
    {
        public abstract Map Generate(Map map, MapGenerator.ClimatGenerationOptions options);
    }
}