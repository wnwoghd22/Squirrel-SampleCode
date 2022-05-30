using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    IDataHandler dataHandler;

    [SerializeField] InputField cheatFieldEndingItem;

    [SerializeField] InputField cheatFieldStageState;

    void Start()
    {
        dataHandler = FindObjectOfType<GameManager>();
    }

    public void onCheatEndingItem()
    {
        if (cheatFieldEndingItem.text.Length != 20) return;

        string str = cheatFieldEndingItem.text;
        for(int i = 0; i < str.Length; i++)
        {
            dataHandler.Data.MyItemPerStage[i] = (int)str[i] - 48;
            Debug.Log((int)str[i]);
        }
    }

    public void onCheatStageState()
    {
        if (cheatFieldStageState.text.Length != 4) return;

        string str = cheatFieldStageState.text;

        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                dataHandler.Data.ClearStatus[i].intArr[j] = 2;
            }
        }

        for (int i = 0; i < str.Length; i++)
        {
            dataHandler.Data.ClearStatus[i].intArr[4] = (int)str[i] - 48;
        }
    }
}
