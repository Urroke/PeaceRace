
﻿namespace Assets.Maps.Generation.Strategies.Base
{
    public abstract class ResourceGenerationStrategy
    {
        public abstract Map Generate(Map map, MapGenerator.ResourceGenerationOptions options);
    }
}