using System;
using System.Collections.Generic;
using Assets.Terrain.Enums;
using UnityEngine;

namespace Assets.Terrain
{
    public class Hexagon : MonoBehaviour
    {
        [SerializeField] private GameObject Relief;
        [SerializeField] private GameObject Flore;
        [SerializeField] private GameObject river;

        [SerializeField] private Sprite[] reliefs;
        [SerializeField] private Sprite[] whaterZones;
        [SerializeField] private Sprite[] climatZone;
        
        [Serializable]
        public struct HexagonProperties
        {
            public bool[] river;
            public Placement Placement;
            public float Fertility;
            public EClimatType LandType;
            public EReliefType ReliefType;
            public List<Unit.Unit> Standers;
            public float Temperature;
            public float MoveCost;
            public Dictionary<HexagonSides, GameObject> Sides;
            public bool isDry;
            public bool IsRiver;
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

        public EClimatType LandType
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

        public void Draw()
        {
            if (Properties.isDry)
                gameObject.GetComponent<SpriteRenderer>().sprite = climatZone[(int)Properties.LandType];
            else
            {
                if (Properties.LandType == 0) gameObject.GetComponent<SpriteRenderer>().sprite = whaterZones[0];
                else gameObject.GetComponent<SpriteRenderer>().sprite = whaterZones[1];
            }

            if (Properties.ReliefType != 0)
                Relief.GetComponent<SpriteRenderer>().sprite = reliefs[(int)Properties.ReliefType];
        }

        void Start()
        {
            Properties.river = new bool[6];
            for(int i = 0;)
        }

        void Update()
        {

        }

    }
}

