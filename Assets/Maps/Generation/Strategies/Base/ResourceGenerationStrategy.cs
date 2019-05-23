
﻿using Assets.Maps.Generation.Options;

 namespace Assets.Maps.Generation.Strategies.Base
{
    public abstract class ResourceGenerationStrategy
    {
        public abstract Map Generate(Map map, ResourceGenerationOptions options);
    }
}