using System;
using System.Collections.Generic;
using UnityEngine;

namespace YnPhyx
{
    /// <summary>
    /// Cloth mesh.
    /// The bridge between ClothModel and Physics(Particles, Constraints).
    /// 1. Receive the original mesh from ClothModel, and Process it. // e.g. calculate the welded vertices
    /// 2. Responsible for generating Constraints & Particles.
    /// 3. Receive the simulation result from the Physics and Push the results back to ClothModel.
    /// </summary>
    //[Serializable]
    public class ClothMesh 
	{
		/// <summary>
		/// Original vertices come from ClothModel.mesh.vertices
		/// </summary>
		public Vector3[] vertices;
        public int vertexCount;

		public int[] uniqueVerticesIds;
		public int[] origToUniqueVertMapping;
		public int uniqueVerticesCount;


        #region Functions
        public static ClothMesh CreateFromClothModel(ClothModel model)
        {
            ClothMesh cm = new ClothMesh();

            model.PullWorldVertices(out cm.vertices);
            cm.vertexCount = model.mesh.vertexCount;

            // generate unique verticesIds
            cm.GenerateUniqueVertices();

            return cm;
        }
        #endregion

        void GenerateUniqueVertices()
        {
            origToUniqueVertMapping = new int[vertexCount];
            uniqueVerticesCount = 0;

            // key is not a reference type
            Dictionary<Vector3, int> uniqueVertexMap = new Dictionary<Vector3, int>();
            for (int i = 0; i < vertexCount; i++)
            {
                if (!uniqueVertexMap.ContainsKey(vertices[i]))
                {
                    // this vertex is unique
                    uniqueVertexMap.Add(vertices[i], i);
                    origToUniqueVertMapping[i] = i;
                    uniqueVerticesCount++;
                }
                else
                {
                    int actualID = uniqueVertexMap[vertices[i]];
                    origToUniqueVertMapping[i] = actualID;
                }
            }

            uniqueVerticesIds = new int[uniqueVerticesCount];
            uniqueVertexMap.Values.CopyTo(uniqueVerticesIds, 0);
        }


    }

    // Todo : 
    // struct Particle {} // basic simulating unit
    // struct Constraint {} // basic Constraint.
    // class Particles {} 
    //     container for a list of particles of a model. talk to CPU or GPU.
    // class Constraints {} 
    //     container for constraints. talk to CPU or GPU.
}
