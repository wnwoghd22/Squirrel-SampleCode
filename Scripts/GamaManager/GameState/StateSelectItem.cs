using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class GameManager
{
    public async void LoadSelectItemScene(int chapter)
    {
        PlaySE(SE_CLICK);

        this.StageNum = chapter;

        /// 선택할려는 스테이지가 0(잠금 상태)라면 팝업창 뜨고 스테이지에는 못 들어가게 함
        if (Data.ClearStatus[ChapterNum - 1].intArr[StageNum] == 0)
        {
            WarningPopUp.gameObject.SetActive(true);
            WarningPopUp.setText("이전 스테이지를 클리어 해주세요");
            return;
        }


        Debug.Log("현재 스테이지 : " + ChapterNum + " - " + StageNum);

        await PushSceneState(new StateSelectItem());

    }

    public class StateSelectItem : IGameState
    {

        public void Enter(ISceneHandler gm)
        {
            // UI 초기화
            // gm.SelectChapterUI.SetActive(false);
            // gm.SelectStageUI.SetActive(false);
            gm.SelectItemUI.ShowSelectItemTab();

            gm.SelectChapterUI.OpenChapter(gm.ChapterNum); // 스테이지에서 재도전으로 나갈 때.

            gm.LobbyUpperBar.gameObject.SetActive(false);

            gm.LobbyMenu.FoldMenu();

            // 아이템 개수 초기화
            Debug.Log(gm.Data.MyItemCount[0]);
        }

        public void Exit(ISceneHandler gm)
        {
            gm.LoadingScene.SetActive(true);

            gm.SelectItemUI.CloseSelectItemTab();
        }

        public void HandleInput(ISceneHandler gm)
        {

        }

        public async UniTask Start(ISceneHandler gm)
        {
            if (!SceneManager.GetActiveScene().name.Equals("Lobby"))
                await SceneManager.LoadSceneAsync("Lobby");

            gm.LoadingScene.SetActive(false);

            gm.PlayMain();

            return;
        }

        public void Update(ISceneHandler gm)
        {

        }
    }

}
