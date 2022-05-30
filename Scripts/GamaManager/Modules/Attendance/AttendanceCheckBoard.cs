using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceCheckBoard : MonoBehaviour
{
    [SerializeField] private AttendanceManager attendanceManager;
    [SerializeField] private AttendanceSlot[] slots;

    [SerializeField] private Sprite[] rewardImages;

    // 디버그용
    [SerializeField] Text todayText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitBoard(List<(eReward, int)> table)
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            slots[i].RewardImage.sprite = rewardImages[(int)table[i].Item1];
            slots[i].RewardCountText.text = table[i].Item2 + " 개";
        }
    }

    public void SetValues(int attended)
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            slots[i].SetAttended(i < attended);
        }
    }

    public void AddDays(int i)
    {
        attendanceManager.AddDays(i);
        todayText.text = attendanceManager.Today.ToString();
    }
}
