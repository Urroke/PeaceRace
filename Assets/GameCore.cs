using UnityEngine;
using System.Collections;
using Assets.Maps.Generation;
public class GameCore : MonoBehaviour
{
    [SerializeField] public MapGenerator Generator;

    void Start()
    {
        Generator= ScriptableObject.CreateInstance<MapGenerator>();
    }
}
