using Assets.Terrain;
using UnityEngine;

namespace Assets.Maps
{
    public class MapRenderer : MonoBehaviour
    {

        private Map map;
        [SerializeField] private GameObject Gex;

        private const float gexConst = 0.866f;

        void putGex(int x, int y, GameObject Gex)
        {
            Gex.transform.position = new Vector3(gexConst * ((x % map.width) + 0.5f * (y % 2)), 0.75f * (y % map.height), 0);
        }

        public void render(Map map)
        {
            this.map = map;
            for (int i = 0; i < map.height; i++)
            for (int j = 0; j < map.width; j++)
            {
                GameObject vr = Instantiate(Gex);
                vr.GetComponent<Hexagon>().Properties = map.terrain[j, i].Properties;
                putGex(j, i, vr);
                vr.GetComponent<Hexagon>().Draw();
            }
        }   
    }
}
