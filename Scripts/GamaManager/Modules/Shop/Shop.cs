using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private int[] price;

    [SerializeField] private GameObject shopTab;
    [SerializeField] private GameObject background;

    [SerializeField] private Image[] categoryButtonImages;
    [SerializeField] private Sprite selectedButton;
    [SerializeField] private Sprite unselectedButton;
    [SerializeField] private GameObject[] pages;

    [SerializeField] private GameObject setCountPopUp;
    [SerializeField] private GameObject exceedPopUp;
    [SerializeField] private WarningPopUp warningPopUp;   // 나중에 없앨 코드(구현이 안됨을 보여주는 팝업창)

    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemCountText;
    [SerializeField] private Text totalPrice;

    [SerializeField] private Text[] countTexts;
    [SerializeField] private Text[] priceTexts;

    [SerializeField] private Text acornCount;

    [SerializeField] private Sprite[] itemSprites;

    // 다람쥐 동굴 관련 변수
    private int freeMap { get { return dataHandler.Data.FreeMap; } set { dataHandler.Data.FreeMap = value; } }
    private int boughtMap { get { return dataHandler.Data.BoughtMap; } set { dataHandler.Data.BoughtMap = value; } }


    private int mapPrice = 50;
    private int maxBoughtCount = 3;   // 도토리로 다람쥐동굴을 살 수 있는 최대 개수

    //private int[] mapPrice = { 50, 70, 100, int.MaxValue };
    //private int MapPrice => mapPrice[boughtMap];

    [SerializeField] private Text mapPriceText;
    [SerializeField] private Text coolDownTimeText;
    [SerializeField] private GameObject buyMapButtonAcornSprite;
    [SerializeField] private GameObject coolDownTimeField;

    private TimeSpan CoolDownTime => TimeSpan.FromHours(3) - (DateTime.Now - DateTime.Parse(dataHandler.Data.LastBoughtDayTime));
    //private TimeSpan CoolDownTime => TimeSpan.FromSeconds(3) - (DateTime.Now - DateTime.Parse(dataHandler.Data.LastBoughtDayTime));  // 테스트용 쿨타임 3초

    [SerializeField] private GameObject buyMapPopUp;
    [SerializeField] private Text buyMapPopUpPriceText;
    [SerializeField] private GameObject buyMapResult;
    [SerializeField] private Text buyMapResultText;

    IShopItemHandler inventory;
    IDataHandler dataHandler;
    IMailHandler mailHandler;
    ISEHandler se;
    IFileHandler fileHandler;

    private int itemIndex = 0;
    public int ItemIndex {
        get => itemIndex;
        set
        {
            itemIndex = value;
            itemImage.sprite = itemSprites[itemIndex]; // UI 변경
        }
    }
    private int itemCount = 1;

    // if (price[itemIndex] * value <= dataHandler.Data.Acorn && value >= 1) 
    //    itemCount = value;
    public int ItemCount {
        get => itemCount;
        set
        {
            itemCount = 1 < value ? value : 1;
            se.PlaySE(se.SE_CLICK);
            itemCountText.text = itemCount.ToString(); // UI 변경
            totalPrice.text = (price[ItemIndex] * itemCount).ToString() + "개";
        }
    }

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        dataHandler = FindObjectOfType<GameManager>();
        se = FindObjectOfType<GameManager>();
        mailHandler = FindObjectOfType<MailManager>();
        // lobbyUpperBar = FindObjectOfType<LobbyUpperBar>();
        fileHandler = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        SetMapButtonFields();
    }

    public void ShowShopTab()
    {
        SetValues();
        background.SetActive(true);
    }
    public void ShowShopTab(int i)
    {
        ShowShopTab();

        OpenPage(i);
    }

    private void SetValues()
    {
        acornCount.text = dataHandler.Acorn.ToString();
        Debug.Log(TimeSpan.FromHours(3)); 
        for (int i = 0; i < countTexts.Length; ++i)
        {
            countTexts[i].text = $"x {dataHandler.Data.MyItemCount[i]}";
        }
        for (int i = 0; i < priceTexts.Length; ++i)
        {
            priceTexts[i].text = $"{price[i]}";
        }

        if (dataHandler.Data.LastBoughtDay != DateTime.Today.ToString("yyyy MM dd")) // 날이 넘어갔다면
        {
            // 다람쥐동굴 관련 데이터 초기화
            freeMap = 3;
            boughtMap = 0;
            dataHandler.Data.LastBoughtDayTime = DateTime.Today.AddDays(-1).ToString();
            dataHandler.Data.LastBoughtDay = DateTime.Today.AddDays(-1).ToString("yyyy MM dd"); // 어제 날짜 설정
        }

        if (freeMap > 0) // 무료 지도를 얻을 수 있다면
        {
            mapPriceText.text = 0.ToString();
        }
        // 오늘치 지도 다 샀을 때
        else if(boughtMap >= maxBoughtCount)
        {
            mapPriceText.text = "<size=35>내일까지 기다려주세요!</size>";
            buyMapButtonAcornSprite.gameObject.SetActive(boughtMap >= maxBoughtCount);
            //fileHandler.Save();
            return;
        }
        // 도토리로 지도를 살 때(50원 고정)
        else
        {
            mapPriceText.text = mapPrice.ToString();
        }

        SetMapButtonFields();

        //fileHandler.Save();
    }

    public void SetItemIndex(int index)
    {
        ItemIndex = index;
        ItemCount = 1;
        setCountPopUp.SetActive(true);
    }

    public void SetItemCount(int count)
    {
        ItemCount = count;
    }
    public void IncreaseCount()
    {
        ItemCount++;
    }
    public void DecreaseCount()
    {
        ItemCount--;
    }
    public void BuyItems()
    {
        int total = price[itemIndex] * itemCount;

        setCountPopUp.SetActive(false);

        if (dataHandler.Acorn >= total)
        {
            dataHandler.Acorn -= total;

            se.PlaySE(se.SE_JINGLE);

            Debug.Log("Buy Item : " + itemIndex + ", " + itemCount);
            inventory.AddItem(itemIndex, itemCount);

            SetValues();
        }
        else
        {
            Debug.Log("Not enough money :" + dataHandler.Data.MyAcorn);
            exceedPopUp.SetActive(true);

            return;
        }
    }

    public void BuyPackages(int i)
    {
        se.PlaySE(se.SE_JINGLE);

        // mailHandler.Send(new Mail("테스트 패키지", eReward.ACORN, 100));

        switch(i)
        {
            case 0:
                // Ads 제거

                mailHandler.Send(new Mail("파랑새 패키지", eReward.ACORN, 50));
                break;
            case 1:
                // Ads 제거

                mailHandler.Send(new Mail("다람이 패키지", eReward.BALLOON, 1));
                mailHandler.Send(new Mail("다람이 패키지", eReward.BUBBLE, 1));
                mailHandler.Send(new Mail("다람이 패키지", eReward.MAGNET, 1));
                mailHandler.Send(new Mail("다람이 패키지", eReward.ACORN, 150));
                break;
            case 2:
                // Ads 제거

                mailHandler.Send(new Mail("다람양 패키지", eReward.INITSTAGE, 3));
                mailHandler.Send(new Mail("다람양 패키지", eReward.ACORN, 300));
                break;
            case 3:
                mailHandler.Send(new Mail("도토리 주머니 (소)", eReward.ACORN, 100));
                break;
            case 4:
                mailHandler.Send(new Mail("도토리 주머니 (중)", eReward.ACORN, 300));
                break;
            case 5:
                mailHandler.Send(new Mail("도토리 주머니 (대)", eReward.ACORN, 700));
                break;
        }
    }

    public void OpenPage(int i)
    {
        se.PlaySE(se.SE_CLICK);

        foreach (GameObject o in pages)
        {
            o.SetActive(false);
        }
        foreach (Image image in categoryButtonImages)
        {
            image.sprite = unselectedButton;
        }
        categoryButtonImages[i].sprite = selectedButton;

        pages[i].SetActive(true);
    }

    [Obsolete]
    public void goPakageShop()
    {
        warningPopUp.gameObject.SetActive(true);
        warningPopUp.setText("다음을 기대해주세요!!");
    }

    [Obsolete]
    public void goCave()
    {
        warningPopUp.gameObject.SetActive(true);
        warningPopUp.setText("다음을 기대해주세요!!");
    }

    public void OpenBuyMapPopUp()
    {
        se.PlaySE(se.SE_CLICK);

        if (CoolDownTime > TimeSpan.Zero)
            return;

        /// 공짜
        if (freeMap > 0)
        {
            buyMapPopUp.SetActive(true);
            buyMapPopUpPriceText.text = "0개";
        }
        else if(boughtMap >= maxBoughtCount)
        {
            // 하루 살 수 있는 지도를 다 쓴 상태 기다리라는 팝업을 띄울 수도 있음
            return;
        }
        /// 50 도토리
        else if(boughtMap >= 0)
        {
            buyMapPopUp.SetActive(true);
            buyMapPopUpPriceText.text = mapPrice.ToString() + "개";
        }
    }

    private void SetMapButtonFields()
    {
        if (background.gameObject.activeSelf)
        {
            coolDownTimeText.text = CoolDownTime.ToString(@"hh\:mm\:ss");
            // 3시간이 경과했다면 지도 구매 버튼을, 3시간이 경과하지 않았다면 쿨타임을 보여줍니다
            coolDownTimeField.SetActive(CoolDownTime > TimeSpan.Zero);
            // 도토리 그림은 쿨타임 중이 아니거나 3번 구매를 하지 않았을 때를 빼고 보여줌
            buyMapButtonAcornSprite.SetActive(!(CoolDownTime > TimeSpan.Zero) && boughtMap < maxBoughtCount);
            mapPriceText.gameObject.SetActive(!(CoolDownTime > TimeSpan.Zero));
        }
    }

    public void BuyMap()
    {
        if (freeMap > 0)
        {
            dataHandler.Data.LastBoughtDayTime = DateTime.Now.ToString();       // 이 줄의 코드가 정상 작동
            //dataHandler.Data.LastBoughtDayTime = DateTime.Today.ToString();   // 이 줄의 코드는 쿨타임없이 살 수 있어요(테스트용)

            dataHandler.Data.LastBoughtDay = DateTime.Today.ToString("yyyy MM dd"); // 어제 날짜 설정

            --freeMap;

            se.PlaySE(se.SE_JINGLE);

            Rullet();
            SetValues();
        }
        else if (dataHandler.Acorn >= mapPrice)
        {
            boughtMap++;

            /// 마지막 도토리로 살 때는 사고 난 후 바로 오늘 하루 다 샀다는 정보를 보여주기 위해 시간 설정 X
            if (boughtMap < maxBoughtCount)
            {
                dataHandler.Data.LastBoughtDayTime = DateTime.Now.ToString();
                dataHandler.Data.LastBoughtDay = DateTime.Today.ToString("yyyy MM dd"); // 어제 날짜 설정
            }

            dataHandler.Acorn -= mapPrice;
            
            se.PlaySE(se.SE_JINGLE);
            Rullet();
            SetValues();
        }
        /// 가지고 있는 도토리가 가격보다 비싸면 도토리가 부족하다는 팝업창 출력
        else if(dataHandler.Acorn < mapPrice)
        {
            se.PlaySE(se.SE_CLICK);

            exceedPopUp.SetActive(true);
            buyMapPopUp.SetActive(false);
        }
    }

    private void Rullet()
    {
        for(int i = 0; i < 3; i++)
        {
            int result = UnityEngine.Random.Range(0, 100); // 0부터 99까지 중 무작위 반환
            if (result < 5) // 5% 확률로
            {
                mailHandler.Send(new Mail("비밀동굴", eReward.INITSTAGE, 1));
                buyMapResultText.text = "빈 병 아이템 1개 획득!";
            }
            else
            {
                result = UnityEngine.Random.Range(0, 3); // 0부터 2까지 중 무작위 반환
                switch (result)
                {
                    case Inventory.BUBBLE_SHIELD:
                        mailHandler.Send(new Mail("비밀동굴", eReward.BUBBLE, 1));
                        buyMapResultText.text = "비눗방울 1개 획득!";
                        break;
                    case Inventory.BALLOON:
                        mailHandler.Send(new Mail("비밀동굴", eReward.BALLOON, 1));
                        buyMapResultText.text = "풍선 1개 획득!";
                        break;
                    case Inventory.MAGNET:
                        mailHandler.Send(new Mail("비밀동굴", eReward.MAGNET, 1));
                        buyMapResultText.text = "잠자리채 1개 획득!";
                        break;
                }
            }
        }

        buyMapPopUp.SetActive(false);
        buyMapResult.SetActive(true);

        return;
    }
}
