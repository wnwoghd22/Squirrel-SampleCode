using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class PlayerController
{
    /** State Machine
     * 달리는 상태에서의 업데이트와 조작을 따로 정의한 상태 클래스
     * 굳이 번거롭게 내부 클래스로 정의하는 이유는,
     * PlayerController의 private field에 쉽게 접근하기 위함과,
     * 다른 클래스에서의 접근을 불가능하게 하여 더 명확한 프로그래밍이 가능하게 하기 위함입니다.
     */
    class StateRun : IState
    {
        private PlayerController player;

        public StateRun(PlayerController player)
        {
            this.player = player;

            player.rb.gravityScale = 1f;

            player.animator.ResetTrigger("run");
            player.animator.ResetTrigger("climb");
            player.animator.ResetTrigger("jump");
            player.animator.ResetTrigger("stop");

            player.animator.SetTrigger("run");

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
                return Jump(player); // 점프

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
                        // 점프
                        return Jump(player);
                    case TouchPhase.Moved:
                        break;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Ended:
                        break;
                    case TouchPhase.Canceled:
                        break;
                }
            }

            return this;
        }

        /// <summary>
        /// 점프 처리 메소드
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private IState Jump(PlayerController player)
        {
            Debug.Log("jump");

            // 위로 점프
            player.rb.AddForce(Vector2.up * player.jumpEff, ForceMode2D.Impulse);

            player.se.PlaySE(player.se.SE_JUMP);

            // 점프했다면, StateJump를 반환하여 플레이어가 점프 상태가 될 수 있도록 함
            return new StateJump(player);
        }

        public IState HandleUpdate()
        {
            /**
             * 맵을 뒤로 가게 할 건지 플레이어를 앞으로 가게 할 건지 결정할 필요 있음.
             * 무한모드라면 obstacle manager를 재활용하고,
             * 레벨디자인을 고려하여 만들 예정인 스토리모드에서도 gm이 맵을 뒤로 움직이게 하는게
             * 현재로써는 가장 저렴한 구현방식일듯 합니다.
             */

            if (!player.isGrounded())
                return new StateFall(player);

            if (player.AttachedTree) // bool 속성까지 확인할 것 없이 눈 앞에 Tree가 있는지만 검사함
                return new StateClimb(player);

            return this;
        }
    }
}