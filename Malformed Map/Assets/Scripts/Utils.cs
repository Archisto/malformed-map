using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalformedMap
{
    public static class Utils
    {
        public static Vector3 SideToVector3(BigCube.Side side)
        {
            switch (side)
            {
                case BigCube.Side.Front:
                {
                    return Vector3.forward;
                }
                case BigCube.Side.Back:
                {
                    return Vector3.back;
                }
                case BigCube.Side.Left:
                {
                    return Vector3.left * -1;
                }
                case BigCube.Side.Right:
                {
                    return Vector3.right * -1;
                }
                case BigCube.Side.Top:
                {
                    return Vector3.up * -1;
                }
                case BigCube.Side.Bottom:
                {
                    return Vector3.down * -1;
                }
                default:
                {
                    return Vector3.zero;
                }
            }
        }

        public static BigCube.Side GetNeighborSide(BigCube.Side side, Vector3 direction)
        {
            // TODO: The camera's rotation matters!
            // This is just for the demo.

            int sideNum = (int) side + 1;
            if (sideNum > 5)
            {
                sideNum = 0;
            }

            return (BigCube.Side) sideNum;
        }
    }
}
