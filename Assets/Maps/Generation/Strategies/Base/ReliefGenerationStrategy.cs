namespace Assets.Maps.Generation.Strategies
{
    public abstract class ReliefGenerationStrategy
    {
        public abstract Map Generate(Map map, MapGenerator.ReliefGenerationOptions options);
    }
}