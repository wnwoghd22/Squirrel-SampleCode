using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : IVibrationHandler
{
    public void Vibrate()
    {
        vibrationManager.Vibrate();
    }
}