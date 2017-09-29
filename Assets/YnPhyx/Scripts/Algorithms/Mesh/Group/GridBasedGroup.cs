using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YnPhyx
{
    public class GridBasedGroup : BaseGroupable
    {
        public static Vector3 GridCenter = Vector3.zero;
        public static Vector3 GridSize = new Vector3(0.1f, 0.1f, 0.1f);
        public static Vector3 GridNum = new Vector3(50, 30, 30); // new Vector3(24, 18, 18);

        public static int GridLongestAxis = (int)GridNum[LongestAxis(GridNum)];
        public static int TotalGrids = (int)(GridNum.x * GridNum.y * GridNum.z); 

        /// <summary>
        /// Grid origin is not the center, but the left corner;
        /// </summary>
        public static Vector3 GridOrigin = Vector3.zero;
        // static initialize
        static GridBasedGroup()
        {
            int centerGridX = (int)GridNum.x / 2;
            int centerGridY = (int)GridNum.y / 2;
            int centerGridZ = (int)GridNum.z / 2;
            GridOrigin = GridCenter - new Vector3(centerGridX * GridSize.x, centerGridY * GridSize.y, centerGridZ * GridSize.z);
            if (((int)GridNum.x & 1) == 1)
            {
                GridOrigin.x -= GridSize.x * .5f;
            }
            if (((int)GridNum.y & 1) == 1)
            {
                GridOrigin.y -= GridSize.y * .5f;
            }
            if (((int)GridNum.z & 1) == 1)
            {
                GridOrigin.z -= GridSize.z * .5f;
            }
        }

        public override void MakeGroup(ClothModel clothModel)
        {
            groups = new Groups();

            groupIdToIndexMap = new Dictionary<int, int>();

            Vector3[] verticesRef = clothModel.clothMesh.vertices;
            for (int i = 0; i < clothModel.clothMesh.vertexCount; i++)
            {
                Vector3 centerOffset = verticesRef[i] - GridOrigin;
                int x = (int)(centerOffset.x / GridSize.x);
                int y = (int)(centerOffset.y / GridSize.y);
                int z = (int)(centerOffset.z / GridSize.z);
                int hashID = (int)(x + y * GridNum.x + z * (GridNum.x * GridNum.y));
                if (hashID > TotalGrids)
                {
                    Debug.LogFormat("Vertex {0} is out of bounds of the Grid world.", i);
                    continue;
                }
                //Debug.LogFormat("GridId={0}, xyz = ({1}, {2}, {3}).", hashID, x, y, z);

                if (!groupIdToIndexMap.ContainsKey(hashID))
                {
                    Group group = new Group();
                    group.GroupId = hashID;

                    // add vertex index
                    ArrayUtility.Add(ref group.GroupedData, i);

                    ArrayUtility.Add(ref groups.groups, group);
                    groupIdToIndexMap.Add(hashID, groups.groups.Length - 1);
                }
                else
                {
                    // Get Group
                    int groupIndex = groupIdToIndexMap[hashID];
                    Group group = groups.groups[groupIndex];

                    // add vertex index
                    ArrayUtility.Add(ref group.GroupedData, i);
                }
            }
        }

        public override void DrawGizmo()
        {
            Gizmos.color = Color.white;

            Vector3 gridCornerBase = GridOrigin + GridSize * 0.5f;

            if (TotalGrids < 200)
            {
                for (int z = 0; z < GridNum.z; z++)
                {
                    for (int y = 0; y < GridNum.y; y++)
                    {
                        for (int x = 0; x < GridNum.x; x++)
                        {
                            Vector3 center = gridCornerBase + new Vector3(x * GridSize.x, y * GridSize.y, z * GridSize.z);
                            Gizmos.DrawWireCube(center, GridSize);
                            /*
                            int gridGroupId = (int)(x + y * GridNum.x + z * (GridNum.x * GridNum.y));
                            UnityEditor.Handles.Label(center, " " + gridGroupId);
                            */
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("Grids amount is over 200, drawing that much Gizmos will certainly slow you down.");
            }
        }
        
        public static void CalculateGroupXYZ(int groupId, out int x, out int y, out int z)
        {
            z = groupId / (int)(GridNum.x * GridNum.y);
            int yx = groupId % (int)(GridNum.x * GridNum.y);
            y = yx / (int)GridNum.x;
            x = yx % (int)GridNum.x;

            //Debug.LogFormat("[Inverse]GridId={0}, xyz = ({1}, {2}, {3}).", groupId, x, y, z);
        }

        public static int LongestAxis(Vector3 a)
        {
            int axis = Mathf.Abs(a.x) > Mathf.Abs(a.y) ? 0 : 1;
            return Mathf.Abs(a[axis]) > Mathf.Abs(a.z) ? axis : 2;
        }
    }
}
