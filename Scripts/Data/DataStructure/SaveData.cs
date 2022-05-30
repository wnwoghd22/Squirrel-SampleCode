using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SaveData
{
    [SerializeField] private int lastChapterNum; // save player's last played chapter number
    public int LastChapterNum { get => lastChapterNum; set => lastChapterNum = value; }

    [SerializeField] private int lastStageNum; // save player's last played stage number
    public int LastStageNum { get => lastStageNum; set => lastStageNum = value; }

    /// <summary>
    /// 이 필드는 필요한지 잘 모르게 되어버렸군요...
    /// </summary>
    [Obsolete][SerializeField] private int acorn;  // 스테이지에서 먹은 도토리 수(재화)
    [Obsolete] public int Acorn { get { return acorn; } set { acorn = value; } }

    [SerializeField] private int myAcorn;  // 내가 가진 도토리 수(재화)
    public int MyAcorn { get { return myAcorn; } set { myAcorn = value; } }

    /// <summary>
    /// ///////////////////////////////////업적관련/////////////////////////////////////////////
    /// </summary>

    [SerializeField] private int hitSnake;  // 뱀이랑 맞은 횟수
    public int HitSnake { get { return hitSnake; } set { hitSnake = value; } }

    [SerializeField] private int hitRose;  // 장미랑 맞은 횟수
    public int HitRose { get { return hitRose; } set { hitRose = value; } }

    [SerializeField] private int hitLotus;  // 연잎이랑 맞은 횟수
    public int HitLotus { get { return hitLotus; } set { hitLotus = value; } }

    [SerializeField] private int hitChestnut;  // 밤송이랑 맞은 횟수
    public int HitChestnut { get { return hitChestnut; } set { hitChestnut = value; } }

    [SerializeField] private int hitMushroom;  // 독버섯이랑 맞은 횟수
    public int HitMushroom { get { return hitMushroom; } set { hitMushroom = value; } }

    [SerializeField] private int hitIcicle;  // 고드름이랑 맞은 횟수
    public int HitIcicle { get { return hitIcicle; } set { hitIcicle = value; } }

    [SerializeField] private int hitWinterAcorn;  // 눈덩이랑 맞은 횟수
    public int HitWinterAcorn { get { return hitWinterAcorn; } set { hitWinterAcorn = value; } }

    [SerializeField] private int fallCount;   // 낙사 횟수
    public int FallCount { get { return fallCount; } set { fallCount = value; } }
    
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    [SerializeField] private List<IntArr> clearStatus;   // 스테이지 상태를 나타내는 2차원 배열
    public List<IntArr> ClearStatus => clearStatus;

    [SerializeField] private int[] itemPerStage;   // 한 스테이지 얻은 파밍 아이템 개수(최대 3개임)
    public int[] ItemPerStage => itemPerStage;

    [SerializeField] private int[] myItemPerStage;   // 내가 스테이지 얻은 파밍 아이템 개수(최대 3개임)
    public int[] MyItemPerStage => myItemPerStage;

    [SerializeField] private int[] myitemCount;  // 내가 가진 아이템 개수
    public int[] MyItemCount => myitemCount;

    [SerializeField] private bool[] isFirstChapter;  // 해당 챕터에 처음 들어왔는지 아닌지 확인 (튜토리얼을 보여주기 위해)
    public bool[] IsFirstChapter => isFirstChapter;

    /// <summary>
    /// 위에 데이터는 크기가 변하지 않음
    /// 밑에 데이터는 크기가 변함
    /// </summary>

    [SerializeField] private List<Mail> mails;  // 메일 리스트
    public List<Mail> Mails { get { return mails; } set { mails = value; } }

    [SerializeField] private List<Achievement> achievements;  // 업적 리스트
    public List<Achievement> Achievements { get { return achievements; } set { achievements = value; } }

    /// <summary>
    /// ////////////////////////////////도토리 동굴 관련/////////////////////////////////////////
    /// </summary>
    [SerializeField] private int freeMap;
    public int FreeMap { get { return freeMap; } set { freeMap = value; } }
    [SerializeField] private int boughtMap;
    public int BoughtMap { get { return boughtMap; } set { boughtMap = value; } }

    [SerializeField] private string lastBoughtDayTime;
    public string LastBoughtDayTime { get { return lastBoughtDayTime; } set { lastBoughtDayTime = value; } }

    [SerializeField] private string lastBoughtDay;
    public string LastBoughtDay { get { return lastBoughtDay; } set { lastBoughtDay = value; } }


    /// <summary>
    /// ////////////////////////////////출석 관련/////////////////////////////////////////
    /// </summary>
    [SerializeField] private string lastCheckDay;
    public string LastCheckDay { get { return lastCheckDay; } set { lastCheckDay = value; } }
    [SerializeField] private int attendedDays;
    public int AttendedDays { get { return attendedDays; } set { attendedDays = value; } }

    /// <summary>
    /// 광고 제거 변수
    /// </summary>
    [SerializeField] private bool removeAds;
    public bool RemoveAds { get { return removeAds; } set { removeAds = value; } }

    /// <summary>
    /// 설정값 변수
    /// </summary>
    [SerializeField] private float backgroundVolume;
    public float BackgroundVolume { get { return backgroundVolume; } set { backgroundVolume = value; } }
    [SerializeField] private float soundEffectVolume;
    public float SoundEffectVolume { get { return soundEffectVolume; } set { soundEffectVolume = value; } }
    [SerializeField] private bool isOn;
    public bool IsOn { get { return isOn; } set { isOn = value; } }

    /// <summary>
    /// 처음 게임을 시작하는거라면 true, 아니면 false
    /// </summary>
    [SerializeField] private bool isFirst;
    public bool IsFirst { get { return isFirst; } set { isFirst = value; } }

    public SaveData()
    {
        Debug.Log("생성자 등장!!");
        lastChapterNum = 1;
        lastStageNum = 1;
        acorn = 0;
        myAcorn = 0;

        hitSnake = 0;
        HitRose = 0;
        hitLotus = 0;
        hitChestnut = 0;
        hitMushroom = 0;
        hitIcicle = 0;
        hitWinterAcorn = 0;
        fallCount = 0;

        clearStatus = new List<IntArr>();
        itemPerStage = new int[20]; // 4챕터 5스테이지이므로 크기는 20
        myItemPerStage = new int[20];
        myitemCount = new int[5];  // 아이템 개수는 inventory에서 5개로 해놨음
        isFirstChapter = new bool[4] { true, true, true, true};

        mails = new List<Mail>();
        achievements = new List<Achievement>();

        init();

        freeMap = 3;
        boughtMap = 0;

        lastBoughtDayTime = DateTime.Today.AddDays(-1).ToString();
        lastBoughtDay = DateTime.Today.AddDays(-1).ToString("yyyy MM dd"); // 어제 날짜 설정

        lastCheckDay = DateTime.Today.AddDays(-1).ToString();
        attendedDays = 0;

        removeAds = false;

        backgroundVolume = 1.0f;
        soundEffectVolume = 0.5f;
        isOn = true;

        isFirst = false;
    }

    /// <summary>
    /// 도토리 재화의 변화입니다. 상점에서 물건을 살때나 출석보상, 달성과제
    /// 스테이지 클리어, 페일 시 사용될 수 있습니다.
    /// 도토리의 변화가 부정당할시(예로 자신이 가진 도토리보다 비싼 아이템을 살때) false반환
    /// 도토리 재화가 변화하면 변화하고 저장
    /// </summary>
    /// <param name="n"></param>
    [Obsolete] public bool changeAcorn(int n)
    {
        if (myAcorn < -n)
        {
            Debug.Log("Not enough money :" + MyAcorn);
            return false;  // 사용할 도토리가 현재 가진 도토리보다 많으면 그냥 리턴
        }
        myAcorn += n;
        FileManager.WriteSaveFile(this);
        return true;
    }


    private void init()
    {
        initClearStatus();

        for (int i = 0; i < itemPerStage.Length; i++) itemPerStage[i] = 0;
        for (int i = 0; i < myItemPerStage.Length; i++) myItemPerStage[i] = 0;
        for (int i = 0; i < myitemCount.Length; i++) myitemCount[i] = 0;

        // UniTask.Create(async () => await CreateAchievementAsync());
    }

    private void initClearStatus()
    {
        for(int i = 0; i < 4; i++)
        {
            IntArr intArr;
            if (i == 0) intArr = new IntArr(new int[] { 1, 0, 0, 0, 0 });
            else intArr = new IntArr(new int[] { 0, 0, 0, 0, 0});
            clearStatus.Add(intArr);
        }
    }

    /// <summary>
    /// 구조 개선을 위한 메소드
    /// </summary>
    /// <returns></returns>
    public async UniTask CreateAchievementAsync()
    {
        string txt = await FileManager.GetAchievementFileForAndroid();
        achievements = DataParser.ParseAchievementData(txt);
    }

    public async UniTask UpdataAchievementAsync()
    {
        string txt = await FileManager.GetAchievementFileForAndroid();
        List<Achievement> updataAchievements = new List<Achievement>();
        updataAchievements = DataParser.ParseAchievementData(txt);

        for(int index = 0; index < achievements.Count; index++)
        {
            if(achievements[index].name.Trim() == "empty")
            {
                achievements[index] = updataAchievements[index];
            }
        }
    }

    [System.Serializable]
    public class IntArr
    {
        public int[] intArr;
        public IntArr(int[] intArr)
        {
            this.intArr = intArr;
        }
    }
}


