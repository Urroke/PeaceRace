using Assets.Maps.Generation;
using UnityEngine;

namespace Assets.Maps
{
    public class Initialize : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;

        private MapGenerator gen;
        [SerializeField]
        private MapRenderer render;

        void Start()
        {
            gen = new MapGenerator();
            render.render(gen.Generate(width, height));
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
