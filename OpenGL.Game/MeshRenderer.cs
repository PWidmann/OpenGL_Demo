using System;
using System.Collections.Generic;
using System.Linq;
using OpenGL;
using OpenGL.Mathematics;
using OpenGL.Game;
using System.Text;
using System.Threading.Tasks;

namespace OpenGL.Game
{
    public class MeshRenderer
    {
        public Material material;
        public VAO Geometry;

        public MeshRenderer(Material _material, VAO _vao)
        {
            material = _material;
            Geometry = _vao;
        }

        public GameObject Parent
        {
            get;
            internal set;
        }

        public virtual void Render()
        {
            Geometry.Program.Use();

            Parent.Commit();
            
            Geometry.Draw();
        }
    }
}
