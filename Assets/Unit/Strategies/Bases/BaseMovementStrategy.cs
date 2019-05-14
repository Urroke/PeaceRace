using Assets.Terrain;

namespace Assets.Unit.Strategies.Bases
{
    public abstract class BaseMovementStrategy
    {
        public float BasicSpeed;
        public Unit unit;

        public abstract void MoveTo(Hexagon destination);




    }
}
