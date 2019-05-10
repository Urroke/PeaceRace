using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Terrain
{
    public class Hexagon : MonoBehaviour
    {
        [Serializable]
        public struct HexagonProperties
        {
            public Placement placement;
            public float fertility;
            public ELandType landType;
            public List<Unit> standers;
            public float temperature;
            public float moveCost;
            public Dictionary<HexagonSides, GameObject> sides;

        }

        public HexagonProperties properties;

        public Placement Placement
        {
            get => properties.placement;
            set => properties.placement = value;
        }

        public float Fertility
        {
            get => properties.fertility;
            set => properties.fertility = value;
        }

        public ELandType LandType
        {
            get => properties.landType;
            set => properties.landType = value;
        }

        public List<Unit> Standers
        {
            get => properties.standers;
            set => properties.standers = value;
        }

        public float Temperature
        {
            get => properties.temperature;
            set => properties.temperature = value;
        }


        // Use this for initialization
        void Start()
        {
            properties = new HexagonProperties
            {
                placement = null,
                standers = new List<Unit>(),
                sides = new Dictionary<HexagonSides, GameObject>()
            };
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

