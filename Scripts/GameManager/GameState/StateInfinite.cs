using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager
{
    public async void LoadInfiniteScene()
    {
        await PushSceneState(new StateInfinite());
    }

    class StateInfinite : IGameState
    {
        private PlayerController player;
        private ObstacleManager obstacleManager;
        private ItemManager itemManager;

        public void Enter(ISceneHandler gm)
        {

        }

        public void Exit(ISceneHandler gm)
        {
            gm.StageUI.CloseInGameUI();
        }

        public void HandleInput(ISceneHandler gm)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                    activity.Call<bool>("moveTaskToBack", true);
                }
                else
                {
                    Application.Quit();
                }
            }
        }

        public async UniTask Start(ISceneHandler gm)
        {
            gm.StageUI.ShowInGameUI();

            Time.timeScale = 0f;

            if (!SceneManager.GetActiveScene().name.Equals("Stage"))
                await SceneManager.LoadSceneAsync("Stage"); // Scene이 완전히 시작할 때까지 대기

            obstacleManager = FindObjectOfType<ObstacleManager>();
            itemManager = FindObjectOfType<ItemManager>();
            player = FindObjectOfType<PlayerController>();

            obstacleManager.Generate();
            itemManager.Generate();
        }

        public void Update(ISceneHandler gm)
        {
            // 플레이어가 공중에 있을 때만 앞으로 이동
            if (player && !player.AttachedTree)
            {
                obstacleManager?.Scroll();
                itemManager?.Scroll();
            }
        }
    }
}