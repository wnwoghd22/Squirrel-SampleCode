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
    /// <param name="trigger">���� �������� ���� Ȯ�� �޼ҵ尡 �Ǳ� ���� �з����� �̸��� trigger�� �Ͽ����ϴ�.</param>
    /// <param name="count"></param>
    public void CheckAchievement(string trigger, int count)
    {
        Achievement achievement;
        if ((achievement = GetAchievement(trigger, count)) == null) return;  // ���� ��ȸ�� �Ұ����ϴٸ� ����
        
        SetAchievementNew(achievement.index); // ���� ���¸� New�� ����, Show ��ư ���ÿ� Ȱ��ȭ

        // ��������� �޼������� ������� setAchievementNew
        if (CheckHiddenAchievement()) SetAchievementNew(47);

        /// �������� �� Ŭ���� ������ ��ܿ� Ŭ�����ߴٴ� ������ 3�ʰ� Ȱ��ȭ
        UniTask.Create(async () => await ShowAchievementClearTabAsync(3f));
    }
    private Achievement GetAchievement(string trigger, int count)
    {
        foreach (Achievement a in Achievements)
        {
            if (!a.IsLocked) continue; // �رݵ� �����̶�� ���� ���� ��ȸ
            Debug.Log(a.trigger + ", " + trigger);
            if (!a.trigger.Equals(trigger)) continue;  // Ʈ���Ű� ��ġ���� ������ ���� ���� ��ȸ
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

