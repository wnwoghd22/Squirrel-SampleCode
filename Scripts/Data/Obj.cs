using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// 씬 생성을 위한 클래스
/// 실제 인게임 상에서 사용되는 클래스는 아닙니다.
/// </summary>
public abstract class Obj : IObjInstantiator
{
    protected int type;
    public int Type { get { return type; } set { type = value; } }

    protected float posX;
    public float PosX { get { return posX; } set { posX = value; } }

    protected float posY;
    public float PosY { get { return posY; } set { posY = value; } }

    public abstract GameObject InstantiatePrefab(GameObject prefab);
    
    public void AssignValuesAutomatically(ObjData data)
    {
        // 부모 클래스에서 타입을 확인하더라도 결과는 자식 클래스가 됩니다.
        // 이렇게 되면 메서드 오버라이딩 필요없겠죠?
        // 이렇게 되면 필요한 건 프리팹 생성에 필요한 메소드 하나겠네요.
        Debug.Log("Get Type from Parent class : " + GetType());

        foreach (PropertyInfo p1 in this.GetType().GetProperties())
        {
            foreach (PropertyInfo p2 in typeof(ObjData).GetProperties())
            {
                if (p1.Name == p2.Name)
                {
                    p1.SetValue(this, p2.GetValue(data));
                    break;
                }
            }
        }
    }
}

public abstract class TerrainObj : Obj  // 지형지물
{
    // 지형 지물은 이 변수를 공유하는 게 편합니다. 장애물도 이 변수를 공유한다면, 이건 상위 클래스에 넣는게 더 좋을지도...?
    protected static float prePosX = 0;
    public static float PrePosX => prePosX;
    public static void Initialize()
    {
        prePosX = 0;
    }
}

public abstract class ObstacleObj : Obj // 장애물
{
    protected int damage;
    public int Damage { get { return damage; } set { damage = value; } }
}

public abstract class ItemObj : Obj     // 아이템
{
    protected int score;
    public int Score { get { return score; } set { score = value; } }
}
