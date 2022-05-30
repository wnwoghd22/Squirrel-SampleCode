using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public const int ENDING_NORMAL_0 = 0;
    public const int ENDING_NORMAL_1 = 1;
    public const int ENDING_NORMAL_2 = 2;
    public const int ENDING_NORMAL_3 = 3;
    public const int ENDING_NORMAL_4 = 4;

    public const int ENDING_FULL_SPRING = 5;
    public const int ENDING_FULL_SUMMER = 6;
    public const int ENDING_FULL_AUTUMN = 7;
    public const int ENDING_FULL_WINTER = 8;
    public const int ENDING_FULL_ALL = 9;

    public const int ENDING_ZERO_SPRING = 10;
    public const int ENDING_ZERO_SUMMER = 11;
    public const int ENDING_ZERO_AUTUMN = 12;
    public const int ENDING_ZERO_WINTER = 13;
    public const int ENDING_ZERO_ALL = 14;

    public const int COUNT_ACORN_ALL = 48;
    public const int COUNT_ACORN_SEASON = 12;

    public const int COUNT_NORMAL_4 = 41;
    public const int COUNT_NORMAL_3 = 36;
    public const int COUNT_NORMAL_2 = 26;
    public const int COUNT_NORMAL_1 = 14;

    [SerializeField] GameObject endingTab;
    [SerializeField] Text itemCountText;
    [SerializeField] GameObject endingCutScene;
    [SerializeField] WarningPopUp warningPopUp;

    IFileHandler fileHandler;
    SaveData Data => fileHandler.Data;

    Achievement achievement;
    ShareManager shareManager;
    ISEHandler se;

    public ISceneHandler sceneHandler;

    private void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        fileHandler = gm;
        shareManager = FindObjectOfType<ShareManager>();
        se = gm;
    }

    /// <summary>
    /// gameManager > SelectChapter > ChapterButtons > EndingButton
    /// </summary>
    public void ActivateEndingTab()
    {
        se.PlaySE(se.SE_CLICK);

        int springItemCount = Data.MyItemPerStage[1] + Data.MyItemPerStage[2] + Data.MyItemPerStage[3] + Data.MyItemPerStage[4];
        int summerItemCount = Data.MyItemPerStage[6] + Data.MyItemPerStage[7] + Data.MyItemPerStage[8] + Data.MyItemPerStage[9];
        int autumnItemCount = Data.MyItemPerStage[11] + Data.MyItemPerStage[12] + Data.MyItemPerStage[13] + Data.MyItemPerStage[14];
        int winterItemCount = Data.MyItemPerStage[16] + Data.MyItemPerStage[17] + Data.MyItemPerStage[18] + Data.MyItemPerStage[19];

        int totalItemCount = springItemCount + summerItemCount + autumnItemCount + winterItemCount;

        itemCountText.text = "= " + totalItemCount;

        /// 겨울 4스테이지 열리면 엔딩보러가기 버튼도 활성화
        if (Data.ClearStatus[3].intArr[4] != 0) endingTab.SetActive(true);
        else
        {
            warningPopUp.gameObject.SetActive(true);
            warningPopUp.setText("겨울 3스테이지를 클리어하세요!!");
        }
    }

    /// <summary>
    /// gameManager > EndingTab > YesButton
    /// </summary>
    public void getEnding()
    {
        se.PlaySE(se.SE_CLICK);

        int endingIndex = CountEndingItem();

        achievement = Data.Achievements[endingIndex];

        Data.Achievements[endingIndex].state = Achievement.OLD;   // 이번 회차에 만족한 엔딩을 해금

        // 엔딩 화면 보여주기!!
        var endingCutSceneLogic =  endingCutScene.GetComponent<EndingCutScene>();
        endingCutSceneLogic.getEndingIndex(endingIndex);
        endingCutScene.SetActive(true);
        endingCutSceneLogic.endingText.text = fileHandler.Data.Achievements[endingIndex].name;

        // 엔딩아이템 개수 초기화
        for (int i = 0; i < Data.MyItemPerStage.Length; i++) Data.MyItemPerStage[i] = 0;

        // 스테이지 클리어 초기화
        for(int i = 0; i < 4; i++)
            for(int j = 0; j < 5; j++)
            {
                if (j == 0) Data.ClearStatus[i].intArr[j] = 2;  // 2회차일 때는 0스테이지는 클리어 한걸로
                else if (i == 0 && j == 1) Data.ClearStatus[i].intArr[j] = 1; // 2회차일 때는 봄 챕터 1스테이지부터 하는걸로
                else Data.ClearStatus[i].intArr[j] = 0;
            }

        //fileHandler.Save();
    }

    /// <summary>
    /// 이때까지 모은 엔딩아이템을 세어서 조건에 맞는 엔딩index를 반환
    /// </summary>
    /// <returns>엔딩 번호</returns>
    private int CountEndingItem()
    {
        int springItemCount = Data.MyItemPerStage[1] + Data.MyItemPerStage[2] + Data.MyItemPerStage[3] + Data.MyItemPerStage[4];
        int summerItemCount = Data.MyItemPerStage[6] + Data.MyItemPerStage[7] + Data.MyItemPerStage[8] + Data.MyItemPerStage[9];
        int autumnItemCount = Data.MyItemPerStage[11] + Data.MyItemPerStage[12] + Data.MyItemPerStage[13] + Data.MyItemPerStage[14];
        int winterItemCount = Data.MyItemPerStage[16] + Data.MyItemPerStage[17] + Data.MyItemPerStage[18] + Data.MyItemPerStage[19];

        int totalItemCount = springItemCount + summerItemCount + autumnItemCount + winterItemCount;

        bool isClearChallengeStage = true;
        for(int i = 0; i < 4; i++)
        {
            if (Data.ClearStatus[i].intArr[4] != 2) isClearChallengeStage = false;
        }
        
        /**
         * 우선순위에 따라 훑으면서 조건을 만족시키면 바로 빠져나가게 한다면 간단한 작업이 됩니다.
         * 
         * 굳이 여러 식에 걸쳐서 확인할 필요가 없도록 할 수 있다면...
         */
        
        // 도전난이도 올클리어 체크
        if(isClearChallengeStage)
        {
            // 풀 파밍 체크
            if (totalItemCount == COUNT_ACORN_ALL) return ENDING_FULL_ALL;
            // 각 계절에 대해서는 우선순위를 가지는 것부터 확인
            if (winterItemCount == COUNT_ACORN_SEASON) return ENDING_FULL_WINTER;
            if (autumnItemCount == COUNT_ACORN_SEASON) return ENDING_FULL_AUTUMN;
            if (summerItemCount == COUNT_ACORN_SEASON) return ENDING_FULL_SUMMER;
            if (springItemCount == COUNT_ACORN_SEASON) return ENDING_FULL_SPRING;
        }

        // 제로 파밍 체크
        if (totalItemCount == 0) return ENDING_ZERO_ALL;
        // 각 계절에 대해서는 우선순위를 가지는 것부터 확인
        if (springItemCount == 0) return ENDING_ZERO_SPRING;
        if (summerItemCount == 0) return ENDING_ZERO_SUMMER;
        if (autumnItemCount == 0) return ENDING_ZERO_AUTUMN;
        if (winterItemCount == 0) return ENDING_ZERO_WINTER;

        // 노말 엔딩 체크
        if (totalItemCount >= COUNT_NORMAL_4) return ENDING_NORMAL_4;
        if (totalItemCount >= COUNT_NORMAL_3) return ENDING_NORMAL_3;
        if (totalItemCount >= COUNT_NORMAL_2) return ENDING_NORMAL_2;
        if (totalItemCount >= COUNT_NORMAL_1) return ENDING_NORMAL_1;
        return ENDING_NORMAL_0;
    }

    public void Share() => shareManager.ShareScreenShot(achievement);

    public async void ShareByInstagram() => await shareManager.ShareScreenShotThroughPlatformAsync(achievement, ShareManager.INSTAGRAM);
    public async void ShareByTwitter() => await shareManager.ShareScreenShotThroughPlatformAsync(achievement, ShareManager.TWITTER);
    public async void ShareByFacebook() => await shareManager.ShareScreenShotThroughPlatformAsync(achievement, ShareManager.FACEBOOK);


    public void ShareEndingByInstagram()
    {
        if (sceneHandler.Stack.Count == 2) shareManager.ShareImage(achievement, ShareManager.INSTAGRAM);
    }
    public void ShareEndingByTwitter()
    {
        if (sceneHandler.Stack.Count == 2) shareManager.ShareImage(achievement, ShareManager.TWITTER);
    }
    public void ShareEndingByFacebook()
    {
        if (sceneHandler.Stack.Count == 2) shareManager.ShareImage(achievement, ShareManager.FACEBOOK);
    }
}
