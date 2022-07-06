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

        /// �ܿ� 4�������� ������ ������������ ��ư�� Ȱ��ȭ
        if (Data.ClearStatus[3].intArr[4] != 0) endingTab.SetActive(true);
        else
        {
            warningPopUp.gameObject.SetActive(true);
            warningPopUp.setText("�ܿ� 3���������� Ŭ�����ϼ���!!");
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

        Data.Achievements[endingIndex].state = Achievement.OLD;   // �̹� ȸ���� ������ ������ �ر�

        // ���� ȭ�� �����ֱ�!!
        var endingCutSceneLogic =  endingCutScene.GetComponent<EndingCutScene>();
        endingCutSceneLogic.getEndingIndex(endingIndex);
        endingCutScene.SetActive(true);
        endingCutSceneLogic.endingText.text = fileHandler.Data.Achievements[endingIndex].name;

        // ���������� ���� �ʱ�ȭ
        for (int i = 0; i < Data.MyItemPerStage.Length; i++) Data.MyItemPerStage[i] = 0;

        // �������� Ŭ���� �ʱ�ȭ
        for(int i = 0; i < 4; i++)
            for(int j = 0; j < 5; j++)
            {
                if (j == 0) Data.ClearStatus[i].intArr[j] = 2;  // 2ȸ���� ���� 0���������� Ŭ���� �Ѱɷ�
                else if (i == 0 && j == 1) Data.ClearStatus[i].intArr[j] = 1; // 2ȸ���� ���� �� é�� 1������������ �ϴ°ɷ�
                else Data.ClearStatus[i].intArr[j] = 0;
            }

        //fileHandler.Save();
    }

    /// <summary>
    /// �̶����� ���� ������������ ��� ���ǿ� �´� ����index�� ��ȯ
    /// </summary>
    /// <returns>���� ��ȣ</returns>
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
         * �켱������ ���� �����鼭 ������ ������Ű�� �ٷ� ���������� �Ѵٸ� ������ �۾��� �˴ϴ�.
         * 
         * ���� ���� �Ŀ� ���ļ� Ȯ���� �ʿ䰡 ������ �� �� �ִٸ�...
         */
        
        // �������̵� ��Ŭ���� üũ
        if(isClearChallengeStage)
        {
            // Ǯ �Ĺ� üũ
            if (totalItemCount == COUNT_ACORN_ALL) return ENDING_FULL_ALL;
            // �� ������ ���ؼ��� �켱������ ������ �ͺ��� Ȯ��
            if (winterItemCount == COUNT_ACORN_SEASON) return ENDING_FULL_WINTER;
            if (autumnItemCount == COUNT_ACORN_SEASON) return ENDING_FULL_AUTUMN;
            if (summerItemCount == COUNT_ACORN_SEASON) return ENDING_FULL_SUMMER;
            if (springItemCount == COUNT_ACORN_SEASON) return ENDING_FULL_SPRING;
        }

        // ���� �Ĺ� üũ
        if (totalItemCount == 0) return ENDING_ZERO_ALL;
        // �� ������ ���ؼ��� �켱������ ������ �ͺ��� Ȯ��
        if (springItemCount == 0) return ENDING_ZERO_SPRING;
        if (summerItemCount == 0) return ENDING_ZERO_SUMMER;
        if (autumnItemCount == 0) return ENDING_ZERO_AUTUMN;
        if (winterItemCount == 0) return ENDING_ZERO_WINTER;

        // �븻 ���� üũ
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
