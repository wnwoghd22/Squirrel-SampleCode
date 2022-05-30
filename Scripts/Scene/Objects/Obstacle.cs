using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneObj
{
    public abstract class Obstacle : MonoBehaviour
    {
        public int Damage { get; private set; }
    }
}
