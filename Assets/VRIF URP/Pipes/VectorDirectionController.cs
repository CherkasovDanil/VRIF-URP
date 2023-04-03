using System;
using UnityEngine;
using VRIF_URP.Player;

namespace VRIF_URP.Pipes
{
    public class VectorDirectionController
    {
        private Vector3 _vector3 = Vector3.zero;

        public Vector3 GetVectorFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    _vector3 = Vector3.left;
                    break;
                case Direction.Right:
                    _vector3 = Vector3.right;
                    break;
                case Direction.Up:
                    _vector3 = Vector3.up;
                    break;
                case Direction.Down:
                    _vector3 = Vector3.down;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _vector3;
        }

        public float GetAngleFromDirection(Direction direction)
        {
            _vector3 = GetVectorFromDirection(direction);
            
            float n = Mathf.Atan2(_vector3.y, _vector3.x) * Mathf.Rad2Deg;
            if (n < 0)
                n += 360;
            return n;
        }

        public float GetAngle(Direction direction, Direction targetDirection)
        {
            float angle = direction 
                switch
                {
                    Direction.Up => targetDirection switch
                    {
                        Direction.Left => 0 + 90, 
                        Direction.Right => 0 - 90,
                        Direction.Down => 0 + 180,
                        _ => 0
                    },
                    Direction.Left => targetDirection switch
                    {
                        Direction.Down => 180,
                        Direction.Up => 0,
                        _ => 180
                    },
                    Direction.Down => targetDirection switch
                    {
                        Direction.Left => 90, 
                        Direction.Right => 270, 
                        _ => 270
                    },
                    Direction.Right => targetDirection switch
                    {
                        Direction.Down => 180, 
                        Direction.Up => 0, 
                        _ => 0
                    },
                };
            return angle;
        }
    }
}