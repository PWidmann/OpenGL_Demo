using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Game
{
    public static class Camera
    {
        public static float screenWidth;
        public static float screenHeight;
        public static Vector3 worldPosition = new Vector3(1, 1.5f, 1);
        public static float fov = 60f;
        public static float cameraYaw = 0f; // Camera Y rotation in radiant
        public static Vector3 Forward = new Vector3(0, 0, 1);
    }
}
