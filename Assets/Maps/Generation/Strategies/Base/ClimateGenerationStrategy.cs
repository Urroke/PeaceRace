using Assets.Maps.Generation.Options;

namespace Assets.Maps.Generation.Strategies.Base
{
    public abstract class ClimateGenerationStrategy
    {
        public abstract Map Generate(Map map, ClimateGenerationOptions options);
    }
}