using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class GameManager : IGameFlowHandler
{
    private bool[] shopItemContainer = new bool[5];
    public bool[] ShopItemContainer => shopItemContainer;
    public Inventory Inventory => inventory;
    public Slider ProgressBar => StageUI.ProgressBar;

    public float ScrollSpeed => scrollSpeed;


    public void TestProp()
    {
        StageNum += 1;
        Debug.Log(ChapterNum + "-" + StageNum);
    }

    public void GameOver(int acorns)
    {
        int index = (ChapterNum - 1) * 5 + StageNum;
        StageUI.ShowGameOverUI(acorns, Data.LastChapterNum, Data.ItemPerStage[index]);

        Time.timeScale = 0f;
        gameOver = true;

        // Data.changeAcorn(acorns);

        Acorn += acorns;

        /// 파밍 아이템은 이미 클리어하거나 잠겨있는 스테이지에서는 아이템을 사용하지 않는 이상 엔딩아이템 수집 불가능
        /// 따라서 1인 현재 진행중인 스테이지 일때만 처리
        if (Data.ClearStatus[ChapterNum - 1].intArr[StageNum] == 1)
        {
            Data.ItemPerStage[index] = 0;
        }
        /// 스테이지 초기화권 사용한 상태로 스테이지를 클리어하지 못하면 엔딩아이템 개수 유지
        else if (Data.ClearStatus[ChapterNum - 1].intArr[StageNum] == 3)
        {
            Data.ClearStatus[ChapterNum - 1].intArr[StageNum] = 2;
        }

        //Save();

        if (!Data.RemoveAds)
            adsManager.ShowFrontAd();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        gameOver = false;

        // 재시작을 위해 씬을 벗어납니다.
        PopSceneState();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        stageUI.OpenMenu();
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        stageUI.CloseMenu();
    }

    /// <summary>
    /// 스테이지 클리어시 스테이지에서 먹은 도토리 파밍아이템 저장
    /// </summary>
    public void ClearStage(int acorns)
    {
        int index = (ChapterNum - 1) * 5 + StageNum;
        StageUI.ShowClearUI(acorns, Data.LastChapterNum, Data.ItemPerStage[index], CheckClearStatus(index));

        Acorn += acorns;
        gameOver = true; // 화면을 멈추게 하기 위한 장치

        //Save();

        Time.timeScale = 0f;

        if (!Data.RemoveAds)
            adsManager.ShowFrontAd();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns>다음 챕터가 개방되면 true 반환</returns>
    private bool CheckClearStatus(int index)
    {
        bool result = false;

        /// 도전중 상태(1)일 때 클리어 하면 클리어 상태(2)로 전환하고 엔딩 수집물 개수 적용
        /// 스테이지 초기화권 사용한 상태(3)일 때 클리어 하면 클리어 상태(2)로 전환하고 엔딩 수집물 개수 적용
        if (Data.ClearStatus[ChapterNum - 1].intArr[StageNum] == 1 || Data.ClearStatus[ChapterNum - 1].intArr[StageNum] == 3)
        {
            Data.ClearStatus[ChapterNum - 1].intArr[StageNum] = 2;
            Data.MyItemPerStage[index] = Data.ItemPerStage[index];
            Data.ItemPerStage[index] = 0;
        }

        StageNum += 1;

        // 다음 스테이지가 잠겨있는 상태(0)라면 도전중(1)이라는 스테이지로 변경 
        if (Data.ClearStatus[ChapterNum - 1].intArr[StageNum] == 0)
            Data.ClearStatus[ChapterNum - 1].intArr[StageNum] = 1;

        // 이전 챕터의 3 스테이지를 클리어하면 다음 챕터의 1스테이지가 열림(챕터마다 4스테이지는 도전 난이도... 스테이지 해금과 상관없음)
        // 다만 겨울일 경우 다음 챕터가 없으므로 예외처리를 해주어야 함..(20220424 정호님이 주신 버그)
        if (ChapterNum != 4 && StageNum == 4 && Data.ClearStatus[ChapterNum].intArr[0] == 0)
        {
            result = true;
            Data.ClearStatus[ChapterNum].intArr[0] = 1;
        }

        // 2회차 플레이시 3스테이지를 클리어하면 다음 챕터가 열림
        // 다만 스테이지 0부터가 아니라 1부터 할 수 있음
        if (ChapterNum != 4 && StageNum == 4 && Data.ClearStatus[ChapterNum].intArr[0] == 2 && Data.ClearStatus[ChapterNum].intArr[1] == 0)
        {
            Data.ClearStatus[ChapterNum].intArr[1] = 1;
            result = true;
        }

        return result;
    }

    /// <summary>
    /// 스테이지 클리어 또는 게임오버 화면에서 로비로 나갈 때 호출하는 메소드
    /// </summary>
    public void BackToLobby()
    {
        // 클리어 후 씬을 벗어납니다.
        //PopSceneState();
        PopTwoStates(); // 바로 스테이지 선택 화면으로 돌아가기
    }
}