using OpenGL;
using OpenGL.Mathematics;
using OpenGL.Platform;
using OpenGL.Game;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using OpenGL.UI;
using static OpenGL.GenericVAO;

namespace OpenGL_Rendering_Demo
{
    static class Program
    {
        private static int screenWidth = 1366;
        private static int screenHeight = 768;

        static Map map = new Map();

        static void Main()
        {
            InitializeMain();
            InitializeTexture();

            Game.Instance.Awake();

            map.GenerateMap();

            #region Input

            // Key Input
            Event evt = new Event(new OpenGL.Platform.Event.KeyEvent(OnKeyStateChanged));

            Input.Subscribe((char)Key.Left, evt);
            Input.Subscribe((char)Key.W, evt);
            Input.Subscribe((char)Key.A, evt);
            Input.Subscribe((char)Key.S, evt);
            Input.Subscribe((char)Key.D, evt);
            Input.Subscribe((char)Key.Q, evt);
            Input.Subscribe((char)Key.E, evt);

            // Hook to the escape press event using the OpenGL.UI class library
            Input.Subscribe((char)Keys.Escape, Window.OnClose);
            // Make sure to set up mouse event handlers for the window
            Window.OnMouseCallbacks.Add(OpenGL.UI.UserInterface.OnMouseClick);
            Window.OnMouseMoveCallbacks.Add(OpenGL.UI.UserInterface.OnMouseMove);
            
            #endregion

            ///////////////////
            // Game loop
            ///////////////////
            while (Window.Open)
            {
                Window.HandleInput();
                OnPreRenderFrame();

                Game.Instance.Update();
                Game.Instance.Render();

                OnPostRenderFrame();

                PlayerMovement();
            }
        }

        private static void PlayerMovement()
        {
            // Forward Movement
            if (GameInput.W && Collision.IsTargetPosOnFloor(Camera.worldPosition + Game.Instance.CheckForward(Collision.playerRadius)))
                Camera.worldPosition += Game.Instance.MoveForward() * Time.SmoothDeltaTime;
            // Backward Movement
            if (GameInput.S && Collision.IsTargetPosOnFloor(Camera.worldPosition + Game.Instance.CheckBackward(Collision.playerRadius)))
                Camera.worldPosition += Game.Instance.MoveBackward() * Time.SmoothDeltaTime;
            // Strafe Left
            if (GameInput.A && Collision.IsTargetPosOnFloor(Camera.worldPosition + Game.Instance.CheckLeft(Collision.playerRadius)))
                Camera.worldPosition += Game.Instance.StrafeLeft() * Time.SmoothDeltaTime;
            // Strafe Right
            if (GameInput.D && Collision.IsTargetPosOnFloor(Camera.worldPosition + Game.Instance.CheckRight(Collision.playerRadius)))
                Camera.worldPosition += Game.Instance.StrafeRight() * Time.SmoothDeltaTime;
            // Turn Left
            if (GameInput.Q)
                Game.Instance.TurnLeft();
            // Turn Right
            if (GameInput.E)
                Game.Instance.TurnRight();
        }

        private static void InitializeMain()
        {
            Time.Initialize();
            Camera.screenWidth = screenWidth;
            Camera.screenHeight = screenHeight;
            Window.CreateWindow("OpenGL Rendering Demo", screenWidth, screenHeight);

            // add a reshape callback to update the UI
            Window.OnReshapeCallbacks.Add(OnResize);

            // add a close callback to make sure we dispose of everythng properly
            Window.OnCloseCallbacks.Add(OnClose);

            // Enable depth testing to ensure correct z-ordering of our fragments
            Gl.Enable(EnableCap.DepthTest);
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Gl.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
        }

        private static void InitializeTexture()
        {
            var crateTexture = new Texture("textures/crate1.png");
            Gl.ActiveTexture(0);
            Gl.BindTexture(crateTexture);

            // Scale up (magnify)
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
            TextureParameter.Nearest); // Linear
            // Scale down (minify)
            Gl.TexParameteri(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
            TextureParameter.Nearest); // Linear

            Gl.Enable(EnableCap.CullFace);
            Gl.CullFace(CullFaceMode.Back);
        }

        #region Keyboard Input

        private static void OnKeyStateChanged(char c, bool pressed)
        {
            //game.OnKeyStateChanged(c, pressed);

            Key key = (Key)c;
            
            if (key == Key.W)
            {
                if (pressed)
                    GameInput.W = true;
                else
                    GameInput.W = false;
            }
            else if (key == Key.S)
            {
                if (pressed)
                    GameInput.S = true;
                else
                    GameInput.S = false;
            }
            else if (key == Key.A)
            {
                if (pressed)
                    GameInput.A = true;
                else
                    GameInput.A = false;
            }

            else if (key == Key.D)
            {
                if (pressed)
                    GameInput.D = true;
                else
                    GameInput.D = false;
            }
            else if (key == Key.Q)
            {
                if (pressed)
                    GameInput.Q = true;
                else
                    GameInput.Q = false;
            }
            else if (key == Key.E)
            {
                if (pressed)
                    GameInput.E = true;
                else
                    GameInput.E = false;
            }
            else if (key == Key.Left)
            {
                if (pressed)
                {
                    System.Console.WriteLine("--------- Camera and player ---------");
                    System.Console.WriteLine("Player Position: " + Camera.worldPosition);
                    System.Console.WriteLine("Camera forward vector: " + Camera.Forward);
                }
                    
            }
        }

        #endregion

        #region Callbacks

        private static void OnResize()
        {
            screenWidth = Window.Width;
            screenHeight = Window.Height;

            OpenGL.UI.UserInterface.OnResize(Window.Width, Window.Height);
        }

        private static void OnClose()
        {
            // make sure to dispose of everything
            OpenGL.UI.UserInterface.Dispose();
            OpenGL.UI.BMFont.Dispose();
        }

        private static void OnPreRenderFrame()
        {
            // set up the OpenGL viewport and clear both the color and depth bits
            Gl.Viewport(0, 0, Window.Width, Window.Height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        private static void OnPostRenderFrame()
        {
            // Draw the user interface after everything else
            OpenGL.UI.UserInterface.Draw();

            // Swap the back buffer to the front so that the screen displays
            Window.SwapBuffers();

            Time.Update();
        }

        private static void OnMouseClick(int button, int state, int x, int y)
        {
            // take care of mapping the Glut buttons to the UI enums
            if (!OpenGL.UI.UserInterface.OnMouseClick(button + 1, (state == 0 ? 1 : 0), x, y))
            {
                // do other picking code here if necessary
            }
        }

        private static void OnMouseMove(int x, int y)
        {
            if (!OpenGL.UI.UserInterface.OnMouseMove(x, y))
            {
                
                
            }
        }

        #endregion
    }
}
