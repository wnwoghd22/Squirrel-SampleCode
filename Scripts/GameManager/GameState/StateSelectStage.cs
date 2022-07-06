using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class GameManager
{
    public async void LoadSelectStageScene(int chapter)
    {
        PlaySE(SE_CLICK);

        /// 이전 챕터의 4 스테이지가 잠김이라면 못들어감
        if (chapter >= 2 && Data.ClearStatus[chapter - 2].intArr[4] == 0) {
            WarningPopUp.gameObject.SetActive(true);
            WarningPopUp.setText("이전 챕터 3스테이지를\n클리어 해주세요");
            return; 
        }

        this.ChapterNum = chapter;

        await PushSceneState(new StateSelectStage());
    }

    public class StateSelectStage : IGameState
    {
        public void Enter(ISceneHandler gm)
        {
            // UI 초기화
            gm.LobbyUpperBar.gameObject.SetActive(false);  // 스테이지선택화면에서 다시 안보이게 하기로 했습니다

            gm.LobbyMenu.FoldMenu();

            gm.SelectChapterUI.OpenChapter(gm.ChapterNum);

            gm.SelectStageUI.ShowSelectStageTab();

            /// 해당 챕터에 게임 설치하고 처음 들어갔다면 튜토리얼을 보라는 image 활성화
            if(gm.Data.IsFirstChapter[gm.ChapterNum - 1] == true)
            {
                gm.TutorialGuide.SetActive(true);
                gm.Data.IsFirstChapter[gm.ChapterNum - 1] = false;
                //FileManager.WriteSaveFile(gm.Data);
            }
        }

        public void Exit(ISceneHandler gm)
        {
            gm.SelectStageUI.CloseStageTab();
            
            gm.LoadingScene.SetActive(true);
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
