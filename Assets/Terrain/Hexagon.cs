﻿using System;
using System.Collections.Generic;
using Assets.Terrain.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private Sprite[] rivers;
        private static float gexConst = 0.866f;
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

            public bool IsRiver()
            {
                bool res = false;
                for (int i = 0; i < 6; i++)
                    res = res || river[i];
                return res;
            }
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
                gameObject.GetComponent<SpriteRenderer>().sprite = climatZone[(int) Properties.LandType];
            else
            {
                if (Properties.LandType == 0) gameObject.GetComponent<SpriteRenderer>().sprite = whaterZones[0];
                else gameObject.GetComponent<SpriteRenderer>().sprite = whaterZones[1];
            }

            if (Properties.ReliefType != 0)
                Relief.GetComponent<SpriteRenderer>().sprite = reliefs[(int) Properties.ReliefType];


            for (int i = 0; i < 6; i++)
                if (Properties.river[i])
                {
                    Vector3 initPosition = transform.position + new Vector3(0, 0, -1);
                    float direct = ((float)i) * (3.141592f / 3.0f);
                    initPosition += new Vector3(Mathf.Cos(direct), Mathf.Sin(direct), 0) * gexConst / 2;
                    river.GetComponent<SpriteRenderer>().sprite = rivers[Random.Range(0, 5)];
                }
        }

        void Start()
        {
            Properties.river = new bool[6];
        }

        void Update()
        {

        }

    }
}

