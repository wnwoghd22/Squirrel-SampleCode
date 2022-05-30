using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class FarmingItemObj : ItemObj
{
    public override GameObject InstantiatePrefab(GameObject prefab)
    {
        Vector3 pos = new Vector3(posX, posY, 0);

        GameObject result = Object.Instantiate(prefab, pos, Quaternion.identity);
        
        result.GetComponent<SceneObj.FarmingItem>().SetScore(this.score);

        return result;
    }
}
