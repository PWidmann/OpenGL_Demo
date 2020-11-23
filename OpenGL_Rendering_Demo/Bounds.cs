using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL_Rendering_Demo
{
    public class Bounds
    {
        // Bounds used for floor tiles -> player collision
        public float minX { get; set; }
        public float minZ { get; set; }
        public float maxX { get { return minX + width; } }
        public float maxZ { get { return minZ + height; } }

        private float width = 3f;
        private float height = 3f;
    }
}
