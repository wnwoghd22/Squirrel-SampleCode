using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public partial class GameManager
{
    public async void LoadSelectChapterScene()
    {
        PlaySE(SE_CLICK);

        await PushSceneState(new StateSelectChapter());
    }

    public class StateSelectChapter : IGameState
    {
        public void Enter(ISceneHandler gm)
        {
            // UI 초기화
            gm.LobbyUI.SetActive(false);

            gm.LobbyUpperBar.gameObject.SetActive(false);

            gm.LobbyMenu.FoldMenu();

            gm.SelectChapterUI.InitSelectChapter();
        }

        public void Exit(ISceneHandler gm)
        {
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

            gm.SelectChapterUI.gameObject.SetActive(true);

            return;
        }

        public void Update(ISceneHandler gm)
        {

        }
    }
}
