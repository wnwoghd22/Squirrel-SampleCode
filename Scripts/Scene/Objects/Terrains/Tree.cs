using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene에서 실제로 동작하는 Obj 클래스 계열
/// </summary>
namespace SceneObj
{
    public class Tree : MonoBehaviour
    {
        [SerializeField] private ParticleSystem leavesParticle;
        public ParticleSystem LeavesParticle => leavesParticle;

        [SerializeField] private GameObject leftBranch;
        public GameObject LeftBranch => leftBranch;
        [SerializeField] private GameObject rightBranch;
        public GameObject RightBranch => rightBranch;
        [SerializeField] private GameObject leaves;
        public GameObject Leaves => leaves;
        [SerializeField] private GameObject leavesPos;
        [SerializeField] private GameObject pillar;
        public GameObject Pillar => pillar;
        [SerializeField] private GameObject lBranchPos;
        [SerializeField] private GameObject rBranchPos;

        [Header("낙하물")]
        [SerializeField] private GameObject rDrop;
        [SerializeField] private GameObject lDrop;
        public GameObject LDrop => lDrop;
        public GameObject RDrop => rDrop;

        public void SetHeight(float height)
        {
            pillar.transform.localScale = new Vector3(1, height, 1);
            leaves.transform.position = leavesPos.transform.position;

        }
        public void SetLeftBranch(float posY, float length)
        {
            lBranchPos.transform.localPosition = new Vector3(0f, posY, 0f);

            Vector3 pos = lBranchPos.transform.position;

            // pos.y -= 0.4f * (posY / pillar.transform.localScale.y);

            pos.y -= 0.25f;

            leftBranch.transform.position = pos;

            // leftBranch.transform.localScale = new Vector3(length, 1f, 1);

            leftBranch.transform.localScale = new Vector3(length, 0.5f, 1);

            var effector = leftBranch.transform.GetChild(0).GetComponent<PlatformEffector2D>();
            if (length < 0) length *= -1;
            /// 한 방향으로 나뭇가지를 2개로 할려면 길이을 음수로 하면 되는데 surfaceArc를 구할때 문제가 생기네요..
            /// 길이가 음수면 surfaceArc 값이 180이 넘어버려요(근데 씬뷰에서 확인하면 값이 0이 되버림...) 
            /// 따라서 surfaceArc 계산하기 전에만 음수를 다시 양수로 바꿔줘야 해요 
            effector.surfaceArc = 180 - 2 * Mathf.Atan2(0.25f, length / 2) * Mathf.Rad2Deg;
        }
        public void SetRightBranch(float posY, float length)
        {
            rBranchPos.transform.localPosition = new Vector3(0f, posY, 0f);

            Vector3 pos = rBranchPos.transform.position;
            // pos.y -= 0.4f * (posY / pillar.transform.localScale.y);

            pos.y -= 0.25f;

            rightBranch.transform.position = pos;

            // rightBranch.transform.localScale = new Vector3(length, 1f, 1);

            rightBranch.transform.localScale = new Vector3(length, 0.5f, 1);

            var effector = rightBranch.transform.GetChild(0).GetComponent<PlatformEffector2D>();
            if (length < 0) length *= -1;
            effector.surfaceArc = 180 - 2 * Mathf.Atan2(0.25f, length / 2) * Mathf.Rad2Deg;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void PlayParticle() => leavesParticle.Play();
        public void StopParticle() => leavesParticle.Stop();

        public void DropObstacle()
        {
            if (rDrop.activeSelf)
            {
                rDrop.GetComponent<Collider2D>().enabled = true;
                rDrop.GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
            if (lDrop.activeSelf)
            {
                lDrop.GetComponent<Collider2D>().enabled = true;
                lDrop.GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
        }
    }
}
