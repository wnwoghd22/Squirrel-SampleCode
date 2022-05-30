using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataParser
{
    /// <summary>
    /// 단순히 parse만 해주는 메소드로 했습니다.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ObjData[] ParseObjData(string data)
    {
        List<ObjData> objList = new List<ObjData>();

        string[] sList = data.Split('\n');
        string[] header = sList[0].Split(',');
        string[] dataType = sList[1].Split(',');

        header[header.Length - 1] = header[header.Length - 1].Trim();
        dataType[dataType.Length - 1] = dataType[dataType.Length - 1].Trim();

        for (int i = 2; i < sList.Length; i++)
        {
            string[] row = sList[i].Split(',');

            // csv파일을 고치면 모르겠지만 줄이 한칸 더 생겨요...
            // 그래서 제일 첫번째 칼럼(Type)은 score처럼 없을 일이 없으니까
            // Type을 확인해서 이 부분이 없는 값이면 알 수 없는 이유로 생긴 한 줄이기에
            // 이때 break를 해주는 식으로 응급처치 했습니다...
            if (row[0] == "") break;

            row[row.Length - 1] = row[row.Length - 1].Trim();

            ObjData obj = new ObjData();

            obj.setDataType(header, dataType, row);

            objList.Add(obj);
        }

        return objList.ToArray();
    }

    public static List<Achievement> ParseAchievementData(string data)
    {
        List<Achievement> list = new List<Achievement>();

        string[] sList = data.Split('\n');

        int n;
        for (int i = 1; i < sList.Length; i++)
        {
            string[] tokens = sList[i].Split(',');

            if (tokens[0] == "") break;   // 여기도 파일 고치면 한 줄이 더 생기기에...

            Achievement achievement = new Achievement();

            int tokenIndex = 0;

            achievement.index = int.TryParse(tokens[tokenIndex++], out n) ? n : -1;
            achievement.state = int.TryParse(tokens[tokenIndex++], out n) ? n : -1;
            achievement.trigger = tokens[tokenIndex++].Replace("\\n", "\n");
            achievement.count = int.TryParse(tokens[tokenIndex++], out n) ? n : -1;
            achievement.reward = int.TryParse(tokens[tokenIndex++], out n) ? n : -1;
            achievement.rewardCount = int.TryParse(tokens[tokenIndex++], out n) ? n : -1;
            achievement.name = tokens[tokenIndex++].Replace("\\n", "\n");
            /*achievement.condition = tokens[tokenIndex++].Replace("\\n", "\n");
            achievement.detail = tokens[tokenIndex++].Replace("\\n", "\n");
            achievement.hint = tokens[tokenIndex++].Replace("\\n", "\n");*/

            list.Add(achievement);
        }

        return list;
    }

    public static List<(eReward, int)> ParseAttendanceTable(string data)
    {
        List<(eReward, int)> result = new List<(eReward, int)>();

        string[] sList = data.Split('\n');

        int n;

        foreach (string s in sList)
        {
            Debug.Log(s);

            string[] tokens = s.Split(','); 
            if (tokens[0] == "") break;

            int item1 = int.TryParse(tokens[0], out n) ? n : 0;
            int item2 = int.TryParse(tokens[1], out n) ? n : 0;

            result.Add(((eReward)item1, item2));
        }

        return result;
    }
}
