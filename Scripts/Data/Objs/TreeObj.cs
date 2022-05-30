using UnityEngine;
using System.Reflection;

public class TreeObj : TerrainObj
{
    private float treeHeight;
    public float TreeHeight { get { return treeHeight; } set { treeHeight = value; } }
    private float lBranchPosY;
    public float LBranchPosY { get { return lBranchPosY; } set { lBranchPosY = value; } }
    private float lBranchLength;
    public float LBranchLength { get { return lBranchLength; } set { lBranchLength = value; } }
    private float rBranchPosY;
    public float RBranchPosY { get { return rBranchPosY; } set { rBranchPosY = value; } }
    private float rBranchLength;
    public float RBranchLength { get { return rBranchLength; } set { rBranchLength = value; } }
    private int lDrop;
    public int LDrop { get { return lDrop; } set { lDrop = value; } }
    private int rDrop;
    public int RDrop { get { return rDrop; } set { rDrop = value; } }

    private int isNaked;
    public int IsNaked { get { return isNaked; } set { isNaked = value; } }

    public override GameObject InstantiatePrefab(GameObject prefab)
    {
        Vector3 pos = new Vector3(prePosX + posX, 0, 0);
        prePosX += posX;
        
        //Debug.Log("���� ��" + prePosX + "�������� �Ÿ�" + posX);

        GameObject result = Object.Instantiate(prefab, pos, Quaternion.identity);       // ���� ��ġ

        SceneObj.Tree tree = result.transform.GetChild(0).GetChild(0).GetComponent<SceneObj.Tree>();

        tree.Pillar.transform.localPosition = new Vector3(0, posY, 0);

        tree.SetHeight(treeHeight);
        tree.SetLeftBranch(lBranchPosY, lBranchLength);
        tree.SetRightBranch(rBranchPosY, rBranchLength);

        Debug.Log("LDrop : " + LDrop + ", RDrop : " + RDrop);
        tree.LDrop.SetActive(lDrop == 1);
        tree.RDrop.SetActive(rDrop == 1);
        tree.Leaves.SetActive(isNaked == 0);

        return result;
    }
}
