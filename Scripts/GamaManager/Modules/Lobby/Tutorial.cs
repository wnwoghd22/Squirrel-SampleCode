using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] SpriteList[] sprites;

    [SerializeField] GameManager gm;

    [SerializeField] private Image prologImage;
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject previousPageButton;
    [SerializeField] private GameObject exitButton;

    IGameFlowHandler flowHandler => gm;

    private int maxPage => sprites[flowHandler.ChapterNum - 1].list.Length - 1;

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

    /// <summary>
    ///  처음 시작시 프롤로그가 활성화되면 수행하는 함수
    /// </summary>
    private void OnEnable()
    {
        CurrentPage = 0;
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

        prologImage.sprite = sprites[flowHandler.ChapterNum - 1].list[page];

        exitButton.SetActive(page == maxPage);

        // 새소리 실행
        // if (page == 1 || page == 3) Sound.Play
    }
}
