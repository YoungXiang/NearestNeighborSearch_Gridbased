using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YnPhyx
{
    public interface ISerializable
    {
        void Deserialize(string path);
        void Serialize(string path);
    }
}