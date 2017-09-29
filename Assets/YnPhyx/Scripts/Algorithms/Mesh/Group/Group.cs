using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Group 
{
	public int GroupId;
	public int[] GroupedData;

    public Group()
    {
        GroupId = -1;
        GroupedData = new int[0];
    }
}

[Serializable]
public class Groups
{
	public Group[] groups;
    public Groups()
    {
        groups = new Group[0];
    }
}
