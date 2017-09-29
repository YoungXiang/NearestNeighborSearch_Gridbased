using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YnPhyx
{
    public class NeighborSearch : ISearching
    {
        Dictionary<int, bool> searchedGroup = new Dictionary<int, bool>();
        public List<int> Search(BaseGroupable searchingIn, int data, int dataGroupId)
        {
            searchedGroup.Clear();

            List<int> retList = new List<int>();
            // first, search directly in that group
            if (searchingIn.groupIdToIndexMap.ContainsKey(dataGroupId))
            {
                searchedGroup.Add(dataGroupId, true);
                int groupIndex = searchingIn.groupIdToIndexMap[dataGroupId];
                for (int i = 0; i < searchingIn.groups.groups[groupIndex].GroupedData.Length; i++)
                {
                    retList.Add(searchingIn.groups.groups[groupIndex].GroupedData[i]);
                }
            }
            else
            {
                Debug.LogFormat("SearchingIn doesn't has GroupID = {0}.", dataGroupId);
            }

            int x, y, z;
            GridBasedGroup.CalculateGroupXYZ(dataGroupId, out x, out y, out z);

            int beforeCount = retList.Count;
            int longestGrid = GridBasedGroup.GridLongestAxis / 2;
            for (int level = 1; level <= longestGrid; level++)
            {
                //Debug.LogFormat("Search Level {0}. ", level);
                SearchAdjacentGrids(ref retList, level, searchingIn, x, y, z);
                if (retList.Count > beforeCount) break;
            }

            return retList;
        }

        // search Level : 1 ... n
        void SearchAdjacentGrids(ref List<int> retList, int searchLevel, BaseGroupable searchingIn, int x, int y, int z)
        {
            // search the adjacency grids. // only used for GridBasedGroup
            for (int xi = -searchLevel; xi <= searchLevel; xi++)
            {
                for (int yi = -searchLevel; yi <= searchLevel; yi++)
                {
                    for (int zi = -searchLevel; zi <= searchLevel; zi++)
                    {
                        if (xi == 0 && yi == 0 && zi == 0) continue;
                        int searchingX = x + xi;
                        int searchingY = y + yi;
                        int searchingZ = z + zi;

                        if ((searchingX >= 0 && searchingX < GridBasedGroup.GridNum.x) 
                            &&(searchingY >= 0 && searchingY < GridBasedGroup.GridNum.y)
                            &&(searchingZ >= 0 && searchingZ < GridBasedGroup.GridNum.z))
                        {
                            SearchGrids(ref retList, searchingIn, searchingX, searchingY, searchingZ);                            
                        }
                    }
                }
            }
        }

        void SearchGrids(ref List<int> retList, BaseGroupable searchingIn, int x, int y, int z)
        {
            int dataGroupId = (int)(x + y * GridBasedGroup.GridNum.x + z * (GridBasedGroup.GridNum.x * GridBasedGroup.GridNum.y));
            if (searchedGroup.ContainsKey(dataGroupId)) return;
            if (searchingIn.groupIdToIndexMap.ContainsKey(dataGroupId))
            {
                searchedGroup.Add(dataGroupId, true);
                int groupIndex = searchingIn.groupIdToIndexMap[dataGroupId];
                for (int i = 0; i < searchingIn.groups.groups[groupIndex].GroupedData.Length; i++)
                {
                    retList.Add(searchingIn.groups.groups[groupIndex].GroupedData[i]);
                }
            }
        }

    }
}
