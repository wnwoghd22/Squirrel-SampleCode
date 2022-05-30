using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class Prolog : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image prologImage;
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject previousPageButton;
    [SerializeField] private GameObject startButton;

    private int maxPage => sprites.Length - 1;

    private IDataHandler dataHandler;
    private ISEHandler se;
    private ISoundHandler back;

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
        dataHandler = FindObjectOfType<GameManager>();
        se = FindObjectOfType<GameManager>();
        back = FindObjectOfType<GameManager>();
        back.PlayProlog();
        CurrentPage = 0;
        showPage(CurrentPage);
    }
    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        back.PlayMain();
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

        prologImage.sprite = sprites[page];

        /// 마지막 페이지이고 처음 처음 보는 프롤로그 일때만 활성화
        startButton.SetActive(page == maxPage && dataHandler.Data.IsFirst);
        

        // 새소리 실행
        switch (page)
        {
            case 1:
                se.PlaySE(se.SE_PROLOG_1);
                break;
            case 3:
                se.PlaySE(se.SE_PROLOG_2);
                break;
        }
    }

    private async UniTask Call()
    {
        await UniTask.Yield();
    }

    /// <summary>
    /// 제일 처음 프롤로그를 보고 모험하러가자!! 버튼 클릭시 isFirst false로 해주고 비활성화
    /// </summary>
    public void StartGame()
    {
        dataHandler.Data.IsFirst = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// isFirst가 false일 때만(제일 처음 프롤로그가 아닐 때) 화면 밖을 터치하면 나갈 수 있게함
    /// </summary>
    public void ExitProlog()
    {
        if (dataHandler.Data.IsFirst == false)
            gameObject.SetActive(false);
    }
}
