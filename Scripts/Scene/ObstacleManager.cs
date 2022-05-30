using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Header("오브젝트")]
    [SerializeField] private GameObject obstaclePrefab;   // 장애물 프리팹

    [Header("장애물 관련")]
    [SerializeField] private int obstacleCount;  // 시작 장애물 개수
    [SerializeField] private float xPosSpaceing; // 장애물끼리의 x간격
    [SerializeField] private float yPosRange;    // 장애물의 y위치 범위
    [SerializeField] private float obstacleScrollSpeed; // 장애물 이동 속도
    [SerializeField] private float xPosEnd;      // x좌표 끝

    private float scrollSpeed;
    private float obstacleOffsetYPos;
    private List<GameObject> obstacles;  // 장애물(나무) 오브젝트 풀링

    void Start()
    {

    }

    public void Generate()
    {
        obstacleOffsetYPos = (obstaclePrefab.transform.localScale.y) / 2;

        obstacles = new List<GameObject>();

        for (int i = 0; i < obstacleCount; i++)
        {
            // 처음 장애물 위치 계산 
            //float yRandPos = -obstacleOffsetYPos + Random.Range(0, yPosRange);
            var pos = new Vector3(xPosEnd + xPosSpaceing * i, -obstacleOffsetYPos, 0);
            // 장애물 생성
            var obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);

            for (int j = 0; j < 2; j++)
            {
                float leafPosX = obstacle.transform.GetChild(j).localScale.x / 2;
                float localPosX = (j % 2 == 0) ? leafPosX : -leafPosX;
                obstacle.transform.GetChild(j).localPosition = new Vector2(localPosX, Random.Range(0.1f, 0.4f));
            }

            // 오브젝트 풀에 추가
            obstacles.Add(obstacle);
        }
    }

    void Update()
    {
        // Scroll();
    }

    // Update 내부 코드를 함수로 추출, GameManager에서 호출하도록 변경
    public void Scroll()
    {
        var speed = Vector3.left * (scrollSpeed * Time.deltaTime);

        foreach (var obstacle in obstacles)
        {
            // 장애물 이동
            obstacle.transform.Translate(speed);

            // 장애물이 끝 지점에 도달하면 장애물 위치 새로 계산해서 이동
            if (obstacle.transform.position.x <= xPosEnd)
            {
                //float yRandPos = -obstacleOffsetYPos + Random.Range(0, yPosRange);
                var pos = new Vector3(xPosEnd + xPosSpaceing * (obstacleCount), -obstacleOffsetYPos, 0);
                obstacle.transform.position = pos;
                obstacle.GetComponent<Collider2D>().enabled = true;

                for (int j = 0; j < 2; j++)
                {
                    float leafPosX = obstacle.transform.GetChild(j).localScale.x / 2;
                    float localPosX = (j % 2 == 0) ? leafPosX : -leafPosX;
                    obstacle.transform.GetChild(j).localPosition = new Vector2(localPosX, Random.Range(0.1f, 0.4f));
                }
            }
        }
    }

    public void scrollStart(bool activate)
    {
        scrollSpeed = activate ? obstacleScrollSpeed : 0f;
    }
}
