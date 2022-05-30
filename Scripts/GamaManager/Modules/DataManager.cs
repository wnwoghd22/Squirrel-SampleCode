using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    // �ε� Ƚ���� ���̱� ���� ���� �ʵ�
    private ObjData[] objData;
    private int chapter = -1; 
    private int stage = -1;

    private SaveData saveData;
    public SaveData SaveData => saveData;
    public void SetData(SaveData data) => saveData = data;
    public async UniTask ConstructNewSaveData()
    {
        saveData = new SaveData();
        await saveData.CreateAchievementAsync();
    }

    [SerializeField] Text[] acornTexts;
    public int Acorn {
        get => saveData.MyAcorn;
        set
        {
            saveData.MyAcorn = value;
            foreach (Text text in acornTexts) text.text = value.ToString();
        }
    }

    private void Start()
    {

    }

    /// <summary>
    /// ���Ӹ޴������� getObj�Լ��� ����ؼ� �ٷ� obj�迭�� �������� ���
    /// </summary>
    /// <param name="chapter"></param>
    /// <param name="stage"></param>
    /// <returns></returns>
    public async UniTask<ObjData[]> GetObj(int chapter, int stage)
    {
        // �̹� �ҷ��Դ� ���������� �ƴ϶��
        if (this.chapter != chapter || this.stage != stage)
        {
            this.chapter = chapter; this.stage = stage;

            string txt = await FileManager.GetMapFileForAndroid(chapter, stage);
            objData = DataParser.ParseObjData(txt);
        }

        return objData;
    }
}
