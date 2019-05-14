using Assets.Unit.Strategies;
using Assets.Unit.Strategies.Bases;
using UnityEngine;

namespace Assets.Unit
{
    public class Unit : MonoBehaviour
    {
        public BaseMovementStrategy Strategy;
        public delegate void TurnDelegate();
        public event TurnDelegate OnEachTurn;


        public virtual void Start()
        {
            Strategy = new GroundOnlyMovingStrategy(this);
        }


    }
}
