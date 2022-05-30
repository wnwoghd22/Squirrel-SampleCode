using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneObj
{
    public class TempObstacle : Obstacle
    {
        private Rigidbody2D rb;
        private BoxCollider2D coll;

        void Start()
        {
            coll = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8 || collision.gameObject.layer == 6)  // 처음 생성되고 나뭇가지와 충돌시
            {
                rb.gravityScale = 0;
                rb.velocity = Vector3.zero;
            }
        }
    }
}