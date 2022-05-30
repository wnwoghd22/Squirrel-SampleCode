using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager
{
    class StateLobby : IGameState
    {
        public void Enter(ISceneHandler gm)
        {
            gm.HideoutUI.SetActive(false);
            gm.MainPageUI.SetActive(true);
            gm.SelectChapterUI.CloseSelectChapterTab();
            gm.LobbyUpperBar.gameObject.SetActive(true);
            gm.LobbyMenu.FoldMenu();

            //gm.ShowEndingBook.SetActive(false);

            
            //for(int index = 16; index < gm.Data.AchieveUnLock.Length; index++)
            //{
            //    if(gm.Data.AchieveUnLock[index] == 2)
            //        gm.ShowEndingBook.SetActive(true);
            //}
        }

        public void Exit(ISceneHandler gm)
        {
            // gm.LobbyUI.SetActive(false);
            gm.LoadingScene.SetActive(true);
        }

        public void HandleInput(ISceneHandler gm)
        {

        }

        public async UniTask Start(ISceneHandler gm)
        {
            gm.LobbyUI.SetActive(true);

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