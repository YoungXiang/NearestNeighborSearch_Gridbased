using System;
using System.Collections.Generic;
using UnityEngine;

namespace YnPhyx
{
	public interface IGroupable 
	{
        // This is a off-line process
		void MakeGroup (ClothModel clothModel);
        void DrawGizmo ();     
	}

    public abstract class BaseGroupable : IGroupable, ISerializable
    {
        public Groups groups;
        public Dictionary<int, int> groupIdToIndexMap;

        /// <summary>
        /// Search every group[] and return the groupId if the data is in that group.
        /// Else return -1;
        /// </summary>
        public int SearchGroupedData(int data)
        {
            for (int i = 0; i < groups.groups.Length; i++)
            {
                for (int d = 0; d < groups.groups[i].GroupedData.Length; d++)
                {
                    if (data == groups.groups[i].GroupedData[d])
                    {
                        return groups.groups[i].GroupId;
                    }
                }
            }

            return -1;
        }

        public abstract void MakeGroup(ClothModel clothModel);
        public abstract void DrawGizmo();
        public void Deserialize(string path)
        {
            // todo: deserialize groups from path
        }
        public void Serialize(string path)
        {
            // todo: serialize groups to path
        }
    }
}
