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

namespace OpenGL.Game
{
    public class GameObject : Transform
    {
        public string ObjectName;
        public Transform Transform = new Transform();

        public MeshRenderer MeshRenderer;

        public Vector3 color;

        public GameObject(string _name, MeshRenderer _meshRenderer)
        {
            ObjectName = _name;
            MeshRenderer = _meshRenderer;
            MeshRenderer.Parent = this;
        }

        public void Update()
        {
            if (ObjectName == "blue")
            {
                Transform.Rotation += new Vector3(0, 1, 0) * Time.DeltaTime * 20;
            }

            if (ObjectName == "green")
            {
                Transform.Rotation += new Vector3(0, -1, 0) * Time.DeltaTime * 20;
            }

            if (ObjectName == "yellow")
            {
                Transform.Rotation += new Vector3(1, 0, 0) * Time.DeltaTime * 20;
            }

            if (ObjectName == "red")
            {
                Transform.Rotation += new Vector3(-1, 0, 0) * Time.DeltaTime * 20;
            }
        }

        internal void Commit()
        {
            SetTransform();
        }

        public void Render()
        {
            MeshRenderer.Render();
        }

        private void SetTransform()
        {
            Matrix4 view = Game.GetViewMatrix();
            Matrix4 projection = Game.GetProjectionMatrix();

            Material material = this.MeshRenderer.material;

            material["projection"].SetValue(projection);
            material["view"].SetValue(view);
            material["model"].SetValue(this.Transform.GetTRS());
            material["color"].SetValue(this.color);
        }
    }
}
