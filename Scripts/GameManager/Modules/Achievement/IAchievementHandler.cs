using System.Collections.Generic;

public interface IAchievementHandler : IAchievementListener
{
    List<Achievement> Achievements { get; }
    int New { get; }

    void SetAchievementNew(int i);
    void SetAchievementOld(int i);
}
