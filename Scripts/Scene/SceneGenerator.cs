using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneGenerator : MonoBehaviour
{
    [SerializeField] private GameObject startPlatform;  // 게임 시작 시 플랫폼
    [SerializeField] private GameObject endPlatform;    // 게임 클리어 시 플랫폼

    [SerializeField] private PrefabList[] prefabList;   // 프리팹 리스트

    [SerializeField] private Sprite[] startingPoints;   // 스타팅포인트 그림  봄, 여름, 가을, 겨울
    [SerializeField] private Sprite[] endingPoints;     // 엔딩포인트 그림

    [Header("장애물 관련")]
    [SerializeField] private float obstacleScrollSpeed; // 장애물 이동 속도
    [SerializeField] private float xPosEnd;      // x좌표 끝

    private float scrollSpeed;
    private float obstacleOffsetYPos;
    private List<GameObject> obstacles;  // 장애물(나무) 오브젝트 풀링

    private Dictionary<int, Type> dict;

    public float MapLength { get; private set; }

    private void Awake()
    {
        // 새로운 오브젝트가 생길때마다 여기서 dict 추가해주면 됨!!  좋습니닷
        dict = new Dictionary<int, Type>();
        dict.Add(0, typeof(TreeObj));
        dict.Add(1, typeof(AcornObj));
        dict.Add(2, typeof(FarmingItemObj));
        dict.Add(3, typeof(DropObsObj));  // 낙하장애물
        dict.Add(4, typeof(TempObs)); // 고정 장애물
        dict.Add(5, typeof(TrampolineObj)); // 트램펄린
        dict.Add(6, typeof(XAxisMovingObstacle));  // x축으로 이동하는 장애물
        dict.Add(7, typeof(CandyObj));  // 회복 아이템
        dict.Add(8, typeof(VineObj));  // 덩쿨
        dict.Add(9, typeof(MushroomObj)); // 독버섯
    }


    public void GenerateScene(int chapter, ObjData[] objs)
    {
        obstacles = new List<GameObject>();

        // 장애물 생성
        var startPlatform = Instantiate(this.startPlatform, new Vector3(0, 0, 0), Quaternion.identity);
        startPlatform.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = startingPoints[chapter - 1];

        obstacles.Add(startPlatform);

        for (int i = 0; i < objs.Length; ++i)
        {
            GameObject @object = prefabList[chapter - 1].list[objs[i].Type];

            try
            {
                Obj obj = (Obj)Activator.CreateInstance(dict[objs[i].Type]);
                Debug.Log(obj);
                //obj.SetFields(objs[i]);

                // 개선된 구조?
                obj.AssignValuesAutomatically(objs[i]);
                Debug.Log("assign : " + obj);

                var obstacle = obj.InstantiatePrefab(@object);
                Debug.Log("instantiate : " + obj);

                // 오브젝트 풀에 추가
                obstacles.Add(obstacle);
            }
            catch
            {
                Debug.Log("잘못된 Type Number입니다.");
            }
        }

        MapLength = TerrainObj.PrePosX;

        // 장애물 생성
        var endPlatform = Instantiate(this.endPlatform, new Vector3(TerrainObj.PrePosX + 15f, 0, 0f), Quaternion.identity);
        endPlatform.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = endingPoints[chapter - 1];

        TerrainObj.Initialize();
        
        obstacles.Add(endPlatform);
    }

    // Update 내부 코드를 함수로 추출, GameManager에서 호출하도록 변경
    public void Scroll()
    {
        var speed = Vector3.left * (scrollSpeed * Time.deltaTime);

        foreach (var obstacle in obstacles)
        {
            // 장애물 이동
            obstacle.transform.Translate(speed);
        }
    }

    // Update 내부 코드를 함수로 추출, GameManager에서 호출하도록 변경
    public void Scroll(float speed)
    {
        var spd = Vector3.left * (speed * Time.deltaTime);

        foreach (var obstacle in obstacles)
        {
            // 장애물 이동
            obstacle.transform.Translate(spd);
        }
    }

    public void scrollStart(bool activate)
    {
        scrollSpeed = activate ? obstacleScrollSpeed : 0f;
    }
}
