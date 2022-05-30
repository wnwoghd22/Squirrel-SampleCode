using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneObj
{
    public class FarmingItem : Item
    {
        GameManager gm;

        public void SetScore(int score)
        {
            this.Score = score;
        }

        private void Start()
        {
            /// 다깬 스테이지에서는 엔딩을 위한 파밍아이템을 없애기로 해서 구현했는데
            /// 데이터의 clearstatus로 해당 스테이지를 깼는지 안 깼는지 확인하기 위해서 
            /// find함수로 gm을 가져왔습니다. 스테이지당 엔딩형 아이템이 3개씩 있으므로 
            /// 한 스테이지당 시간이 오래걸리는 find함수를 3번 사용한다는 것이 맘에 걸리지만...
            /// 일단 이렇게 구현하고 나중에 고쳐보겠습니다...
            /// 일단은 원활한 레벨디자인을 위해 빼겠습니다.
            gm = FindObjectOfType<GameManager>();

            /// 스테이지 클리어시 엔딩아이템 없음
            if (gm.Data.ClearStatus[gm.ChapterNum - 1].intArr[gm.StageNum] == 2)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
