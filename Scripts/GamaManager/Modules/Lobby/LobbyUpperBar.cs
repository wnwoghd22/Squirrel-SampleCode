using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUpperBar : MonoBehaviour
{
    [SerializeField] private Text date;
    [SerializeField] private Text acornText;
    [SerializeField] private GameObject addAcornTab;


    public void SetDateToday()
    {
        DateTime time = DateTime.Now;

        date.text = time.ToString("yyyy-MM-dd");
    }
    [Obsolete]
    public void SetAcornText(int i)
    {
        acornText.text = i.ToString();
    }
    public void ActivateAddAcornTab(bool active)
    {
        addAcornTab.SetActive(active);
    }
}
