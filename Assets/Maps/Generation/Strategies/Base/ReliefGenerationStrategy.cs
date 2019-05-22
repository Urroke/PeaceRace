using Assets.Maps.Generation.Options;

namespace Assets.Maps.Generation.Strategies.Base
{
    public abstract class ReliefGenerationStrategy
    {
        public abstract Map Generate(Map map, ReliefGenerationOptions options);
    }
}