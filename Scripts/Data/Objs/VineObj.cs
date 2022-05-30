using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineObj : TerrainObj
{
    public override GameObject InstantiatePrefab(GameObject prefab)
    {
        Vector3 pos = new Vector3(prePosX + posX, posY, 0);
        prePosX += posX;

        GameObject result = Object.Instantiate(prefab, pos, Quaternion.identity);

        return result;
    }
}
