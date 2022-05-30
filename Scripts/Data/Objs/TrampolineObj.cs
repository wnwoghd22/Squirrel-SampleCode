using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineObj : TerrainObj
{
    public override GameObject InstantiatePrefab(GameObject prefab)
    {
        Vector3 pos = new Vector3(prePosX + posX, posY, 0);
        prePosX += posX;

        GameObject result = Object.Instantiate(prefab, pos, Quaternion.identity);                  // 나무 위치
       
        return result;
    }
}
