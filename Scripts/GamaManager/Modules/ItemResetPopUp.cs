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
        warningPopUp.transform.GetChild(0).GetComponent<Text>().text = "다음을 기대해주세요!!";
        /*int index = (gm.ChapterNum - 1) * 4 + (gm.StageNum - 1);
        /// 현재 진행중이면 경고창  엔딩아이템 초기화는 이미 클리어한 스테이지만 실행할 수 있음.
        if(gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum - 1] == 1)
        {
            warningPopUp.SetActive(true);
            warningPopUp.transform.GetChild(0).GetComponent<Text>().text = "클리어한 스테이지만\n 할 수 있습니다.";
            return;
        }
        if(gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum - 1] == 2)
        {
            gm.Data.MyItemPerStage[index] = 0;  // 해당 스테이지의 엔딩아이템 개수 초기화
            gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum - 1] = 1;   // 도전 가능한 상태로 만들어줌(이래야 엔딩 아이템이 스테이지에 생김)
        }*/
    }
}