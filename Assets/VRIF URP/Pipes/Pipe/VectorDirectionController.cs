using System;
using UnityEngine;
using VRIF_URP.Player;

namespace VRIF_URP.Pipes
{
    public class VectorDirectionController
    {
        private Vector3 _vector3 = Vector3.zero;

        public Vector3 GetVectorFromDirection(PipeDirection pipeDirection)
        {
            switch (pipeDirection)
            {
                case PipeDirection.Left:
                    _vector3 = Vector3.left;
                    break;
                case PipeDirection.Right:
                    _vector3 = Vector3.right;
                    break;
                case PipeDirection.Up:
                    _vector3 = Vector3.up;
                    break;
                case PipeDirection.Down:
                    _vector3 = Vector3.down;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _vector3;
        }

        public float GetAngleFromDirection(PipeDirection pipeDirection)
        {
            _vector3 = GetVectorFromDirection(pipeDirection);
            
            float n = Mathf.Atan2(_vector3.y, _vector3.x) * Mathf.Rad2Deg;
            if (n < 0)
                n += 360;
            return n;
        }

        public float GetAngle(PipeDirection pipeDirection, PipeDirection targetPipeDirection)
        {
            float angle = pipeDirection 
                switch
                {
                    PipeDirection.Up => targetPipeDirection switch
                    {
                        PipeDirection.Left => 0 + 90, 
                        PipeDirection.Right => 0 - 90,
                        PipeDirection.Down => 0 + 180,
                        _ => 0
                    },
                    PipeDirection.Left => targetPipeDirection switch
                    {
                        PipeDirection.Down => 180,
                        PipeDirection.Up => 0,
                        _ => 180
                    },
                    PipeDirection.Down => targetPipeDirection switch
                    {
                        PipeDirection.Left => 90, 
                        PipeDirection.Right => 270, 
                        _ => 270
                    },
                    PipeDirection.Right => targetPipeDirection switch
                    {
                        PipeDirection.Down => 180, 
                        PipeDirection.Up => 0, 
                        _ => 0
                    },
                };
            return angle;
        }
    }
}