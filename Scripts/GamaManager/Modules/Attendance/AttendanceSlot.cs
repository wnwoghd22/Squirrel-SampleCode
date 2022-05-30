using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceSlot : MonoBehaviour
{
    [SerializeField] Image rewardImage;
    public Image RewardImage => rewardImage;
    [SerializeField] Text rewardCountText;
    public Text RewardCountText => rewardCountText;

    [SerializeField] GameObject attendSign;
    [SerializeField] int dayOffset;

    IAttendHandler attendanceHandler;
    [SerializeField] AttendanceCheckBoard board;

    private void Awake()
    {
        attendanceHandler = FindObjectOfType<AttendanceManager>();
    }

    public void SetAttended(bool attend)
    {
        attendSign.SetActive(attend);
    }

    public void Attend()
    {
        if (attendanceHandler.Attend(dayOffset))
        {
            // Ãâ¼® ÆË¾÷ ¶ç¿ì±â?

            SetAttended(true);
        }
    }
}
