using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager
{
    [System.Obsolete]
    public async void LoadStageScene(int stage)
    {
        gameOver = false;
        this.StageNum = stage;
        await PushSceneState(new StateStage());
    }

    public async void LoadStageScene()
    {
        gameOver = false;
        await PushSceneState(new StateStage());
    }

    class StateStage : IGameState
    {
        private PlayerController player;
        private SceneGenerator sceneGenerator;
        private Parallax parallax;

        private float progress = 0f;
        private float mapLength;

        public void Enter(ISceneHandler gm)
        {
            // UI 초기화
            gm.MainPageUI.SetActive(false);
            gm.StageUI.ShowInGameUI();
        }

        public void Exit(ISceneHandler gm)
        {
            Time.timeScale = 1f;

            gm.LoadingScene.SetActive(true);

            gm.StageUI.CloseInGameUI();
            gm.MainPageUI.SetActive(true);


            /// 스테이지 초기화권을 사용하고 강제로 메뉴버튼으로 나갈때 다시 클리어 상태로 바뀜
            /// 원래는 게임오버나 게임 클리어시 처리하지만 강제로 나갈 때도 생각해야 함
            if(gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum] == 3)
                gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum] = 2;
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

            //if (gm.IsGameOver())
            //{
            //    if (Input.anyKeyDown)
            //    {
            //        gm.Restart();

            //        // 다시 시작
            //        UniTask.Create(async () => await Start(gm));
            //    }

            //}
        }

        public async UniTask Start(ISceneHandler gm)
        {
            // 아무 키나 누르세요! 인터페이스를 없애고 씬을 로드하면 바로 시작하는 것으로 변경.

            // 스테이지 들어갈시 스테이지 안에 있는 아이템의 개수 데이터 초기화
            gm.Data.ItemPerStage[(gm.ChapterNum - 1) * 5 + gm.StageNum] = 0;
            gm.Data.Acorn = 0;

            Time.timeScale = 1f;

            // 재시작 시 문제가 되는 코드... 이걸 없애야 할까요?
            // if (!SceneManager.GetActiveScene().name.Equals("Stage"))
                await SceneManager.LoadSceneAsync("Stage"); // Scene이 완전히 시작할 때까지 대기

            gm.PlayBGM(gm.ChapterNum - 1);

            // 개선된 구조의 맵 데이터 로드 메소드
            ObjData[] objs = await gm.GetObjsAsync(gm.ChapterNum, gm.StageNum); // web request가 완전히 완료될 때까지 대기

            sceneGenerator = FindObjectOfType<SceneGenerator>();
            player = FindObjectOfType<PlayerController>();
            parallax = FindObjectOfType<Parallax>();
            parallax.SetSeason(gm.ChapterNum);

            if (gm.ShopItemContainer[Inventory.MAGNET]) // magnet 아이템을 사용했다면
            {
                player.UseMagetItem();
                gm.ShopItemContainer[0] = false;
            }
            if (gm.ShopItemContainer[Inventory.BUBBLE_SHIELD]) // bubbleShield 아이템을 사용했다면
            {
                player.BubbleShield.SetActive(true);
                gm.ShopItemContainer[1] = false;
            }
            if (gm.ShopItemContainer[Inventory.BALLOON]) // balloon 아이템을 사용했다면
            {
                player.UseBalloonItem();
                gm.ShopItemContainer[2] = false;
            }
            if(gm.ShopItemContainer[Inventory.INITSTAGE])  // initStage 아이템을 사용했다면 
            {
                // 해당 스테이지를 엔딩아이템이 나오는 초기화 아이템 사용한(3) 상태로 만든다.
                gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum] = 3;
                gm.ShopItemContainer[3] = false;
            }

            sceneGenerator.GenerateScene(gm.ChapterNum, objs);
            progress = 0f;
            mapLength = sceneGenerator.MapLength;
            sceneGenerator.scrollStart(true);

            gm.LoadingScene.SetActive(false);
        }

        public void Update(ISceneHandler gm)
        {
            // 플레이어가 벽에 붙지 않으면 앞으로 이동
            if (!player || player.IsFlying || player.IsTop || (player.AttachedTree && !player.IsTop) || gm.IsGameOver())
            {
                return;
            }
            sceneGenerator?.Scroll(gm.ScrollSpeed);
            parallax?.Scroll();

            progress += gm.ScrollSpeed * Time.deltaTime;
            gm.ProgressBar.value = progress / mapLength;
        }
    }
}