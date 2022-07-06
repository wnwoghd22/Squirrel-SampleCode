using System;

public enum eReward
{ 
    ACORN,
    BUBBLE,
    BALLOON,
    MAGNET,
    INITSTAGE,
}

[System.Serializable]
public class Mail : IComparable<Mail>
{
    /// <summary>
    /// 메일함 슬롯에 띄울 문자열
    /// </summary>
    public string sentence;

    /// <summary>
    /// 보상 종류. 정책 상 도토리만 준다면 굳이 필요없는 필드
    /// </summary>
    public eReward reward;

    /// <summary>
    /// 보상의 양.
    /// </summary>
    public int rewardCount;

    /// <summary>
    /// 메일이 날아드는 시점
    /// </summary>
    public DateTime time;

    /// <summary>
    /// 보상을 받았는지 유무.
    /// </summary>
    public bool available;

    public Mail(string sentence, eReward reward, int rewardCount)
    {
        // 메일의 내용 할당
        this.sentence = sentence;
        this.reward = reward;
        this.rewardCount = rewardCount;

        this.time = DateTime.Now; // 메일이 날아드는 시점을 자동으로 할당.
        this.available = true;
    }
    public int CompareTo(Mail other) => time.CompareTo(other.time);
}
