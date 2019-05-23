using System;
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
        [SerializeField] private GameObject River;
        [SerializeField] private List<GameObject> Rivers;

        [SerializeField] private Sprite[] reliefs;
        [SerializeField] private Sprite[] whaterZones;
        [SerializeField] private Sprite[] climatZone;
        [SerializeField] private Sprite[] rivers;
        private static float gexConst = 0.866f;
        private const float pi = 3.141593f;
        [Serializable]
        public struct HexagonProperties
        {
            public bool[] river;
            public Placement Placement;
            public float Fertility;
            public float riverScale;
            public EClimatType LandType;
            public EReliefType ReliefType;
            public List<Unit.Unit> Standers;
            public float Temperature;
            public float MoveCost;
            public Dictionary<HexagonSides, GameObject> Sides;
            public bool isDry;
            public bool needDrawRiver;

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
                    gameObject.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0.9f);
                    if (Properties.needDrawRiver)
                    {
                        Vector3 initPosition = transform.position + new Vector3(0, 0, -1);
                        float direct = ((float) i * (pi / 3) + pi / 6);
                        //Debug.Log(i);
                        Rivers.Add(Instantiate(River));
                        initPosition += new Vector3(Mathf.Cos(direct - pi / 2), Mathf.Sin(direct - pi / 2), 0) *
                                        gexConst / 2;
                        Rivers[Rivers.Count - 1].GetComponent<SpriteRenderer>().sprite = rivers[Random.Range(0, 4)];
                        Rivers[Rivers.Count - 1].transform.position = initPosition;
                        Rivers[Rivers.Count - 1].transform.localScale = new Vector3(1, Properties.riverScale, 1);
                        Rivers[Rivers.Count - 1].transform.rotation =
                            Quaternion.Euler(new Vector3(0, 0, 180.0f * direct / pi));
                    }
                }
        }

        void Start()
        {
        }

        void Update()
        {

        }

    }
}

