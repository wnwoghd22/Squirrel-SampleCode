using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class PlayerController
{
    class StateClimb : IState
    {
        PlayerController player;

        SceneObj.Tree tree;

        /// <summary>
        /// 플레이어가 나무를 타고 오를 때.
        /// </summary>
        /// <param name="player"></param>
        public StateClimb(PlayerController player)
        {
            this.player = player;

            player.rb.velocity = Vector2.zero;
            player.rb.gravityScale = 0f;
            player.animator.ResetTrigger("run");
            player.animator.ResetTrigger("climb");
            player.animator.ResetTrigger("jump");
            player.animator.ResetTrigger("stop");

            player.animator.SetTrigger("climb");

            tree = player.AttachedTree.transform.GetComponent<SceneObj.Tree>();

            // 낙하물 투하
            tree.DropObstacle();

            // 나뭇잎 생성 시작
            tree.PlayParticle();
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
                player.AttachedTree.collider.enabled = false;

                // 나뭇잎 생성 종료
                this.tree.StopParticle();

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
                        // 나무 충돌판정 해제
                        player.AttachedTree.collider.enabled = false;

                        // 나뭇잎 생성 종료
                        tree.StopParticle();

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
        /// 점프 처리 메소드
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
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
            //꼭데기에 이르면
            if (!player.AttachedTree)
            {
                // 나뭇잎 생성 종료
                this.tree.StopParticle();

                return new StateClimbTop(player); // 일시적으로 벽 꼭대기에 이르렀음을 나타내는 상태
            }

            // 계속 벽에 붙어있다면 위로 타고 오르기
            Vector3 pos = player.transform.position;
            pos.y += player.climbSpeed * Time.deltaTime;
            player.transform.position = pos;

            return this;
        }
    }
}