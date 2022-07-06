using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrationManager : MonoBehaviour
{
    [SerializeField] Toggle vibSwitchToggle;
    private bool IsOn { 
        get => dataHandler.Data.IsOn;
        set 
        {
            if (dataHandler?.Data != null)
                dataHandler.Data.IsOn = value;
            vibSwitchToggle.isOn = value;
        } 
    }

    public void SetVibration(bool value) => IsOn = value;
    //public void Setting(bool value) => vibSwitchToggle.isOn = isOn;

    IDataHandler dataHandler;

    private async void Start()
    {
        dataHandler = FindObjectOfType<GameManager>();

        await UniTask.WaitUntil(() => dataHandler?.Data != null);

        SetVibration(dataHandler.Data.IsOn);
    }

    public void Setting()
    {
        SetVibration(dataHandler.Data.IsOn);
    }

    public void Vibrate()
    {
#if UNITY_ANDROID
        if (IsOn)
            Handheld.Vibrate();
#endif
    }
}
