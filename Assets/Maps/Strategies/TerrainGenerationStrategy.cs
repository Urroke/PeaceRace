namespace Assets.Maps.Strategies
{
    public abstract class TerrainGenerationStrategy
    {
        private Map _map;

        protected TerrainGenerationStrategy(Map map)
        {
            _map = map;
        }

        public abstract Map Generate();
    }
}