using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager
{
    public async void LoadHideoutScene()
    {
        await PushSceneState(new StateHideout());
    }

    /// <summary>
    /// 원래는 넣을 예정이었으나 폐기된 페이지
    /// </summary>
    public class StateHideout : IGameState
    {
        public void Enter(ISceneHandler gm)
        {
            // UI 초기화
            gm.LobbyUI.SetActive(false);
        }

        public void Exit(ISceneHandler gm)
        {
            // gm.HideoutUI.SetActive(false);

        }

        public void HandleInput(ISceneHandler gm)
        {

        }

        public async UniTask Start(ISceneHandler gm)
        {
            gm.HideoutUI.SetActive(true);

            if (!SceneManager.GetActiveScene().name.Equals("Lobby"))
                await SceneManager.LoadSceneAsync("Lobby");

            return;
        }

        public void Update(ISceneHandler gm)
        {

        }
    }
}