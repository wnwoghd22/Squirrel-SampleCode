using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemResetPopUp : MonoBehaviour
{
    [SerializeField] private GameObject warningPopUp;
    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void pressYes()
    {
        warningPopUp.SetActive(true);
        warningPopUp.transform.GetChild(0).GetComponent<Text>().text = "������ ������ּ���!!";
        /*int index = (gm.ChapterNum - 1) * 4 + (gm.StageNum - 1);
        /// ���� �������̸� ���â  ���������� �ʱ�ȭ�� �̹� Ŭ������ ���������� ������ �� ����.
        if(gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum - 1] == 1)
        {
            warningPopUp.SetActive(true);
            warningPopUp.transform.GetChild(0).GetComponent<Text>().text = "Ŭ������ ����������\n �� �� �ֽ��ϴ�.";
            return;
        }
        if(gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum - 1] == 2)
        {
            gm.Data.MyItemPerStage[index] = 0;  // �ش� ���������� ���������� ���� �ʱ�ȭ
            gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum - 1] = 1;   // ���� ������ ���·� �������(�̷��� ���� �������� ���������� ����)
        }*/
    }
}