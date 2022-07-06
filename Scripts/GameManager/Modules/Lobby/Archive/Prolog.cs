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
    ///  ó�� ���۽� ���ѷαװ� Ȱ��ȭ�Ǹ� �����ϴ� �Լ�
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

        /// ������ �������̰� ó�� ó�� ���� ���ѷα� �϶��� Ȱ��ȭ
        startButton.SetActive(page == maxPage && dataHandler.Data.IsFirst);
        

        // ���Ҹ� ����
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
    /// ���� ó�� ���ѷα׸� ���� �����Ϸ�����!! ��ư Ŭ���� isFirst false�� ���ְ� ��Ȱ��ȭ
    /// </summary>
    public void StartGame()
    {
        dataHandler.Data.IsFirst = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// isFirst�� false�� ����(���� ó�� ���ѷαװ� �ƴ� ��) ȭ�� ���� ��ġ�ϸ� ���� �� �ְ���
    /// </summary>
    public void ExitProlog()
    {
        if (dataHandler.Data.IsFirst == false)
            gameObject.SetActive(false);
    }
}
