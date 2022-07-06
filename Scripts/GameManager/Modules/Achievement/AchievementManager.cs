using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AchievementManager : MonoBehaviour, IAchievementHandler
{
    [SerializeField] GameObject achievementClearTab;
    [SerializeField] GameObject newAchievementPrompt;

    IDataHandler dataHandler;
    IFileHandler fileHandler;

    public List<Achievement> Achievements => dataHandler.Data.Achievements;
    public int New
    {
        get
        {
            int result = -1;
            foreach (Achievement a in Achievements)
            {
                if (a.name.Equals("empty")) continue;
                if (a.IsNew) return a.index;
            }
            return result;
        }
    }

    private void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        dataHandler = gm;
        fileHandler = gm;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trigger">보다 범용적인 업적 확인 메소드가 되기 위해 패러미터 이름은 trigger로 하였습니다.</param>
    /// <param name="count"></param>
    public void CheckAchievement(string trigger, int count)
    {
        Achievement achievement;
        if ((achievement = GetAchievement(trigger, count)) == null) return;  // 업적 조회가 불가능하다면 리턴
        
        SetAchievementNew(achievement.index); // 업적 상태를 New로 설정, Show 버튼 동시에 활성화

        // 히든업적을 달성했으면 히든업적 setAchievementNew
        if (CheckHiddenAchievement()) SetAchievementNew(47);

        /// 스테이지 중 클리어 했으면 상단에 클리어했다는 문구가 3초간 활성화
        UniTask.Create(async () => await ShowAchievementClearTabAsync(3f));
    }
    private Achievement GetAchievement(string trigger, int count)
    {
        foreach (Achievement a in Achievements)
        {
            if (!a.IsLocked) continue; // 해금된 업적이라면 다음 업적 조회
            Debug.Log(a.trigger + ", " + trigger);
            if (!a.trigger.Equals(trigger)) continue;  // 트리거가 일치하지 않으면 다음 업적 조회
            if (count >= a.count)
                return a; 
        }
        return null;
    }
    private async UniTask ShowAchievementClearTabAsync(float seconds)
    {
        achievementClearTab.SetActive(true);

        await UniTask.Delay(TimeSpan.FromSeconds(seconds));

        achievementClearTab.SetActive(false);
    }

    public void SetAchievementNew(int i)
    {
        Achievements[i].state = Achievement.NEW;

        newAchievementPrompt.SetActive(true);

        //fileHandler.Save();
    }
    public void SetAchievementOld(int i)
    {
        Achievements[i].state = Achievement.OLD;

        newAchievementPrompt.SetActive(false);
        foreach(Achievement a in Achievements)
        {
            if (a.IsNew)
            {
                newAchievementPrompt.SetActive(true);
            }
        }

        //fileHandler.Save();
    }

    private bool CheckHiddenAchievement()
    {
        bool isClearHiddenAchievement = true;
        foreach(Achievement a in Achievements)
        {
            if (a.name.Trim() == "empty" || a.index == 47) continue;
            if (a.state == Achievement.LOCKED) isClearHiddenAchievement = false;
        }
        return isClearHiddenAchievement;
    }
}

