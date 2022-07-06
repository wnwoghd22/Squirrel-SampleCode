using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectChapterUI : MonoBehaviour
{
    [SerializeField] GameObject chapterTab;

    [SerializeField] GameObject chapterButtons;
    [SerializeField] Animator chapterAnimator;

    [SerializeField] Image[] chapter;
    [SerializeField] GameObject[] chapterInfoUI;
    [SerializeField] Text[] itemCountText;

    [SerializeField] GameObject endingButton;

    private IDataHandler dataHandler;

    private int[] seasonItemCount;

    private void Awake()
    {
        dataHandler = FindObjectOfType<GameManager>();
    }

    private void SetUIValues()
    {
        seasonItemCount = new int[4] { 0, 0, 0, 0 };

        for (int i = 0; i < 20; i++)
        {
            if (1 <= i && i < 5) seasonItemCount[0] += dataHandler.Data.MyItemPerStage[i];
            else if (6 <= i && i < 10) seasonItemCount[1] += dataHandler.Data.MyItemPerStage[i];
            else if (11 <= i && i < 15) seasonItemCount[2] += dataHandler.Data.MyItemPerStage[i];
            else if (16 <= i && i < 20) seasonItemCount[3] += dataHandler.Data.MyItemPerStage[i];
        }

        for(int i = 0; i < itemCountText.Length; i++)
        {
            itemCountText[i].text = "X " + seasonItemCount[i] + "개";
        }

        for (int i = 1; i < chapter.Length; i++)
        {
            if (dataHandler.Data.ClearStatus[i - 1].intArr[4] == 0) // 이전 계절 3스테이지 클리어시(= 4스테이지 잠김이 아닐때)
                chapter[i].color = new Color(0.5f, 0.5f, 0.5f);
            else
                chapter[i].color = new Color(1f, 1f, 1f);
        }
    }

    public void CloseSelectChapterTab() => chapterTab.SetActive(false);

    public void InitSelectChapter()
    {
        chapterTab.SetActive(true);

        /// 챕터 선택 창에 챕터 정보 활성화
        foreach (GameObject o in chapterInfoUI) o.SetActive(true);

        /// 챕터 선택 창 값 설정
        SetUIValues();

        /// 겨울 4 스테이지가 열리면 엔딩버튼이 활성화(색깔만 변화)
        endingButton.GetComponent<Image>().color = 
            dataHandler.Data.ClearStatus[3].intArr[4] != 0 ? 
            new Color(1, 1, 1) : new Color(0.5f, 0.5f, 0.5f);

        chapterAnimator.ResetTrigger("close");
        chapterAnimator.ResetTrigger("open1");
        chapterAnimator.ResetTrigger("open2");
        chapterAnimator.ResetTrigger("open3");
        chapterAnimator.ResetTrigger("open4");

        chapterAnimator.SetTrigger("close");

        chapterButtons.SetActive(true);
    }

    public void OpenChapter(int i)
    {
        foreach (GameObject o in chapterInfoUI) o.SetActive(false);

        /** (redmine #5382)
         * 챕터 선택 창 값 설정
         * SelectChapterUI를 활성화하는 두 개의 메소드 중, 이쪽에는 SetUI를 해주지 않았습니다.
         * 여름 탭이 잠긴 채로 애니메이션이 출력되니 세로선이 생긴 것처럼 보일 수 밖에 없었겠죠.
         */
        SetUIValues();

        chapterButtons.SetActive(false);

        chapterAnimator.ResetTrigger("close");
        chapterAnimator.ResetTrigger("open1");
        chapterAnimator.ResetTrigger("open2");
        chapterAnimator.ResetTrigger("open3");
        chapterAnimator.ResetTrigger("open4");

        chapterAnimator.SetTrigger("open" + i);
    }
}
