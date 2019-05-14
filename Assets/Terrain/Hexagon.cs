using System;
using System.Collections.Generic;
using Assets.Bases;
using Assets.Terrain.Enums;
using UnityEngine;

namespace Assets.Terrain
{
    public class Hexagon : MonoBehaviour
    {
        [Serializable]
        public struct HexagonProperties
        {
            public Placement Placement;
            public float Fertility;
            public ELandType LandType;
            public List<Unit.Unit> Standers;
            public float Temperature;
            public float MoveCost;
            public Dictionary<HexagonSides, GameObject> Sides;

        }

        public HexagonProperties Properties;

        public Placement Placement
        {
            get => Properties.Placement;
            set => Properties.Placement = value;
        }

        public float Fertility
        {
            get => Properties.Fertility;
            set => Properties.Fertility = value;
        }

        public ELandType LandType
        {
            get => Properties.LandType;
            set => Properties.LandType = value;
        }

        public List<Unit.Unit> Standers
        {
            get => Properties.Standers;
            set => Properties.Standers = value;
        }

        public float Temperature
        {
            get => Properties.Temperature;
            set => Properties.Temperature = value;
        }
        public float MoveCost
        {
            get => Properties.MoveCost;
            set => Properties.MoveCost = value;
        }


        // Use this for initialization
        void Start()
        {
            Properties = new HexagonProperties
            {
                Placement = null,
                Standers = new List<Unit.Unit>(),
                Sides = new Dictionary<HexagonSides, GameObject>()
            };

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

