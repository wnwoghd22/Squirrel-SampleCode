using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public partial class GameManager : MonoBehaviour
{
    [Header("Lobby UI")]
    [SerializeField] private GameObject mainPageUI;

    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private SelectChapterUI selectChapterUI;
    [SerializeField] private SelectStageUI selectStageUI;
    [SerializeField] private SelectItemUI selectItemUI;
    [System.Obsolete][SerializeField] private GameObject hideoutUI;

    [SerializeField] private WarningPopUp WarningPopUp;  // 경고창(예로 스테이지 잠김일 때 뜨는 창, 아직 안 열린 스테이지 접근할 때 뜨는 창, 아직 업데이트 못한 버튼 접금할 때 뜨는 창)

    [SerializeField] private LobbyMenu lobbyMenu;
    [SerializeField] private LobbyUpperBar upperBar;
    [SerializeField] private GameObject tutorialGuide;  // 게임 설치하고 처음 챕터 들어갈 때 튜토리얼 확인하라는 가이드창
    [SerializeField] private GameObject showEndingBook;  // 업적 달성하면 로비에 엔딩북으로 가는 버튼 활성화

    [Header("In game")]
    [SerializeField] private StageUI stageUI;
    [SerializeField] private float scrollSpeed = 3f;

    [Header("Util")]
    [SerializeField] private GameObject loadingScene;

    [Header("Manager")]
    [SerializeField] private DataManager dataManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private VibrationManager vibrationManager;
    [SerializeField] private GPGSManager gpgsManager;
    [SerializeField] private ShareManager shareManager;
    [SerializeField] private MobileAdsManager adsManager;
    [SerializeField] private Inventory inventory;

    private bool gameOver = false;

    private const string LINK_FACEBOOK = "https://www.facebook.com/squirreljump";
    private const string LINK_INSTAGRAM = "https://www.instagram.com/squirrel_jump_/";
    private const string LINK_TWITTER = "https://twitter.com/squirrel__jump";

    /// <summary>
    /// 개발팀 특전 치트 패키지 보너스!!!
    /// 그냥 누르기만 해도 점수가 콸콸
    /// +
    /// 모든 스테이지 클리어로 처리!!
    /// </summary>
    /// <param name="count"></param>
    public void AddAcorn(int count)
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 5; j++)
                Data.ClearStatus[i].intArr[j] = 2;  

        Acorn += 100;

        //Save();
    }

    /// <summary>
    /// 도토리 0으로 초기화
    /// 모든 스테이지 초기화
    /// </summary>
    public void resetData()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 5; j++)
                Data.ClearStatus[i].intArr[j] = 0;

        Data.ClearStatus[0].intArr[0] = 1;

        Acorn = 0;

        //Save();
    }

    public void unLockAchievement()
    {
        for (int i = 0; i < 16; i++)
            Data.Achievements[i].state = Achievement.OLD;
        for (int i = 16; i < Data.Achievements.Count; i++)
            Data.Achievements[i].state = Achievement.NEW;

        //Save();
    }

    public void LockAchievement()
    {
        for (int i = 0; i < Data.Achievements.Count; i++)
            Data.Achievements[i].state = Achievement.LOCKED;

        //Save();
    }

    /// <summary>
    /// 게임을 종료했을 때
    /// </summary>
    private void OnApplicationQuit()
    {
        Debug.Log("게임을 종료할 때 불러오는 메소드");
        Save();
    }

    /// <summary>
    /// 홈버튼을 누르는 등 게임이 잠시 내려갔을 때
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        if (pause) 
        {
            Debug.Log("게임 일시정지 불러오는 메소드");
            OnApplicationQuit(); 
        }
    }

    private void OpenURL(string url) => Application.OpenURL(url);
    public void OpenFacebook() => OpenURL(LINK_FACEBOOK);
    public void OpenInstagram() => OpenURL(LINK_INSTAGRAM);
    public void OpenTwitter() => OpenURL(LINK_TWITTER);
}
