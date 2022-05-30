using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class CandyObj : ItemObj
{
    public override GameObject InstantiatePrefab(GameObject prefab)
    {
        Vector3 pos = new Vector3(posX, posY, 0);

        GameObject result = Object.Instantiate(prefab, pos, Quaternion.identity);

        return result;
    }
}
