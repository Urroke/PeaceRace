
﻿using Assets.Maps.Generation;
using Assets.Maps.Generation.Options;

namespace Assets.Maps
{
    public abstract class ResourceGenerationStrategy
    {
        public abstract Map Generate(Map map, ResourceGenerationOptions options);
    }
}
