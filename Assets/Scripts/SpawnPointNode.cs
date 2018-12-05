using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointNode{

    public bool spawnable;
    public Vector3 worldPosition;

    public SpawnPointNode(bool _Spawnable, Vector3 _worldPos)
    {
        spawnable = _Spawnable;
        worldPosition = _worldPos;
    }

}
