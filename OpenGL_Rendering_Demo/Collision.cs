using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using OpenGL.Mathematics;
using OpenGL.Game;
using OpenGL.UI;
using System.Diagnostics;
using OpenGL.Platform;
using static OpenGL.GenericVAO;

namespace OpenGL_Rendering_Demo
{
    public static class Collision
    {
        public static List<Bounds> floorBounds = new List<Bounds>();

        public static float playerRadius = 0.3f;
        
        public static void AddBound(Vector3 floorTransformPosition)
        {
            Bounds bound = new Bounds();

            bound.minX = floorTransformPosition.X;
            bound.minZ = floorTransformPosition.Z;

            floorBounds.Add(bound);
        }

        public static bool IsTargetPosOnFloor(Vector3 targetpos)
        {
            bool isOnFloor = false;

            foreach (Bounds bound in floorBounds)
            {
                // if targetpos is inside a floor bound
                if (targetpos.X >= bound.minX && targetpos.X <= bound.maxX && 
                    targetpos.Z >= bound.minZ && targetpos.Z <= bound.maxZ)
                {
                    isOnFloor = true;
                    break;
                }
            }

            return isOnFloor;
        }
    }
}
