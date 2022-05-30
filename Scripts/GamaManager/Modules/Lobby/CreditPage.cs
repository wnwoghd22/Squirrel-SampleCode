using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CreditPage : MonoBehaviour
{
    [SerializeField] GameObject creditTab;
    [SerializeField] RectTransform maskRect;
    [SerializeField] RectTransform credit;
    [SerializeField] Text creditText;

    private CancellationTokenSource creditCancelToken;

    [SerializeField] private float scrollSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCredit()
    {
        creditTab.SetActive(true);

        UniTask.Create(async () => await ShowCreditAsync());
    }
    public void StopCredit()
    {
        creditCancelToken?.Cancel();

        creditTab.SetActive(false);
    }

    private async UniTask ShowCreditAsync()
    {
        creditText.text = "";

        Canvas.ForceUpdateCanvases();

        credit.anchoredPosition = Vector2.zero;

        creditText.text = await FileManager.GetCreditFileForAndroid();

        Canvas.ForceUpdateCanvases();

        Vector2 currentPos = credit.anchoredPosition;
        currentPos.y -= credit.rect.height / 2f;
        credit.anchoredPosition = currentPos;
        Vector2 scrollPos = credit.anchoredPosition;

        float maxY = credit.rect.height / 2f + maskRect.rect.height;

        Debug.Log(currentPos.y + ", " + scrollPos.y + ", " + credit.rect.height + ", " + maskRect.rect.height);

        creditCancelToken = new CancellationTokenSource();

        while (credit.anchoredPosition.y < maxY)
        {
            await UniTask.Yield(cancellationToken:creditCancelToken.Token);
            scrollPos.y += scrollSpeed * Time.deltaTime;

            credit.anchoredPosition = scrollPos;
        }
    }
}
