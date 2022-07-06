using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IGameFlowHandler : IPropertyHandler
{
    float ScrollSpeed { get; }
    Slider ProgressBar { get; }

    Inventory Inventory { get; }
    bool[] ShopItemContainer { get; }

    void ClearStage(int acorns);
    void GameOver(int acorns);
    public void Restart();
    public void Pause();
    public void Resume();
}
