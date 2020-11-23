using System;
using System.Windows;
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

namespace OpenGL.Game
{
    public class Game
    {
        //Singleton pattern
        public static Game Instance { get { if (instance == null) { instance = new Game(); } return instance; } }
        private static Game instance;

        private Game()
        { 
            //Only accessible from inside this class
        }

        public bool IsInitialized { get; private set; }
        

        // Gameobjects list
        public static List<GameObject> SceneGraph = new List<GameObject>();

        // Player
        public static float movementSpeed = 8f;
        public static float turnSpeed = 3f;

        // Materials
        static Material material1;
        static Material material2;

        

        public void Update()
        {
            // Update all game objects
            for (int i = 0; i < SceneGraph.Count; i++)
            {
                SceneGraph[i].Update();
            }
        }

        public void Render()
        {
            for (int i = 0; i < SceneGraph.Count; i++)
            {
                SceneGraph[i].Render();
            }
        }

        public static GameObject CreateGameObject(string _name, Vector3 _colorRGB, Vector3[] _vertices, uint[] _indices, Vector2[] _uvs, Vector3[] _colorMask, int materialIndex)
        {
            
            if (!Instance.IsInitialized)
            {
                throw new Exception("Game not initialized!");
            }

            //Create VBO and Gameobject
            List<IGenericVBO> vbos = new List<IGenericVBO>();
            vbos.Add(new GenericVBO<Vector3>(new VBO<Vector3>(_vertices), "in_position"));
            vbos.Add(new GenericVBO<Vector3>(new VBO<Vector3>(_colorMask), "in_color"));
            vbos.Add(new GenericVBO<Vector2>(new VBO<Vector2>(_uvs), "in_texcoords"));
            vbos.Add(new GenericVBO<uint>(new VBO<uint>(_indices, BufferTarget.ElementArrayBuffer, BufferUsageHint.DynamicRead)));

            GameObject obj;

            if (materialIndex == 0)
            {
                var vao = new VAO(material1, vbos.ToArray());
                obj = new GameObject(_name, new MeshRenderer(material1, vao));
                obj.color = _colorRGB;
                SceneGraph.Add(obj);
                return obj;
            }
            if (materialIndex == 1)
            {
                var vao = new VAO(material2, vbos.ToArray());
                obj = new GameObject(_name, new MeshRenderer(material2, vao));
                obj.color = _colorRGB;
                SceneGraph.Add(obj);
                return obj;
            }
            else
            {
                return null;
            }
        }

        public static Matrix4 GetProjectionMatrix()
        {
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(Mathf.ToRad(Camera.fov), Camera.screenWidth / Camera.screenHeight, 0.1f, 100f);
            return projection;
        }

        public static Matrix4 GetViewMatrix()
        {
            Matrix4 viewTranslation = Matrix4.Identity;
            Matrix4 viewRotation = Matrix4.Identity;
            Matrix4 viewScale = Matrix4.Identity;

            viewTranslation = Matrix4.CreateTranslation(new Vector3(-Camera.worldPosition.X, -Camera.worldPosition.Y, Camera.worldPosition.Z));
            viewRotation = Matrix4.CreateRotation(new Vector3(0f, 1f, 0f), Camera.cameraYaw); // In radiant
            viewScale = Matrix4.CreateScaling(new Vector3(1.0f, 1.0f, 1.0f));

            Matrix4 view = viewRotation * viewTranslation * viewScale;// RTS matrix -> scale, rotate then translate -> All applied in LOCAL Coordinates

            return view;
        }

        public void Awake()
        {
            material1 = Material.Create("shaders\\colorVert.vs", "shaders\\colorFrag.fs");
            material2 = Material.Create("shaders\\textureVert.vs", "shaders\\textureFrag.fs");

            IsInitialized = true;
        }

        #region PlayerMovement

        public Vector3 MoveForward()
        {
            float newXposition = (float)Math.Sin(Camera.cameraYaw) * movementSpeed;
            float newZposition = (float)Math.Cos(Camera.cameraYaw) * movementSpeed;
            Vector3 direction = new Vector3(-newXposition, 0, newZposition);
            return direction;
        }

        public Vector3 MoveBackward()
        {
            float newXposition = (float)Math.Sin(Camera.cameraYaw) * movementSpeed;
            float newZposition = (float)Math.Cos(Camera.cameraYaw) * movementSpeed;
            Vector3 direction = new Vector3(newXposition, 0, -newZposition);
            return direction;
        }
        
        public Vector3 StrafeLeft()
        {
            float newXposition = (float)Math.Sin(Camera.cameraYaw + Mathf.ToRad(90f)) * movementSpeed;
            float newZposition = (float)Math.Cos(Camera.cameraYaw + Mathf.ToRad(90f)) * movementSpeed;
            Vector3 direction = new Vector3(-newXposition, 0, newZposition);
            return direction;
        }

        public Vector3 StrafeRight()
        {
            float newXposition = (float)Math.Sin(Camera.cameraYaw + Mathf.ToRad(270f)) * movementSpeed;
            float newZposition = (float)Math.Cos(Camera.cameraYaw + Mathf.ToRad(270f)) * movementSpeed;
            Vector3 direction = new Vector3(-newXposition, 0, newZposition);
            return direction;
        }
        
        public void TurnLeft()
        {
            // Turn left
            Camera.cameraYaw += Time.SmoothDeltaTime * turnSpeed; // Camera Y rotation

            // Clamp to 0-360°
            if (Camera.cameraYaw > 2 * (float)Math.PI)
                Camera.cameraYaw = Camera.cameraYaw -= 2 * (float)Math.PI;

            Camera.Forward = MoveForward().Normalize();
        }

        public void TurnRight()
        {
            // Turn right
            Camera.cameraYaw -= Time.SmoothDeltaTime * turnSpeed; // Camera Y rotation

            // Clamp to 0-360°
            if (Camera.cameraYaw < 0)
                Camera.cameraYaw += 2 * (float)Math.PI;

            Camera.Forward = MoveForward().Normalize();
        }

        #endregion

        #region Player Collision Checks

        // Forward Check
        public Vector3 CheckForward(float playerRadius)
        {
            float newXposition = (float)Math.Sin(Camera.cameraYaw) * playerRadius;
            float newZposition = (float)Math.Cos(Camera.cameraYaw) * playerRadius;
            Vector3 direction = new Vector3(-newXposition, 0, newZposition);
            return direction;
        }

        // Backward Check
        public Vector3 CheckBackward(float playerRadius)
        {
            float newXposition = (float)Math.Sin(Camera.cameraYaw) * playerRadius;
            float newZposition = (float)Math.Cos(Camera.cameraYaw) * playerRadius;
            Vector3 direction = new Vector3(newXposition, 0, -newZposition);
            return direction;
        }

        // Left Check
        public Vector3 CheckLeft(float playerRadius)
        {
            float newXposition = (float)Math.Sin(Camera.cameraYaw + Mathf.ToRad(90f)) * playerRadius;
            float newZposition = (float)Math.Cos(Camera.cameraYaw + Mathf.ToRad(90f)) * playerRadius;
            Vector3 direction = new Vector3(-newXposition, 0, newZposition);
            return direction;
        }

        // Right Check
        public Vector3 CheckRight(float playerRadius)
        {
            float newXposition = (float)Math.Sin(Camera.cameraYaw + (3 * Mathf.ToRad(90f))) * playerRadius;
            float newZposition = (float)Math.Cos(Camera.cameraYaw + (3 * Mathf.ToRad(90f))) * playerRadius;
            Vector3 direction = new Vector3(-newXposition, 0, newZposition);
            return direction;
        }

        #endregion
    }
}
