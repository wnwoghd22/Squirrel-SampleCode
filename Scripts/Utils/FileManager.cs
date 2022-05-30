using UnityEngine;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

/// <summary>
/// 이 클래스 또한 Start, Update가 불필요합니다.
/// Mono의 내장 메소드 또한 호출하지 않습니다.
/// Singleton 패턴으로도 충분히 해결이 가능할 정도로
/// 각 메소드의 호출 빈도가 낮고, 호출하려는 오브젝트도 하나 뿐이기 때문에
/// 전부 static field로 뒀습니다. 구조를 바꾸고 싶으시면 바꿔도 상관없습니다.
/// 
/// 원래 FileManager 클래스는 try - catch로 떡칠 돼있는게 국룰인데...
/// 잘 돌아가면 문제 없는 거겠죠?
/// </summary>
public class FileManager
{
	/// <summary>
	/// 비동기로 StreamingAssets에 접근하기 위한 함수.
	/// </summary>
	/// <param name="chapter">봄, 여름, 가을, 겨울에 맞춰 1, 2, 3, 4</param>
	/// <param name="stage">스테이지 숫자</param>
	/// <returns></returns>
	public static async UniTask<string> GetMapFileForAndroid(int chapter, int stage)
    {
		string filePath = Application.streamingAssetsPath + "/" + chapter + "-" + stage + ".csv";
		Debug.Log(filePath);

		var txt = (await UnityWebRequest.Get(filePath).SendWebRequest()).downloadHandler.text;
		
		return txt;
	}

    public static async UniTask<string> GetAchievementFileForAndroid()
    {
        string filePath = Application.streamingAssetsPath + "/achievement.txt";
        Debug.Log("achievement csv파일 위치 : " + filePath);

        string txt = (await UnityWebRequest.Get(filePath).SendWebRequest()).downloadHandler.text;
        return txt;
    }

    public static async UniTask<string> GetCreditFileForAndroid()
    {
        string filePath = Application.streamingAssetsPath + "/credit.txt";
        Debug.Log("credit csv파일 위치 : " + filePath);

        string txt = (await UnityWebRequest.Get(filePath).SendWebRequest()).downloadHandler.text;
        return txt;
    }

    public static async UniTask<string> GetAttendanceFileForAndroid()
    {
        string filePath = Application.streamingAssetsPath + "/attendance.csv";
        Debug.Log("credit csv파일 위치 : " + filePath);

        string txt = (await UnityWebRequest.Get(filePath).SendWebRequest()).downloadHandler.text;
        return txt;
    }

    /// <summary>
    /// 저장된 파일을 읽어오는 메소드.
    /// 멤버변수 gm의 메소드를 호출하는 대신, 저장된 클래스를 바로 반환하도록.
    /// 클래스 구조를 직관적으로 만드는게 좋겠습니다.
    /// </summary>
    /// <returns>class SaveData</returns>
    public static SaveData ReadSaveFile()
    {
        string filePath = Application.persistentDataPath + "/save.json";
        Debug.Log(filePath);

        if (!File.Exists(filePath))
        {
            Debug.Log("No file!");
            return null;
        }

        StreamReader saveFile = new StreamReader(filePath);

        //SaveData data = JsonUtility.FromJson<SaveData>(saveFile.ReadToEnd());
        SaveData data = new SaveData();
        JsonUtility.FromJsonOverwrite(saveFile.ReadToEnd(), data);

        if (data != null)
        {
            Debug.Log(data.LastChapterNum + " " + data.LastStageNum);
        }
        saveFile.Close();
        return data;
    }

    /// <summary>
    /// 인수를 받으면 저장하도록 바꾼 메소드.
    /// </summary>
    /// <param name="data">저장이 필요한 변수 집합 class SaveData</param>
    public static void WriteSaveFile(SaveData data)
    {
        string filePath = Application.persistentDataPath + "/save.json";

        StreamWriter saveFile = new StreamWriter(filePath);

        saveFile.Write(JsonUtility.ToJson(data, true));
        saveFile.Close();
    }

    public static string WriteTextureFile(Texture2D texture, string name)
    {
        string filePath = Path.Combine(Application.temporaryCachePath, name);
        File.WriteAllBytes(filePath, texture.EncodeToPNG());

        return filePath;
    }
}
