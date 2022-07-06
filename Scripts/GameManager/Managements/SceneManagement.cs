using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public partial class GameManager : ISceneHandler
{
    private Stack<IGameState> stack;
    public Stack<IGameState> Stack => stack;

    public bool IsGameOver() => gameOver;

    public GameObject MainPageUI => mainPageUI;

    public SelectChapterUI SelectChapterUI => selectChapterUI;
    public SelectStageUI SelectStageUI => selectStageUI;
    public SelectItemUI SelectItemUI => selectItemUI;
    public GameObject LobbyUI => lobbyUI;
    public GameObject HideoutUI => hideoutUI;
    public LobbyMenu LobbyMenu => lobbyMenu;

    public StageUI StageUI => stageUI;

    public GameObject LoadingScene => loadingScene;

    public LobbyUpperBar LobbyUpperBar => upperBar;

    public GameObject TutorialGuide => tutorialGuide;
    public GameObject ShowEndingBook => showEndingBook;

    [System.Obsolete]
    public void SetUpperBar()
    {
        upperBar.SetDateToday();
        //upperBar.SetAcornText(data.MyAcorn);
        LobbyUpperBar.SetAcornText(Data.MyAcorn);
    }

    public async UniTask PushSceneState(IGameState state)
    {
        if (stack.Count > 0)
            stack.Peek().Exit(this);

        stack.Push(state);
        stack.Peek().Enter(this);
        await stack.Peek().Start(this);
    }

    public void PopSceneState()
    {
        stack.Peek().Exit(this);
        stack.Pop();

        stack.Peek().Enter(this);
        UniTask.Create(async () => await stack.Peek().Start(this));
    }

    public void PopTwoStates()
    {
        PopSceneState();
        PopSceneState();
    }

    public void ExitGame()
    {
        // if (Application.platform == RuntimePlatform.Android)
        //{
        //    AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        //    activity.Call<bool>("moveTaskToBack", true);
        //}
        //else
        //{
            Application.Quit();
        //}
    }

    // GameManager가 Scene에 단 하나만 존재하도록 해주는 코드.
    private static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) // 게임을 처음 시작했을 때. 할당 후 파괴하지 않도록 설정.
        {
            stack = new Stack<IGameState>();
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject); // 2개 이상인 경우 오브젝트 파괴.
        }
    }
    // DontDestroyOnLoad 속성을 부여했기 때문에 게임 시작시 최초 1회만 호출.
    private async UniTaskVoid Start()
    {

        Debug.Log("Start");

        bool ifFirst = false;

        try
        {
#if UNITY_ANDROID
            gpgsManager.LogIn(); // 안드로이드 기기 실행시 자동 로그인
#endif
            // 게임 시작시 기기에 저장된 데이터로 Load
            // 게임 처음 접속하면 Load해도 데이터가 없기 때문에 data는 null값이다.
            Load();
            // 따라서 data가 null값이면 SaveData생성하고 저장
            if (Data == null)
            {

                // 여기에 처음 접속하는거니까 스토리 같은거 넣어도 될듯(바나나 오브 레전드 처럼)
                Debug.Log("게임에 처음 접속하시는군요!!");
                ifFirst = true;

                await dataManager.ConstructNewSaveData();
            }
            /// 이 부분은 나중에 데이터가 추가되거나 변경되었을 때 바로바로 json파일에서 최신화된것을 볼 수 있도록 저장하는 부분입니다.
            else
            {
                await Data.UpdataAchievementAsync();
                Save();
            }
            // 게임 시작 시 도토리 개수 UI 설정
            Acorn = Acorn;
        }
        catch
        {

        }
        finally
        {
            if (stack.Count == 0)
            {
                if (SceneManager.GetActiveScene().name == "Lobby")
                {
                    await PushSceneState(new StateLobby());

                    Debug.Log(ifFirst);

                    if (ifFirst)
                    {
                        Data.IsFirst = true;
                        Archive endingBook = FindObjectOfType<Archive>();
                        endingBook.ShowProlog();
                    }
                }
                else if (SceneManager.GetActiveScene().name == "Stage") await PushSceneState(new StateStage());
                // else if (SceneManager.GetActiveScene().name == "Infinite") PushSceneState(new StateStage());
            }
        }
    }

    void Update()
    {
        if (stack.Count > 0)
        {
            stack.Peek().HandleInput(this);
            stack.Peek().Update(this);
        }
    }
}