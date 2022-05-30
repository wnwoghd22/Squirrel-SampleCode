using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class MobileAdsManager : MonoBehaviour
{
    public bool isTestMode;
    public Text LogText;
    public Button FrontAdsBtn;

    [SerializeField] private bool isBeta = false; // 베타 테스트 버전에서는 광고가 출력되지 않게 함

    private const float ADS_PROBABILITY = 0.3f;
    private float c = 0f;
    private float C
    {
        get => c;
        set => c = value < 0.1f ? value : 0.1f;
    }
    private const float PROBABILITY_EFF = 0.05f;
    private float probability = 0f;

    void Start()
    {
        var requestConfiguration = new RequestConfiguration
           .Builder()
           .SetTestDeviceIds(new List<string>() { "76D82A808BD52B09" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadFrontAd();
    }

    void Update()
    {
        FrontAdsBtn.interactable = frontAd.IsLoaded();
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";  // 테스트
    const string frontID = "ca-app-pub-5017918845116044/7098548168";  // 실제
    InterstitialAd frontAd;


    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
        frontAd.OnAdClosed += (sender, e) =>
        {
            LogText.text = "전면광고 성공";
        };
    }

    public void ShowFrontAd()
    {
        // float result = Random.Range(0f, 1f);
        // if (result > ADS_PROBABILITY + C) return;

        probability += ADS_PROBABILITY + C;
        if (probability < 1f) return; // 3~4번마다 광고가 출력되게 함

        probability = 0f;

        C += PROBABILITY_EFF; // 광고가 나올 때마다 그 확률을 높여줍니다. (최대 40%)

        if (isBeta) return; // 베타 버전이면 광고가 나오지 않게 합니다.

        frontAd.Show();
        LoadFrontAd();
    }
    #endregion
}
