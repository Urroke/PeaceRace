namespace Assets.Maps.Generation.Strategies
{
    public abstract class ResourceGenerationStrategy
    {
        public abstract Map Generate(Map map, MapGenerator.ResourceGenerationOptions options);
    }
}