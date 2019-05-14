using Assets.Terrain;
using Assets.Unit.Strategies.Bases;
using UnityEngine;

namespace Assets.Unit.Strategies
{
    public class GroundOnlyMovingStrategy : BaseMovementStrategy
    {
        private Rigidbody2D rigidbody;


        public GroundOnlyMovingStrategy(Unit unit)
        {
            this.unit = unit;
            rigidbody = unit.GetComponent<Rigidbody2D>();
            BasicSpeed = 2;
        }


        public override void MoveTo(Hexagon destination)
        {
            Vector3.MoveTowards(unit.transform.position, destination.transform.position, BasicSpeed);
        }
    }
}
