using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingCutScene : MonoBehaviour
{
    [SerializeField] SpriteList[] spriteLists;

    [SerializeField] private Image endingCutSceneImage;
    [SerializeField] public Text endingText;
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject previousPageButton;
    [SerializeField] private GameObject reStartButton;

    [SerializeField] private GameObject endingTab;

    [Header("Share")]
    [SerializeField] GameObject shareButtonInstagram;
    [SerializeField] GameObject shareButtonFacebook;
    [SerializeField] GameObject shareButtonTwitter;
    [SerializeField] GameObject shareText;

    public ISceneHandler sceneHandler;

    private int index;
    private int maxPage;

    private int currentPage = 0;
    public int CurrentPage
    {
        get => currentPage;
        set
        {
            if (value < 0)
                currentPage = 0;
            else if (value >= maxPage)
                currentPage = maxPage;
            else
                currentPage = value;
        }
    }

    // ���° �������� ending���� �˷���
    public void getEndingIndex(int i)
    {
        index = i;
        maxPage = spriteLists[index].list.Length - 1;
    }

    /// <summary>
    ///  ó�� ���۽� �����ƽ��� Ȱ��ȭ�Ǹ� �����ϴ� �Լ�
    /// </summary>
    private void OnEnable()
    {
        showPage(CurrentPage);
    }

    public void nextPage()
    {
        CurrentPage += 1;
        showPage(CurrentPage);
    }

    public void previousPage()
    {
        CurrentPage -= 1;
        showPage(CurrentPage);
    }

    void showPage(int page)
    {
        nextPageButton.SetActive(page < maxPage);
        previousPageButton.SetActive(page > 0);

        endingCutSceneImage.sprite = spriteLists[index].list[page];

        // �ٽ��ϱ� ��ư�� ������ �������� �� �׸��� ������������� �� �����϶��� Ȱ��ȭ
        reStartButton.SetActive(page == maxPage && sceneHandler.Stack.Count == 2);
        shareButtonInstagram.SetActive(page == maxPage);
        shareButtonFacebook.SetActive(page == maxPage);
        shareButtonTwitter.SetActive(page == maxPage);
        shareText.SetActive(page == maxPage);
    }

    /// <summary>
    /// ������������� ������ ���� �� ������ �ٽ�����! ��ư�� Ŭ����
    /// �������� �κ�ȭ������ ���ư�
    /// </summary>
    public void reStartGame()
    {
        CurrentPage = 0;
        gameObject.SetActive(false);
        endingTab.SetActive(false);

        /// �����Ͽ��� �� �����̶�� pop������ ����
        if (sceneHandler.Stack.Count == 1) return; 

        sceneHandler.PopSceneState();
    }

    /// <summary>
    /// stack�� ���̰� 1�̸� �κ񿡼� ����(�����Ͽ��� ����) �׶��� ȭ�� �� ��ġ�ϸ� ������
    /// </summary>
    public void exitCutScene()
    {
        if (sceneHandler.Stack.Count == 1)
        {
            CurrentPage = 0;
            gameObject.SetActive(false);
            endingTab.SetActive(false);
        }
    }
}
