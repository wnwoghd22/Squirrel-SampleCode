using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

/// <summary>
/// 모든 데이터를 가지고 있을 클래스
/// 데이터가 추가되면 멤버변수만 추가해주면 됨
/// </summary>
public class ObjData
{
    private int type;
    private float posX;
    private float posY;
    private float treeHeight;
    private float lBranchPosY;
    private float lBranchLength;
    private float rBranchPosY;
    private float rBranchLength;
    private int score;
    private int damage;
    private int lDrop;
    private int rDrop;
    private int isNaked;
    public int Type { get { return type; } set { type = value; } }
    public float PosX { get { return posX; } set { posX = value; } }
    public float PosY { get { return posY; } set { posY = value; } }
    public float TreeHeight { get { return treeHeight; } set { treeHeight = value; } }
    public float LBranchPosY { get { return lBranchPosY; } set { lBranchPosY = value; } }
    public float LBranchLength { get { return lBranchLength; } set { lBranchLength = value; } }
    public float RBranchPosY { get { return rBranchPosY; } set { rBranchPosY = value; } }
    public float RBranchLength { get { return rBranchLength; } set { rBranchLength = value; } }
    public int Score { get { return score; } set { score = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int LDrop { get { return lDrop; } set { lDrop = value; } }
    public int RDrop { get { return rDrop; } set { rDrop = value; } }
    public int IsNaked { get { return isNaked; } set { isNaked = value; } }

    /// <summary>
    /// 파싱된 데이터를 가지고 ObjData의 멤버변수에 하나하나씩 자동으로 
    /// 값을 할당할 수 있다.
    /// </summary>
    /// <param name="header">header 있는 부분</param>
    /// <param name="dataType">자료형 있는 부분</param>
    /// <param name="row">실제 데이터 있는 부분</param>
    public void setDataType(string[] header, string[] dataType, string[] row)
    {
        /// ObjData의 멤버변수의 순서와 csv파일의 header의 순서가 똑같아야 index를 증가하는 식으로 다음과 같이 구현할 수 있음
        /// 하지만 맞춰주지 않는 이상 순서가 같지 않은 경우가 태반.... 그때는 반복문을 하나 더 써서 할 수 있음..
        /// 지금은 O(N) 순서가 다르면 O(N^2)으로 구현 가능 
        /// 지금은 순서를 맞췄음  
        /// 순서 맞추는 식으로 하니까 불편함 + 오류가 많이 생기네요...
        for (int index = 0; index < header.Length; index++)
        {
            foreach (PropertyInfo p in typeof(ObjData).GetProperties())
            {
                if (p.Name == header[index])
                {
                    int n;
                    float f;
                    bool b;
                    switch (dataType[index])
                    {
                        case "int":
                            if (int.TryParse(row[index], out n)) p.SetValue(this, n);
                            break;
                        case "float":
                            if (float.TryParse(row[index], out f)) p.SetValue(this, f);
                            break;
                        case "bool":
                            if (bool.TryParse(row[index], out b)) p.SetValue(this, b);
                            break;
                        default:
                            p.SetValue(this, row[index]);
                            break;
                    }
                }
            }
        }
        
    }
}
