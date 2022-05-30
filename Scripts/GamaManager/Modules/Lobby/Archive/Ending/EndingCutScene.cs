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

    // 몇번째 엔딩인지 ending에서 알려줌
    public void getEndingIndex(int i)
    {
        index = i;
        maxPage = spriteLists[index].list.Length - 1;
    }

    /// <summary>
    ///  처음 시작시 엔딩컷신이 활성화되면 수행하는 함수
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

        // 다시하기 버튼은 마지막 페이지일 때 그리고 엔딩보러가기로 본 엔딩일때만 활성화
        reStartButton.SetActive(page == maxPage && sceneHandler.Stack.Count == 2);
        shareButtonInstagram.SetActive(page == maxPage);
        shareButtonFacebook.SetActive(page == maxPage);
        shareButtonTwitter.SetActive(page == maxPage);
        shareText.SetActive(page == maxPage);
    }

    /// <summary>
    /// 엔딩보러가기로 엔딩을 봤을 때 나오는 다시하자! 버튼을 클릭시
    /// 나가지고 로비화면으로 돌아감
    /// </summary>
    public void reStartGame()
    {
        CurrentPage = 0;
        gameObject.SetActive(false);
        endingTab.SetActive(false);

        /// 엔딩북에서 본 엔딩이라면 pop해주지 않음
        if (sceneHandler.Stack.Count == 1) return; 

        sceneHandler.PopSceneState();
    }

    /// <summary>
    /// stack의 길이가 1이면 로비에서 본것(엔딩북에서 본것) 그때는 화면 밖 터치하면 나가짐
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
