
﻿using Assets.Maps.Generation;

namespace Assets.Maps
{
    public abstract class ResourceGenerationStrategy
    {
        public abstract Map Generate(Map map, MapGenerator.ResourceGenerationOptions options);
    }
}
