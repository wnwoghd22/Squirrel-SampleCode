using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObsObj : ObstacleObj
{
    public override GameObject InstantiatePrefab(GameObject prefab)
    {
        Debug.Log("posX값 : " + posX + " posY값 : " + posY);
        //Vector3 pos = new Vector3(posX, posY, 0);

        GameObject result = Object.Instantiate(prefab, new Vector3(posX, posY, 0), Quaternion.identity);
        //result.transform.position = pos;
        //Debug.Log("위치는 : " + pos + " " + result.transform.position);
        //result.transform.GetChild(0).transform.position = new Vector3(posX + 3, 5, 0);
        //result.transform.GetChild(1).transform.position = new Vector3(posX + 3, 7, 0);

        //result.transform.GetChild(0).gameObject.SetActive(false);

        return result;
    }
}
