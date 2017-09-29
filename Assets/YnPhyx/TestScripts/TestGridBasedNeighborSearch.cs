using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YnPhyx;

public class TestGridBasedNeighborSearch : MonoBehaviour
{
    [Tooltip("Search vertex from A in B")]
    public ClothModel A;
    public int vertexIndex = -1;
    int cachedIndex = -1;

    [Tooltip("Search vertex from A in B")]
    public ClothModel B;
    int nearestIndex = -1;

	void Update ()
    {
		if (cachedIndex != vertexIndex)
        {
            if (vertexIndex >= 0 && vertexIndex < A.clothMesh.vertexCount)
            {
                Search();
                cachedIndex = vertexIndex;
            }
        }
	}

    void Search()
    {
        int dataGroupId = A.vertexGroup.SearchGroupedData(vertexIndex);        
        //Debug.LogFormat("Vertex {0} is in Group {1}.", vertexIndex, dataGroupId);

        List<int> retList = ClothModel.SearchGroup(new NeighborSearch(), B.vertexGroup, vertexIndex, dataGroupId);
        float minDist = float.MaxValue;
        nearestIndex = -1;
        Vector3 baseVec = A.clothMesh.vertices[vertexIndex];

        // find the nearest neighbor.
        for (int i = 0; i < retList.Count; i++)
        {
            float currentDist = (baseVec - B.clothMesh.vertices[retList[i]]).sqrMagnitude;
            if (currentDist < minDist)
            {
                minDist = currentDist;
                nearestIndex = retList[i];
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            if (vertexIndex >= 0 && vertexIndex < A.clothMesh.vertexCount)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(A.clothMesh.vertices[vertexIndex], Vector3.one * 0.02f);

                int dataGroupId = A.vertexGroup.SearchGroupedData(vertexIndex);
                int x, y, z;
                GridBasedGroup.CalculateGroupXYZ(dataGroupId, out x, out y, out z);
                Vector3 gridCornerBase = GridBasedGroup.GridOrigin + GridBasedGroup.GridSize * 0.5f;
                Vector3 center = gridCornerBase + new Vector3(x * GridBasedGroup.GridSize.x, y * GridBasedGroup.GridSize.y, z * GridBasedGroup.GridSize.z);
                Gizmos.DrawWireCube(center, GridBasedGroup.GridSize);

                if (nearestIndex >= 0)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(B.clothMesh.vertices[nearestIndex], Vector3.one * 0.02f);
                }
            }
    }
}
