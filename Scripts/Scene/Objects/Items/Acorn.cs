using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneObj
{
    public class Acorn : Item
    {
        public virtual void SetScore(int score)
        {
            this.Score = score;
        }
    }
}
