using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inspector 상에서는 다차원 배열을 띄울 수 없어서 정의한 wrapper 클래스
/// </summary>
[System.Serializable]
public class PrefabList 
{
    public GameObject[] list;
}
