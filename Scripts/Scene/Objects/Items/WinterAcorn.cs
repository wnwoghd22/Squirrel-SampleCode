using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneObj
{
    public class WinterAcorn : Acorn
    {
        public override void SetScore(int score)
        {
            // 1부터 score 사이의 무작위 수 구하기
            int result = Random.Range(1, score + 1);

            this.Score = result;
        }
    }
}
