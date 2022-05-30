using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class PlayerController
{
    class StateFlying : IState
    {
        private PlayerController player;

        public StateFlying(PlayerController player)
        {
            this.player = player;

            // 풍선 사용 처리
            player.HasBalloon = false;

            // 풍선 활성화
            player.balloon.SetActive(true);
            player.rb.gravityScale = 0f;

            player.animator.ResetTrigger("run");
            player.animator.ResetTrigger("climb");
            player.animator.ResetTrigger("jump");
            player.animator.ResetTrigger("stop");

            // player.animator.SetTrigger("run");

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
            {
                DisableBalloon(player);
                return Jump(player);
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
                        DisableBalloon(player);
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

        /// <summary>
        /// 풍선 비활성화 및 사용 처리
        /// </summary>
        /// <param name="player"></param>
        private void DisableBalloon(PlayerController player) => player.balloon.SetActive(false);

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
            Vector3 pos = player.transform.position;
            pos.y += player.flyingEff * Time.deltaTime;
            player.transform.position = pos;
            if (pos.y > 3f)
            {
                player.balloon.SetActive(false);
                player.rb.gravityScale = 1f;

                return new StateJump(player);
            }

            return this;
        }
    }
}
