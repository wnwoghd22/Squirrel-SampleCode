using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : IFileHandler
{
    public DataManager DataManager => dataManager;
    public SaveData Data => dataManager.SaveData;

    public int Acorn { 
        get => dataManager.Acorn;
        set
        {
            if (value < 0) return; // SaveData 안에서 이루어지던 작업을 property로 리팩터링

            dataManager.Acorn = value;
            Save();
        }
    }

    private const int MAX_CHAPTER_COUNT = 4;  // 봄 : 1, 여름 : 2, 가을 : 3, 겨울 : 4
    private const int MAX_STAGE_COUNT = 4;    // 임의로 4개 했습니다.

    public int StageNum
    {
        get => Data.LastStageNum;
        private set
        {
            if (value > MAX_STAGE_COUNT)
            {
                if (ChapterNum == MAX_CHAPTER_COUNT) // 프로퍼티를 사용한다면, 이 부분을 안에 넣어서 자동으로 처리할 수 있습니다.
                {
                    Data.LastStageNum = MAX_STAGE_COUNT;
                }
                else
                {
                    Data.LastStageNum = 0;
                    ChapterNum += 1;
                }
            }
            else if (value < 0)
            {
                Data.LastStageNum = 0;
            }
            else
            {
                Data.LastStageNum = value;
            }
        }
    }
    public int ChapterNum
    {
        get => Data.LastChapterNum;
        private set => Data.LastChapterNum = value <= 1 ? 1 : value >= MAX_CHAPTER_COUNT ? MAX_CHAPTER_COUNT : value;
    }


    public void Save() => FileManager.WriteSaveFile(Data);
    public void Load() => dataManager.SetData(FileManager.ReadSaveFile());

    public async UniTask<ObjData[]> GetObjsAsync(int chapter, int stage) => await dataManager.GetObj(chapter, stage);

    /// <summary>
    /// 광고를 제거하는 메소드.
    /// 
    /// 패키지 구매 시 호출하면 됩니다.
    /// </summary>
    public void RemoveAds()
    {
        Data.RemoveAds = true;
        Save();
        gpgsManager.SaveCloud();
    }
}