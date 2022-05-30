using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAxisMovingObstacle : ObstacleObj
{
    public override GameObject InstantiatePrefab(GameObject prefab)
    {
        Vector3 pos = new Vector3(posX, posY, 0);

        GameObject result = Object.Instantiate(prefab, pos, Quaternion.identity);

        return result;
    }
}
