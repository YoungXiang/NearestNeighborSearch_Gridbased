using System;
using System.Collections.Generic;
using UnityEngine;

namespace YnPhyx
{
	/// <summary>
	/// Cloth model.
	/// The communiator between Physics & UnityEngine.
	/// The orignal Mesh that is rendered by unity.
	/// The model may be skinned or not skinned, the Physics doesn't care.
	/// </summary>
	public class ClothModel : MonoBehaviour
	{
		// cloth mesh used for rendering, might come from a skinnedMeshRenderer or meshFilter
		[NonSerialized] public Mesh mesh;
		[NonSerialized] public SkinnedMeshRenderer skinnedRenderer;
		[NonSerialized] public MeshFilter meshFilter;

        //[HideInInspector] public Vector3[] clothVertices;

        [NonSerialized]
        public ClothMesh clothMesh;

        [NonSerialized]
        public BaseGroupable vertexGroup;

        #region Functions
        public void InitMesh()
		{
			skinnedRenderer = GetComponent<SkinnedMeshRenderer> ();
			if (skinnedRenderer != null) 
			{
				// avoid editing the original mesh
				mesh = Instantiate(skinnedRenderer.sharedMesh);
				skinnedRenderer.sharedMesh = mesh;
			} 
			else 
			{
				// as for MeshFilter, there is no need to instantiate a new Mesh.
				meshFilter = GetComponent<MeshFilter> ();
				mesh = meshFilter.mesh;
			}

			Debug.Assert (mesh != null, string.Format("Simulation need a <SkinnedMeshRenderer> or <MeshFilter> on [{0}]!", name));

            clothMesh = ClothMesh.CreateFromClothModel(this);

            // temp make group
            vertexGroup = new GridBasedGroup();
            vertexGroup.MakeGroup(this);
        }

		public bool IsSkinned
		{
			get { return skinnedRenderer != null; }		
		}

		public void PushLocalVertices(Vector3[] vertices)
		{
			mesh.vertices = vertices;
		}

        public void PushWorldVertices(Vector3[] worldVertices)
        {
            for (int i = 0; i < worldVertices.Length; i++)
            {
                worldVertices[i] = transform.InverseTransformPoint(worldVertices[i]);
            }

            mesh.vertices = worldVertices;
        }

		public void PullLocalVertices(out Vector3[] vertices)
		{
			vertices = mesh.vertices;
		}

        public void PullWorldVertices(out Vector3[] worldVertices)
        {
            worldVertices = mesh.vertices;
            for (int i = 0; i < worldVertices.Length; i++)
            {
                worldVertices[i] = transform.TransformPoint(worldVertices[i]);
            }
        }

        #region Statics
        public static List<int> SearchGroup(ISearching searchAlgorithm, BaseGroupable searchingIn, int data, int dataGroupId)
        {
            return searchAlgorithm.Search(searchingIn, data, dataGroupId);
        }
        #endregion

        #endregion

        #region EngineCallbacks
        void Awake()
		{
			InitMesh ();
		}

        // think : How to get the skinned vertices back to physics?
        void LateUpdate()
		{
			
		}
    
        void OnDrawGizmosSelected()
        {
            if (vertexGroup != null)
            {
                vertexGroup.DrawGizmo();
            }
        }
        #endregion
    }

}