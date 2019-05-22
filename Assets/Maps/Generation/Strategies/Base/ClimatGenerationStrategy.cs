namespace Assets.Maps.Generation.Strategies.Base
{
    public abstract class ClimatGenerationStrategy
    {
        public abstract Map Generate(Map map, MapGenerator.ClimatGenerationOptions options);
    }
}