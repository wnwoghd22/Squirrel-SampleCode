using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour, IHealthUI
{
    [SerializeField] private GameObject[] hearts;

    public void HandleHealthUI(int health)
    {
        for (int i = 0; i < hearts.Length; ++i)
        {
            hearts[i].SetActive(i < health ? true : false);
        }
    }
}
