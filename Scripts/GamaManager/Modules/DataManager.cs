using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    // 로딩 횟수를 줄이기 위한 저장 필드
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
    /// 게임메니저에서 getObj함수를 사용해서 바로 obj배열을 가져오는 방식
    /// </summary>
    /// <param name="chapter"></param>
    /// <param name="stage"></param>
    /// <returns></returns>
    public async UniTask<ObjData[]> GetObj(int chapter, int stage)
    {
        // 이미 불러왔던 스테이지가 아니라면
        if (this.chapter != chapter || this.stage != stage)
        {
            this.chapter = chapter; this.stage = stage;

            string txt = await FileManager.GetMapFileForAndroid(chapter, stage);
            objData = DataParser.ParseObjData(txt);
        }

        return objData;
    }
}
