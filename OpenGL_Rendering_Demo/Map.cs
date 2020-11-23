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
    public class Map
    {
        public int[,] mapArray =
        {
            { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1},
            { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
            { 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1},
            { 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1},
            { 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0},
            { 0, 1, 1, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1},
            { 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1}
        };

        // Object Data
        static Vector3[] verticesCube =
            {
                // Z-position has to be changed to align with sketch

                //Front face
                new Vector3(-0.5f, -0.5f, 0.5f),   // Index 0 on Sketch
                new Vector3(0.5f, -0.5f, 0.5f),  // Index 1 on Sketch
                new Vector3(0.5f, 0.5f, 0.5f), // Index 2 on Sketch
                new Vector3(-0.5f, 0.5f, 0.5f),  // Index 3 on Sketch

                //Right face
                new Vector3(0.5f, -0.5f, 0.5f), // Index 1 on Sketch
                new Vector3(0.5f, -0.5f, -0.5f),  // Index 5 on Sketch
                new Vector3(0.5f, 0.5f, -0.5f),   // Index 6 on Sketch
                new Vector3(0.5f, 0.5f, 0.5f),   // Index 2 on Sketch
                
                //Left face
                new Vector3(-0.5f, -0.5f, -0.5f), // Index 4 on Sketch
                new Vector3(-0.5f, -0.5f, 0.5f),  // Index 0 on Sketch
                new Vector3(-0.5f, 0.5f, 0.5f),   // Index 3 on Sketch
                new Vector3(-0.5f, 0.5f, -0.5f),   // Index 7 on Sketch
                
                //Bottom face
                new Vector3(-0.5f, -0.5f, -0.5f), // Index 4 on Sketch
                new Vector3(0.5f, -0.5f, -0.5f),  // Index 5 on Sketch
                new Vector3(0.5f, -0.5f, 0.5f),   // Index 1 on Sketch
                new Vector3(-0.5f, -0.5f, 0.5f),   // Index 0 on Sketch
                
                //Top face
                new Vector3(-0.5f, 0.5f, 0.5f), // Index 3 on Sketch
                new Vector3(0.5f, 0.5f, 0.5f),  // Index 2 on Sketch
                new Vector3(0.5f, 0.5f, -0.5f),   // Index 6 on Sketch
                new Vector3(-0.5f, 0.5f, -0.5f),   // Index 7 on Sketch
                
                //Back face
                new Vector3(0.5f, -0.5f, -0.5f), // Index 5 on Sketch
                new Vector3(-0.5f, -0.5f, -0.5f),  // Index 4 on Sketch
                new Vector3(-0.5f, 0.5f, -0.5f),   // Index 7 on Sketch
                new Vector3(0.5f, 0.5f, -0.5f)   // Index 6 on Sketch
            };

        static Vector3[] verticesPlane =
        {
            //Floor tile
            new Vector3(0, 0, 0),
            new Vector3(3f, 0, 0),
            new Vector3(3f, 0, -3f),
            new Vector3(0, 0, -3f)
        };

        static Vector3[] verticesWall =
        {
            new Vector3(0, 0, 0),
            new Vector3(3f, 0, 0),
            new Vector3(3f, 3f, 0),
            new Vector3(0, 3f, 0)
        };

        static uint[] indicesPlane =
        {
            0, 1, 2, // front face 1
            2, 3, 0 // front face 2
        };

        static uint[] indicesCube =
        {
                0, 1, 2, // front face 1
                2, 3, 0, // front face 2

                4, 5, 6, // right face 1
                6, 7, 4,  // right face 2
                
                8, 9, 10, // left face 1
                10, 11, 8,  // left face 2
                
                12, 13, 14, // bottom face 1
                14, 15, 12,  // bottom face 2
                
                16, 17, 18, // top face 1
                18, 19, 16,  // top face 2
                
                20, 21, 22, // back face 1
                22, 23, 20  // back face 2

            };

        private static Vector2[] uvs =
        {
            //Front face
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0), 

            ////Right face
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            
            ////Left face
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            
            ////Bottom face
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            
            ////Top face
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            
            ////Back face
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        private static Vector2[] uvsPlane =
        {
            //Front face
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        // Color mask
        private static Vector3[] cheap_wall_ambient_occlusion = new Vector3[]
        {
            // 0, 0, 0 = Black
            // 1, 1, 1 = White
            
            // Front face
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 0 in verticesCube
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 1 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 2 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 3 in verticesCube
            
            // Right face
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 0 in verticesCube
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 1 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 2 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 3 in verticesCube
            
            // Left face
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 0 in verticesCube
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 1 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 2 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 3 in verticesCube
            
            // Bottom face
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 0 in verticesCube
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 1 in verticesCube
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 2 in verticesCube
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 3 in verticesCube
            
            // Top face
            new Vector3(1, 1, 1), // Vertex index 0 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 1 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 2 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 3 in verticesCube
            
            // Back face
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 0 in verticesCube
            new Vector3(0.2f, 0.2f, 0.2f), // Vertex index 1 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 2 in verticesCube
            new Vector3(1, 1, 1), // Vertex index 3 in verticesCube
        };
        private static Vector3[] colorMask_white = new Vector3[]
        {
            // 0, 0, 0 = Black
            // 1, 1, 1 = White
            
            // Front face
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1), 

            // Right face
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1), 

            // Left face
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1), 

            // Bottom face
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1), 

            // Top face
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1), 

            // Back face
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1)
        };

        public void GenerateMap()
        {
            // Single objects
            GameObject blueCube = Game.CreateGameObject("blue", new Vector3(0.2f, 0.2f, 1f), verticesCube, indicesCube, uvs, colorMask_white, 1);
            blueCube.Transform.Position = new Vector3(1f, 1f, -2f);
            blueCube.Transform.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            
            GameObject yellowCube = Game.CreateGameObject("yellow", new Vector3(1f, 1f, 0f), verticesCube, indicesCube, uvs, colorMask_white, 1);
            yellowCube.Transform.Position = new Vector3(3f, 1f, -2f);
            yellowCube.Transform.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            
            GameObject redCube = Game.CreateGameObject("red", new Vector3(1, 0, 0), verticesCube, indicesCube, uvs, colorMask_white, 1);
            redCube.Transform.Position = new Vector3(5f, 1f, -2f);
            redCube.Transform.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            
            GameObject greenCube = Game.CreateGameObject("green", new Vector3(0, 0.5f, 0), verticesCube, indicesCube, uvs, colorMask_white, 1);
            greenCube.Transform.Position = new Vector3(7f, 1f, -2f);
            greenCube.Transform.Scale = new Vector3(0.5f, 0.5f, 0.5f);

            

            for (int x = 0; x < mapArray.GetLength(1); x++)
            {
                for (int y = 0; y < mapArray.GetLength(0); y++)
                {
                    if (mapArray[y, x] == 1) // If it's walkable path
                    {
                        //Floor Tile
                        GameObject floor = Game.CreateGameObject("Floor", new Vector3(0.1f, 0.3f, 0.5f), verticesPlane, indicesPlane, uvsPlane, colorMask_white, 0);
                        floor.Transform.Position = new Vector3(x * 3, 0, y * 3);
                        // Add collision bounds of floor tile
                        Collision.AddBound(new Vector3(floor.Transform.Position.X, 0, -floor.Transform.Position.Z));

                        //Ceiling Tile
                        GameObject ceiling = Game.CreateGameObject("Ceiling", new Vector3(0.4f, 0.7f, 0.9f), verticesPlane, indicesPlane, uvsPlane, colorMask_white, 0);
                        ceiling.Transform.Position = new Vector3(x * 3, 3f, (y * 3) -3);
                        ceiling.Transform.Rotation = new Vector3(180f, 0, 0);


                        //Left Wall
                        if (x == 0 || x > 0 && mapArray[y, x -1] == 0)
                        {
                            GameObject wall = Game.CreateGameObject("Wall", new Vector3(0f, 1f, 1f), verticesPlane, indicesPlane, uvsPlane, cheap_wall_ambient_occlusion, 0);
                            wall.Transform.Position = new Vector3(x * 3, 0, y * 3);
                            wall.Transform.Rotation = new Vector3(270f, 0, 90f);
                        }
            
                        //Top Wall
                        if (y == 0 || y > 0 && mapArray[y -1, x] == 0)
                        {
                            GameObject wall2 = Game.CreateGameObject("Wall", new Vector3(0f, 1f, 1f), verticesPlane, indicesPlane, uvsPlane, cheap_wall_ambient_occlusion, 0);
                            wall2.Transform.Position = new Vector3(x * 3, 0f, y * 3 - 3);
                            wall2.Transform.Rotation = new Vector3(-90f, 0, 0);
                        }
            
                        //Right Wall
                        if (x < mapArray.GetLength(1) - 1 && mapArray[y, x + 1] == 0 || x == mapArray.GetLength(1) - 1)
                        {
                            GameObject wall3 = Game.CreateGameObject("Wall", new Vector3(0f, 1f, 1f), verticesPlane, indicesPlane, uvsPlane, cheap_wall_ambient_occlusion, 0);
                            wall3.Transform.Position = new Vector3(x * 3 + 3, 0f, y * 3 - 3);
                            wall3.Transform.Rotation = new Vector3(-90f, 0, -90f);
                        }
            
                        //Bottom Wall
                        if (y == mapArray.GetLength(0) - 1 || (y < mapArray.GetLength(0) - 1) && mapArray[y + 1, x] == 0)
                        {
                            GameObject wall4 = Game.CreateGameObject("Wall", new Vector3(0f, 1f, 1f), verticesPlane, indicesPlane, uvsPlane, cheap_wall_ambient_occlusion, 0);
                            wall4.Transform.Position = new Vector3(x * 3 + 3, 0, y * 3);
                            wall4.Transform.Rotation = new Vector3(90f, 180f, 0);
                        }
                    }
                }
            }
        }
    }
}
