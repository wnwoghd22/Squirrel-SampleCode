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
    /// ���� ����
    /// </summary>
    public string trigger;
    /// <summary>
    /// ���� �޼��� ���� Ƚ��
    /// </summary>
    public int count;

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    public int reward;
    /// <summary>
    /// ���� ���� ����
    /// </summary>
    public int rewardCount;

    /// <summary>
    /// (������) �����Ǿ����� Ȯ���ϴ� ����.
    /// </summary>
    public bool shared = false;

    public bool IsLocked => state == LOCKED;
    public bool IsNew => state == NEW;
}