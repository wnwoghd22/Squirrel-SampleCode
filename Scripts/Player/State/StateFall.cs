using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class PlayerController
{
    /// <summary>
    /// 플레이어가 땅에서 막 떨어지기 시작하는 시점에 점프 판정을 주기 위한 일시적인 상태
    /// </summary>
    public class StateFall : IState
    {
        private PlayerController player;
        private float remain = 0.3f;

        public StateFall(PlayerController player)
        {
            this.player = player;

            player.rb.gravityScale = 1f;

            player.animator.ResetTrigger("run");
            player.animator.ResetTrigger("climb");
            player.animator.ResetTrigger("jump");
            player.animator.ResetTrigger("stop");

            // player.animator.SetTrigger("jump");

            // 상태가 변경되는 시점에 애니메이터 트리거, 사운드 재생 등의 이벤트를 넣을 수 있음
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
            if (player.isGrounded())
            {
                //착지 사운드 및 애니메이션 삽입 위치

                return new StateRun(player);
            }

            if (player.AttachedTree)
                return new StateClimb(player);

            remain -= Time.deltaTime;

            if (remain < 0)
                return new StateJump(player);

            return this;
        }
    }
}
