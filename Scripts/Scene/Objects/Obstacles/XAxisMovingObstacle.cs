using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneObj
{
    public class XAxisMovingObstacle : Obstacle
    {
        public float speed;
        private Rigidbody2D rb;
        private BoxCollider2D coll;
        private SpriteRenderer spriteRenderer;

        private bool isLanding = false;
        private bool toRight = true;
        private bool toLeft = false;
        void Start()
        {
            coll = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            /// 시작부터 오른쪽으로 움직일지 왼쪽으로 움직일지는 랜덤으로 해봤습니다. 이게 좋은지는 모르겠어요.. 뱀이 좀 더 다양하게 움직였으면 해서 넣은거거든요
            int ranNum = Random.Range(0, 2);
            if (ranNum == 0) { speed *= -1; toRight = false; toLeft = true; }
            spriteRenderer.flipX = toRight;
        }

        void Update()
        {
            if(isLanding && coll.enabled == true)
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));

                if(toRight && !Physics2D.BoxCast(coll.bounds.center + new Vector3(1, 0, 0), coll.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Branch")))
                {
                    toRight = false;
                    toLeft = true;
                    speed *= -1;
                    spriteRenderer.flipX = toRight;
                }
                else if(toLeft && !Physics2D.BoxCast(coll.bounds.center - new Vector3(1, 0, 0), coll.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Branch")))
                {
                    toLeft = false;
                    toRight = true;
                    speed *= -1;
                    spriteRenderer.flipX = toRight;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8 || collision.gameObject.layer == 6)  // 처음 생성되고 나뭇가지 또는 나무기둥과 충돌시
            {
                rb.gravityScale = 0;
                rb.velocity = Vector3.zero;
                isLanding = true;
            }

            if (isLanding)
            {
                if(collision.gameObject.layer == 6)  // 나무기둥과 충돌시
                {
                    if (toRight) { toRight = false; toLeft = true; }
                    else if (toLeft) { toRight = true; toLeft = false; }
                    spriteRenderer.flipX = toRight;
                    speed *= -1;
                }    
            }
        }
    }
}

