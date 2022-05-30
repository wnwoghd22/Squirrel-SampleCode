using System;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;
using UnityEngine.Events;
using GooglePlayGames.BasicApi.SavedGame;

public class GPGSManager : MonoBehaviour
{
    [SerializeField] UnityEvent<bool> onSignInSucceed;

    [SerializeField] Text LogText;

    IDataHandler dataHandler;
    IFileHandler fileHandler;
    DataManager dataManager;

    private ISavedGameClient SavedGame => PlayGamesPlatform.Instance.SavedGame;

    void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        dataHandler = gm;
        fileHandler = gm;
        dataManager = FindObjectOfType<DataManager>();


/*        // 구글 게임서비스 활성화(초기화)
        var config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();*/
    }

    public void LogIn()
    {
        var config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) =>
        {
            if (success) LogText.text = Social.localUser.id + " \n " + Social.localUser.userName;
            else LogText.text = "구글 로그인 실패";
            onSignInSucceed.Invoke(success);
        });
    }

    // 업적 보러 가기
    public void GoAchievementList()
    {
        LogText.text = "업적봄";
        Social.ShowAchievementsUI();
    }

    /// <summary>
    /// 불러오기
    /// </summary>
    public void LoadCloud()  // 불러오기 할때 이거만 쓰면 됩니다
    {
        SavedGame.OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, LoadGame);
    }

    void LoadGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
            SavedGame.ReadBinaryData(game, LoadData);
    }

    void LoadData(SavedGameRequestStatus status, byte[] LoadedData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string data = System.Text.Encoding.UTF8.GetString(LoadedData);
            LogText.text = data;
            if (data == "") return;
            // 로그인된 계정의 저장된 데이터를 가져와서 SetData로 데이터를 넣고
            dataManager.SetData(JsonUtility.FromJson<SaveData>(data));

            dataHandler.Acorn = dataHandler.Acorn;
            // 그거 저장하고
            fileHandler.Save();
            // 불러온다
            fileHandler.Load();
        }
        else LogText.text = "로드 실패";
    }

    /// <summary>
    /// 저장하기
    /// </summary>
    public void SaveCloud() // 저장하기 할때 이거만 쓰면 됩니다
    {
        SavedGame.OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, SaveGame);
    }

    public void SaveGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            var update = new SavedGameMetadataUpdate.Builder().Build();
            // 저장된 데이터를 로그인된 계정으로 저장
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(dataHandler.Data));
            SavedGame.CommitUpdate(game, update, bytes, SaveData);
        }
    }

    void SaveData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        { 
            LogText.text = "저장 성공";
        }
        else LogText.text = "저장 실패";
    }

    /// <summary>
    /// 삭제하기
    /// </summary>
    public void DeleteCloud() // 삭제하기 할때 이거만 쓰면 됩니다
    {
        SavedGame.OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, DeleteGame);
    }

    void DeleteGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            SavedGame.Delete(game);
            LogText.text = "삭제 성공";
        }
        else LogText.text = "삭제 실패";
    }
}
