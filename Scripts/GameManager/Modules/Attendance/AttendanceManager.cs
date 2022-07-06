using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceManager : MonoBehaviour, IAttendHandler
{
    [SerializeField] private AttendanceCheckBoard checkBoard;

    private IFileHandler fileHandler;
    private IMailHandler mailHandler;
    ISEHandler se;

    public DateTime LastCheckDay // 최종 출석 시점.
    {
        get => DateTime.Parse(fileHandler.Data.LastCheckDay);
        private set => fileHandler.Data.LastCheckDay = value.ToString();
    }
    
    private DateTime today;
    public DateTime Today => today;

    public int AttendedDays // 출석했던 날 수
    {
        get => fileHandler.Data.AttendedDays;
        private set => fileHandler.Data.AttendedDays = value;
    }
    public const int MAX_ATTEND_DAYS = 14; // 출석체크가 가능한 최대 일 수
    private bool attended; // 오늘 출석했는가?

    private List<(eReward, int)> rewardTable;

    // Start is called before the first frame update
    async void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        fileHandler = gm;
        se = gm;
        mailHandler = FindObjectOfType<MailManager>();

        rewardTable = DataParser.ParseAttendanceTable(await FileManager.GetAttendanceFileForAndroid());
        checkBoard.InitBoard(rewardTable);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenAttendanceCheckBoard()
    {
        se.PlaySE(se.SE_CLICK);
        // 오늘 날짜 할당
        today = DateTime.Today;

        UpdateAttendance();

        checkBoard.gameObject.SetActive(true);
        checkBoard.SetValues(AttendedDays);
    }

    /// <summary>
    /// 실제 출석이 이루어지기 위해 호출되는 메소드.
    /// </summary>
    /// <param name="day">최초 출석일로부터 며칠 떨어져있는지 나타내는 변수</param>
    /// <returns>출석이 유효한 경우(이전 출석일 바로 다음날이라면) true 반환</returns>
    public bool Attend(int day)
    {
        // 이미 출석했거나 현재 날짜가 아닌 버튼을 클릭한 경우 빠져나가기
        if (attended || day != AttendedDays) return false;
        se.PlaySE(se.SE_CLICK);
        LastCheckDay = today; // 최종 출석일을 오늘로 바꾸기

        attended = true;

        SendReward(++AttendedDays);

        //fileHandler.Save();

        return true;
    }

    private void UpdateAttendance()
    {
        // 날짜 체크로 유효성 검증... 로직을 어떻게 구성해야 할까요?
        attended = today == LastCheckDay;

        // 최종 출석일 경과 후 최대 출석일을 넘기는 경우
        if (today > LastCheckDay && AttendedDays == MAX_ATTEND_DAYS)
        {
            AttendedDays = 0;
        }

        if ((today - LastCheckDay).Days > 1)
        {
            // 매일 연속 출석 정책이라면 이곳에서 초기화 필요
        }
    }

    private void SendReward(int i) => mailHandler.Send(new Mail(i + "일차 출석보상", rewardTable[i - 1].Item1, rewardTable[i - 1].Item2));

    /// <summary>
    /// 출석확인 실행 확인용 메소드
    /// </summary>
    /// <param name="i"></param>
    public void AddDays(int i)
    {
        today = today.AddDays(i);

        UpdateAttendance();
    }
}
