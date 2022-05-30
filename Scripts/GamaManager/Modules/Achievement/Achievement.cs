[System.Serializable]
public class Achievement
{
    public const int LOCKED = 0;
    public const int OLD = 1;
    public const int NEW = 2;

    public int index;
    public int state;
    public string name;
    /*public string condition;
    public string detail;
    public string hint;*/

    /// <summary>
    /// 업적 조건
    /// </summary>
    public string trigger;
    /// <summary>
    /// 업적 달성을 위한 횟수
    /// </summary>
    public int count;

    /// <summary>
    /// 업적 보상 종류
    /// </summary>
    public int reward;
    /// <summary>
    /// 업적 보상 개수
    /// </summary>
    public int rewardCount;

    /// <summary>
    /// (엔딩만) 공유되었는지 확인하는 변수.
    /// </summary>
    public bool shared = false;

    public bool IsLocked => state == LOCKED;
    public bool IsNew => state == NEW;
}