﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProceduralMaze
{
    [RequireComponent(typeof(MeshFilter))]
    public class MazeMeshGenerator : MonoBehaviour
    {
        List<MazeCell> cells;
        PositionalGraph floorGraph;
        PositionalGraph wallGraph;
        MeshFilter meshFilter;
        
        enum DIRECTION { horizontal, vertical };

        void Start()
        {
            meshFilter = gameObject.GetComponent<MeshFilter>();
        }

        public void UpdateMesh(List<MazeCell> cells, PositionalGraph floorGraph, PositionalGraph wallGraph)
        {       
            this.cells = cells;
            this.floorGraph = floorGraph;
            this.wallGraph = wallGraph;
                    
            Mesh wallMesh = GenerateWallMesh();
            //Generate floor
            //Generate ceiling

            meshFilter.mesh = wallMesh;
        }        

         Mesh GenerateWallMesh()
        {
            List<Cube> cubes = GetWallCubes();
            return new StitchedCubeMesh(cubes).mesh;
        }

        List<Cube> GetWallCubes()
        {
            List<Cube> cubes = new List<Cube>();

            float wallHeight = 2.0F;
            float wallWidth = 0.5F;
            foreach (GraphConnection<PositionalGraphNode> connection in wallGraph.GetConnections())
            {
                DIRECTION wallDirection = GetWallDirection(connection);

                Vector3 position = (connection.nodeA.position + connection.nodeB.position) / 2;
                position.y += wallHeight / 2;
                Vector3 vectorDistance = connection.nodeA.position - connection.nodeB.position;
                float distance = vectorDistance.magnitude;

                if (wallDirection == DIRECTION.horizontal)
                {
                    Cube cube = new Cube(position, distance, wallWidth, wallHeight);
                    cubes.Add(cube);
                }
                else
                {
                    Cube cube = new Cube(position, wallWidth, distance, wallHeight);
                    cubes.Add(cube);
                }   
            }

            return cubes;
        }        

        DIRECTION GetWallDirection(GraphConnection<PositionalGraphNode> connection)
        {
            if (connection.nodeA.position.z == connection.nodeB.position.z)
            {
                return DIRECTION.horizontal;
            }
            else
            {
                return DIRECTION.vertical;
            }
        }
    }
}

