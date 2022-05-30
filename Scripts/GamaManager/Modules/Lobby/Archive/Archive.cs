using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Archive : MonoBehaviour
{
    [Header("Book")]
    [SerializeField] GameObject archiveTab;
    [SerializeField] BookSlot[] slots;
    [SerializeField] GameObject nextPageButton;
    [SerializeField] GameObject previousPageButton;
    [SerializeField] Text pageText;
    [SerializeField] GameObject prologPage;

    [Header("Pop Up")]
    [SerializeField] GameObject popUp;

    [Header("Ending")]
    [SerializeField] EndingCutScene endingCutScene;
    [SerializeField] Ending ending;

    [SerializeField] GameObject endingPopUp;
    [SerializeField] Image endingImage;
    [SerializeField] Text endingTitle;
    [SerializeField] Text endingCondition;
    [SerializeField] Text endingContent;
    [SerializeField] Sprite[] endingSprites;  // ���� �׸�

    [Header("Achievement")]
    IAchievementHandler achievementManager;
    public List<Achievement> Achievements => achievementManager.Achievements;
    [SerializeField] GameObject achievementPopUp;
    [SerializeField] Sprite[] achievementSprites;  // ���� �׸�

    [Header("Prolog")]
    [SerializeField] Prolog prologTab;

    [Header("Hint")]
    [SerializeField] GameObject endingLockPopUp;
    [SerializeField] GameObject endingHintPopUp;
    [SerializeField] GameObject endingCancelPopUp;
    [SerializeField] Sprite[] endingHintImage;
    [SerializeField] Image hintImage;

    private ISceneHandler sceneHandler;
    private IPropertyHandler acorn;
    private ISEHandler se;
    private ISoundHandler background;

    private IMailHandler mailHandler;

    private ShareManager shareManager;

    private const int HINT_COST = 50;
    private const int MAX_ENDING_NUM = 56; // ���Ƿ� ���� ����
    private const int PAGE_COLUMN = 4;
    private const int PAGE_ROW = 2;
    private const int SLOT_COUNT = PAGE_COLUMN * PAGE_ROW;
    private int page = Mathf.CeilToInt((float)MAX_ENDING_NUM / SLOT_COUNT);

    private int curEndingIndex;

    private int currentPage = 0;
    public int CurrentPage
    {
        get => currentPage;
        set
        {
            if (value < 0)
                currentPage = 0;
            else if (value > page - 1)
                currentPage = page - 1;
            else
            {
                se.PlaySE(se.SE_PAPER);
                currentPage = value;
            }
        }
    }

    void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        sceneHandler = gm;

        acorn = gm;
        se = gm;
        background = gm;

        endingCutScene.sceneHandler = gm;
        ending.sceneHandler = gm;

        mailHandler = FindObjectOfType<MailManager>();
        achievementManager = FindObjectOfType<AchievementManager>();
        shareManager = FindObjectOfType<ShareManager>();
    }

    void Update()
    {

    }

    public void ActivateArchiveTab(bool active)
    {
        if (active)
        {
            se.PlaySE(se.SE_PAPER);

            ShowPage(currentPage = achievementManager.New > 0 ? achievementManager.New / SLOT_COUNT : currentPage);
        }

        // ������ ���̱�
        archiveTab.SetActive(active);
    }

    private void ShowPage(int num)
    {
        previousPageButton.SetActive(num > 0);
        nextPageButton.SetActive(num < page - 1);

        //pageText.text = (CurrentPage + 1).ToString() + " / " + page.ToString();
        switch (num)
        {
            case 0:
            case 1:
                pageText.text = "���� ������ " + (CurrentPage + 1).ToString() + " / " + page.ToString();
                break;
            case 2:
            case 3:
            case 4:
            case 5:
                pageText.text = "���� ������ " + (CurrentPage + 1).ToString() + " / " + page.ToString();
                break;
            case 6:
                pageText.text = "���ѷα� ������ " + (CurrentPage + 1).ToString() + " / " + page.ToString();
                break;
        }

        for (int i = 0; i < 8; i++) slots[i].gameObject.SetActive(num != 6);
        prologPage.SetActive(num == 6);

        if (num == 6) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i + num * 8 >= achievementManager.Achievements.Count || achievementManager.Achievements[i + num * 8].name.Trim() == "empty") // �ش� ������ �����Ǿ����� �ʴٸ� noSlot
            {
                slots[i].SetEmptySlot();
            }
            else
            {
                slots[i].SetSlot(num, achievementManager.Achievements[i + num * 8], endingSprites[i + num * 8]);
            }
        }
    }
    public void ShowPreviousPage() => ShowPage(--CurrentPage);
    public void ShowNextPage() => ShowPage(++CurrentPage);

    // 0~1 page ����, 2~5 page ����, 6 page ���ѷα�
    public void ShowEndingPage() => ShowPage(CurrentPage = 0);
    public void ShowAchievementPage() => ShowPage(CurrentPage = 2);
    public void ShowPrologPage() => ShowPage(CurrentPage = 6);

    public void ShowProlog()
    {
        prologTab.CurrentPage = 0;
        prologTab.gameObject.SetActive(true);
        background.PlayProlog();
    }

    public void HandleClick(Achievement achievement)
    {
        Debug.Log("Handle Archive Slot Click: " + achievement.index);

        switch (CurrentPage)
        {
            case 0: // Ending
            case 1:
                curEndingIndex = achievement.index;
                if (achievement.IsLocked)
                {
                    se.PlaySE(se.SE_LOCKED);

                    endingLockPopUp.SetActive(true);
                    return;
                }
                else
                {
                    /*popUp.SetActive(true);  // �׳� ������

                    endingPopUp.SetActive(true);  // ��������
                    achievementPopUp.SetActive(false);  // ��������

                    endingPopUp.SetActive(true);  // ��������
                    endingImage.sprite = endingSprites[achievement.index];
                    endingTitle.text = achievement.name;
                    endingCondition.text = achievement.condition;
                    endingContent.text = achievement.detail;*/


                    /// �����������⿡�� �Ѱ�ó�� ���� ���丮�� �����ִ� ������ �ٲ���ϴ�.
                    endingCutScene.getEndingIndex(curEndingIndex);
                    endingCutScene.gameObject.SetActive(true);
                    endingCutScene.endingText.text = Achievements[curEndingIndex].name;


                    if (achievement.IsNew)
                        achievementManager.SetAchievementOld(achievement.index); // �ѹ� �����ϱ� true�� �ٲ���� ��

                }
                break;
            case 2: // Achievement
            case 3:
            case 4:
            case 5:
                if (achievement.IsLocked)
                {
                    se.PlaySE(se.SE_LOCKED);
                    return;
                }

                popUp.SetActive(true);

                endingPopUp.SetActive(false);
                achievementPopUp.SetActive(true);

                achievementPopUp.SetActive(true);
                achievementPopUp.GetComponent<Image>().sprite = achievementSprites[achievement.index - 16];

                if (achievement.IsNew)
                {
                    SendAchievementReward(achievement.index);
                    achievementManager.SetAchievementOld(achievement.index); // �ѹ� �����ϱ� true�� �ٲ���� ��
                }

                break;
            default:
                break;
        }
    }

    /// <summary>
    ///  �ε����� ���� ������ �����ִ� �Լ� 
    /// </summary>
    /// <param name="index">���� �ε��� ��ȣ</param>
    public void SendAchievementReward(int index)
    {
        if (index == 47)
        {
            mailHandler.Send(new Mail(achievementManager.Achievements[index].name, eReward.ACORN, 1000));
            mailHandler.Send(new Mail(achievementManager.Achievements[index].name, eReward.BALLOON, 10));
            mailHandler.Send(new Mail(achievementManager.Achievements[index].name, eReward.BUBBLE, 10));
            mailHandler.Send(new Mail(achievementManager.Achievements[index].name, eReward.MAGNET, 10));
            return;
        }

        Achievement achievement = achievementManager.Achievements[index];
        Debug.Log("�������� : " + achievement.reward + " ������? " + achievement.rewardCount);
        mailHandler.Send(new Mail(achievement.name, (eReward)achievement.reward, achievement.rewardCount));
    }

    public void ShowEndingHint()
    {
        endingLockPopUp.SetActive(false);
        // ���丮�� 50���� ������ ��Ʈ ���
        if (acorn.Acorn >= HINT_COST)
        {
            acorn.Acorn -= HINT_COST;
            endingHintPopUp.SetActive(true);
            hintImage.sprite = endingHintImage[curEndingIndex];
        }
        else
        {
            endingCancelPopUp.SetActive(true);
        }
    }

    public void ShareEnding() => shareManager.ShareScreenShot(Achievements[curEndingIndex]);

    // public async void ShareEndingByInstagram() => await shareManager.ShareScreenShotThroughPlatformAsync(Achievements[curEndingIndex], ShareManager.INSTAGRAM);
    // public async void ShareEndingByTwitter() => await shareManager.ShareScreenShotThroughPlatformAsync(Achievements[curEndingIndex], ShareManager.TWITTER);
    // public async void ShareEndingByFacebook() => await shareManager.ShareScreenShotThroughPlatformAsync(Achievements[curEndingIndex], ShareManager.FACEBOOK);
    public void ShareEndingByInstagram() 
    {
        if (sceneHandler.Stack.Count == 1) shareManager.ShareImage(Achievements[curEndingIndex], ShareManager.INSTAGRAM); 
    }
    public void ShareEndingByTwitter() 
    {
        if (sceneHandler.Stack.Count == 1) shareManager.ShareImage(Achievements[curEndingIndex], ShareManager.TWITTER);
    }
    public void ShareEndingByFacebook()
    {
        if (sceneHandler.Stack.Count == 1) shareManager.ShareImage(Achievements[curEndingIndex], ShareManager.FACEBOOK);
    }

}
