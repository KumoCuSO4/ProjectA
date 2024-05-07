using UnityEditor.UI;
using UnityEngine;

namespace Controller.Placeable
{
    public class BasePlaceable : BaseController
    {
        protected int length = 1, width = 1;
        public enum Direction
        {
            EAST = 0,
            SOUTH,
            WEST,
            NORTH,
        }
        protected Direction direction = Direction.EAST;
        
        public BasePlaceable(GameObject gameObject) : base(gameObject)
        {
        }

        public void RotateClockwise()
        {
            direction = (Direction)(((int)direction + 1) % 4);
        }
        
        public void RotateAnticlockwise()
        {
            direction = (Direction)(((int)direction - 1 + 4) % 4);
        }
    }
}