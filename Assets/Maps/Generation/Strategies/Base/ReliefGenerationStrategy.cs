namespace Assets.Maps.Generation.Strategies.Base
{
    public abstract class ReliefGenerationStrategy
    {
        public abstract Map Generate(Map map, MapGenerator.ReliefGenerationOptions options);
    }
}