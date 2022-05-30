using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class PlayerController
{
    class StateJump : IState
    {
        PlayerController player;

        /**
         * 땅을 밟고 있거나 나무를 오르고 있을 때는 더블 점프를 확인할 필요가 없다!
         * true일 때 더블점프를 한 것으로 바꿀까요? 확인 바람.
         */
        bool doubleJump;

        /**
         * 떨어지는지 확인하기 위한 변수.
         */
        bool isFalling;

        /// <summary>
        /// 플레이어가 공중에 있는 상태일 때.
        /// </summary>
        /// <param name="player"></param>
        public StateJump(PlayerController player)
        {
            this.player = player;

            player.rb.gravityScale = 1f;

            player.animator.ResetTrigger("run");
            player.animator.ResetTrigger("climb");
            player.animator.ResetTrigger("jump");
            player.animator.ResetTrigger("stop");

            player.animator.SetTrigger("jump");

            doubleJump = false; // 플레이어는 매번 착지 시마다 doubleJump 변수 초기화에 신경 쓸 필요 없음
            isFalling = false;
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
            {
                // 더블 점프
                if (!doubleJump)
                    DoubleJump();
            }
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
                        // 더블 점프
                        if (!doubleJump)
                            DoubleJump();
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        break;
                }
            }
            return this;
        }

        public IState HandleUpdate()
        {
            if (isFalling && player.isGrounded())
            {
                //착지 사운드 및 애니메이션 삽입 위치

                  return new StateRun(player);
            }

            if (player.AttachedTree)
            { 
                return new StateClimb(player);
            }

            /**
             * 이 코드에서 치명적인 문제를 발견했다고 할까요...?
             * 
             * StateFall에서 바로 StateJump로 전이하는 frame에
             * 즉시 착지한다면, StateJump에서 StateRun으로 전이할 수 없게 됩니다.
             * 
             * 만일 이 부분이 슬라이딩 버그를 일으키는 궁극적인 원인이라면...
             * 
             * 우선은 상태 전이가 이루이지지 않는 상황을 피하기 위해, threshold를 좀 높여봅니다.
             */

            // 떨어지는지 확인
            if (!isFalling && player.rb.velocity.y < 0.01f)
            {
                isFalling = true;
            }

            return this;        
        }

        private void DoubleJump()
        {
            Debug.Log("double jump");

            doubleJump = true;

            // double jump 애니메이션 트리거 삽입 위치 (몸 동그랗게 말기 등등)

            player.se.PlaySE(player.se.SE_JUMP);

            player.rb.velocity = Vector2.zero;
            player.rb.AddForce(Vector2.up * player.jumpEff, ForceMode2D.Impulse);
        }
    }
}