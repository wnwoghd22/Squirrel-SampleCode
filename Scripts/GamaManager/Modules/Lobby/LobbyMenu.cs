using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menuButton;

    [Header("Children Buttons")]
    [SerializeField] private RectTransform[] buttons;

    [SerializeField] private float moveSpeed = 3f;

    private ISEHandler se;

    private float height;

    private CancellationTokenSource cancellationToken;

    private void Awake()
    {
        se = FindObjectOfType<GameManager>();

        menuButton.sizeDelta = new Vector2(1f, menuButton.rect.width);
        height = menuButton.rect.height;

        // configButton.sizeDelta = Vector2.one;
        // infoButton.sizeDelta = Vector2.one;
        // onOffButton.sizeDelta = Vector2.one;
    }

    private bool menuFolded = true;

    public void HandleLobbyMenu()
    {
        cancellationToken?.Cancel();
        cancellationToken = new CancellationTokenSource();

        se.PlaySE(se.SE_CLICK);

        if (menuFolded)
        {
            UnfoldMenu(cancellationToken.Token);
        } 
        else
        {
            FoldMenu(cancellationToken.Token);
        }
    }

    private void FoldMenu(CancellationToken token)
    {
        menuFolded = true;

        for (int i = 0; i < buttons.Length; ++i)
            UniTask.Create(async () => await FoldButtonsAsync(buttons[i], i + 1, moveSpeed, token));
    }

    private void UnfoldMenu(CancellationToken token)
    {
        menuFolded = false;

        for (int i = 0; i < buttons.Length; ++i)
            UniTask.Create(async () => await UnfoldButtonsAsync(buttons[i], i + 1, moveSpeed, token));
    }

    /// <summary>
    /// 스크립트 상에서 메뉴를 접을 때 호출하는 메소드
    /// </summary>
    public void FoldMenu()
    {
        menuFolded = true;
        cancellationToken?.Cancel();

        for (int i = 0; i < buttons.Length; ++i)
        {

            Vector3 pos = buttons[i].anchoredPosition;
            pos.y = 0f;
            buttons[i].anchoredPosition = pos;

            buttons[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 코루틴 대신 비동기 메소드로 애니메이션 효과를 구현!
    /// 매 프레임 버튼 UI를 밑으로 움직입니다.
    /// 다른 거리를 동시에 움직이도록 했습니다.
    /// </summary>
    /// <param name="button">버튼 게임오브젝트의 Rect Transform</param>
    /// <param name="offset">(양수) 버튼 크기의 정수배만큼 내려갑니다</param>
    /// <param name="spd">이동 속도</param>
    /// <returns></returns>
    private async UniTask UnfoldButtonsAsync(RectTransform button, int offset, float spd, CancellationToken token)
    {
        float targetDistance = height * offset;

        button.gameObject.SetActive(true);

        while (button.anchoredPosition.y > -targetDistance)
        {
            await UniTask.Yield(cancellationToken: token);

            Vector3 pos = button.anchoredPosition;
            pos.y -= 0.02f * height * spd * offset;
            button.anchoredPosition = pos;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="button"></param>
    /// <param name="offset">떨어져있는 거리</param>
    /// <param name="spd">이동 속도</param>
    /// <returns></returns>
    private async UniTask FoldButtonsAsync(RectTransform button, int offset, float spd, CancellationToken token)
    {
        while (button.anchoredPosition.y < 0)
        {
            await UniTask.Yield(cancellationToken: token);

            Vector3 pos = button.anchoredPosition;
            pos.y += 0.02f * height * spd * offset;
            button.anchoredPosition = pos;
        }

        button.gameObject.SetActive(false);
    }
}