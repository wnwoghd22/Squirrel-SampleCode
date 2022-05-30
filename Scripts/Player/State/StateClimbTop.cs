using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class PlayerController
{
    class StateClimbTop : IState
    {
        PlayerController player;

        /// <summary>
        /// 나무 꼭대기에 이를 때 버그를 없애기 위한 일시적인 상태
        /// </summary>
        /// <param name="player"></param>
        public StateClimbTop(PlayerController player)
        {
            this.player = player;

            player.rb.velocity = Vector2.zero;

            // player.rb.gravityScale = 1f;
            player.rb.gravityScale = 0f;

            player.animator.ResetTrigger("run");
            player.animator.ResetTrigger("climb");
            player.animator.ResetTrigger("jump");
            player.animator.ResetTrigger("stop");

            player.animator.SetTrigger("stop");

            // Vector3 pos = player.transform.position;
            // pos.y += 0.1f; // 다람쥐 높이를 약간 높여서 충돌 문제 회피...
            // player.transform.position = pos;
        }

        public IState HandleInput()
        {
#if UNITY_EDITOR
            return HandleKeyboardInput();
#elif UNITY_ANDROID
            return HandleTouch();
#endif
        }
        private IState HandleKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                return Jump(player);

            return this;
        }
        private IState HandleTouch()
        {
            if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(0))
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    // State를 쪼개어 놓으면, 굳이 if-else로 상태를 확인할 필요가 없습니다.
                    case TouchPhase.Began:
                        return Jump(player);
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        break;
                }
            }
            return this;
        }
        private IState Jump(PlayerController player)
        {
            Debug.Log("jump");

            // 위로 점프
            player.rb.velocity = Vector2.zero;
            player.rb.AddForce(Vector2.up * player.jumpEff, ForceMode2D.Impulse);

            player.se.PlaySE(player.se.SE_JUMP);

            // 점프했다면, StateJump를 반환하여 플레이어가 점프 상태가 될 수 있도록 함
            return new StateJump(player);
        }

        public IState HandleUpdate()
        {
            Debug.DrawRay(player.coll.bounds.center, Vector2.down);

            if (Physics2D.BoxCast(player.coll.bounds.center, player.coll.bounds.size, 0f, Vector2.down, 1f, LayerMask.GetMask("Tree"))) // 나무가 밑에 있다면
            {

                return new StateRun(player);
            }
            return this;
        }
    }
}
