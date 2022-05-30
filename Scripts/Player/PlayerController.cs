using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public partial class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    public bool TouchingTree => rb.IsTouchingLayers(LayerMask.GetMask("Tree"));
    private bool OnTree => TouchingTree && Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Tree"));
    private bool OnBranch => rb.IsTouchingLayers(LayerMask.GetMask("Branch"));

    [SerializeField] private float jumpEff = 5f;
    [SerializeField] private float climbSpeed = 2f;
    [SerializeField] private float trampolineEff = 10f;

    private IDataHandler data;
    private IGameFlowHandler flow;
    private ISEHandler se;
    private IVibrationHandler vib;
    private IAchievementListener achievementManager;

    private Animator animator;

    private IState state;

    public int maxHealth = 3;
    private int health;
    public int Health { 
        get => health;
        set 
        {
            // 체력이 감소했지만 비눗방울이 활성화된 상태라면
            if (bubbleShield.activeSelf && value < health)
            {
                bubbleShield.SetActive(false);
                return;
            }

            health =  value <= maxHealth ? value : maxHealth;
            ingameInfoUI.HandleHealthUI(value);
        }
    }
    private int acorns = 0;
    public  int Acorns {
        get => acorns;
        set
        {
            acorns = value;
            ingameInfoUI.HandleAcornUI(value);
        } 
    }

    //private IHealthUI healthUI;
    private IIngameInfoUI ingameInfoUI;

    private bool damaged;
    [SerializeField] float damageDelay;
    [SerializeField] int flashingFrame = 10;

    [SerializeField] GameObject bubbleShield;
    public GameObject BubbleShield => bubbleShield;

    [SerializeField] GameObject balloon;
    public bool HasBalloon { get; private set; } = false;
    public void UseBalloonItem() => HasBalloon = true;

    [SerializeField] private float flyingEff = 3f;

    public bool IsFlying => state.GetType() == typeof(StateFlying);
    public bool IsTop => state.GetType() == typeof(StateClimbTop);

    [SerializeField] GameObject magnetItem;
    public void UseMagetItem() => magnetItem.SetActive(true);

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        achievementManager = FindObjectOfType<AchievementManager>();

        GameManager gm = FindObjectOfType<GameManager>();
        data = gm;
        flow = gm;
        se = gm;
        vib = gm;

        animator = GetComponent<Animator>();

        ingameInfoUI = FindObjectOfType<InGameInfoUI>();
        ingameInfoUI.HandleStageText(gm.Data.LastChapterNum, gm.Data.LastStageNum);
        ingameInfoUI.HandleHealthUI(health);
        ingameInfoUI.HandleAcornUI(0);
        ingameInfoUI.HandleEndingItemUI(0);
        Health = maxHealth;
        Acorns = 0;
        damaged = false;

        state = new StateRun(this);
    }

    void Update()
    {
        state = state.HandleInput();
        state = state.HandleUpdate();

        // Debug.Log(IsTop);
        Debug.Log(state + ", isGrounded: " + isGrounded() + ", isAttachWall: " + (AttachedTree.collider != null) + ", isTop: " + IsTop);
    }
    private bool isGrounded() => OnTree || OnBranch; // 지형을 밟고 있으면 true;

    public RaycastHit2D AttachedTree => Physics2D.BoxCast(coll.bounds.center + new Vector3(coll.bounds.size.x / 2, 0, 0), new Vector2(0.01f, coll.bounds.size.y), 0f, Vector2.right, 0.01f, LayerMask.GetMask("Tree"));

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 6: // Tree
                if (IsFlying)
                    collision.collider.isTrigger = true;
                break;
            case 9: // 트렘폴린
                se.PlaySE(se.SE_TREMPOLIN);

                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * trampolineEff, ForceMode2D.Impulse);

                if (balloon.activeSelf)
                    balloon.SetActive(false);

                state = new StateJump(this);

                /// 트램펄린 업적 확인
                ++data.Data.HitLotus;
                achievementManager.CheckAchievement(collision.gameObject.tag, data.Data.HitLotus);

                break;
            case 13: // Death: 땅에 추락

                /// 추락 업적 확인
                ++data.Data.FallCount;
                achievementManager.CheckAchievement("Fall", data.Data.FallCount);

                if (HasBalloon) // 풍선을 갖고 있다면
                {
                    state = new StateFlying(this);
                    break;
                }

                flow.GameOver(acorns);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 7:
            case 20: // 회복 아이템
                HandleItem(collision);
                break;
            case 12:
                if (!damaged)
                    HandleObstacles(collision);
                break;
            case 14:
                flow.ClearStage(acorns);
                break;
            case 16:
                collision.transform.GetChild(1).gameObject.SetActive(true);  // 경고창 표시
                collision.transform.GetChild(0).GetComponent<Rigidbody2D>().gravityScale = 1;
                StartCoroutine(warningDisabled(collision.transform.GetChild(1).gameObject));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 아이템 충돌 처리 메소드
    /// </summary>
    /// <param name="collision"></param>
    private void HandleItem(Collider2D collision)
    {
        se.PlaySE(se.SE_GETITEM);

        switch (collision.gameObject.tag)
        {
            case "Acorn":
                SceneObj.Item item = collision.gameObject.GetComponent<SceneObj.Item>();

                Acorns += item.Score;

                // 겨울이면 눈덩이 눈덩이 업적 관련 코드
                if(flow.ChapterNum == 4)
                {
                    int num = ++data.Data.HitWinterAcorn;
                    achievementManager.CheckAchievement("WinterAcorn", num);
                }

                break;
            case "EndingItem":
                int index = (data.Data.LastChapterNum - 1) * 5 + data.Data.LastStageNum;
                Debug.Log("현재 인덱스 : " + index);
                data.Data.ItemPerStage[index]++;

                ingameInfoUI.HandleEndingItemUI(data.Data.ItemPerStage[index]);
                break;
            case "HealingItem":
                Health++;
                break;
        }


        collision.gameObject.SetActive(false);
    }

    /// <summary>
    /// 장애물 충돌 처리 메소드
    /// </summary>
    /// <param name="collision"></param>
    private void HandleObstacles(Collider2D collision)
    {
        Health--;

        int num = 0;
        switch (collision.gameObject.tag)
        {
            case "Snake":
                num = ++data.Data.HitSnake;
                break;
            case "Vine":
                num = ++data.Data.HitRose;
                break;
            case "Lotus":
                num = ++data.Data.HitLotus;
                break;
            case "Chestnut":
                num = ++data.Data.HitChestnut;
                break;
            case "Mushroom":
                num = ++data.Data.HitMushroom;
                break;
            case "Icicle":
                num = ++data.Data.HitIcicle;
                break;
            default:
                break;
        }
        // achievementManager.CheckAchievement(num, collision.gameObject.tag);

        achievementManager.CheckAchievement(collision.gameObject.tag, num);

        collision.enabled = false;

        if (collision.gameObject.tag == "Mushroom" || collision.gameObject.tag == "Icicle")
        {
            collision.gameObject.SetActive(false); // 버섯은 화면에서 사라지도록
        }

        if (Health <= 0) flow.GameOver(acorns);

        UniTask.Create(async () => await GetDamaged());
    }

    IEnumerator warningDisabled(GameObject warningSign)
    {
        yield return new WaitForSeconds(0.5f);
        warningSign.SetActive(false);
    }
    private async UniTask GetDamaged()
    {
        vib.Vibrate();

        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        float delay = damageDelay;
        int flip = 0;
        damaged = true;

        while (delay > 0)
        {
            delay -= Time.deltaTime;
            ++flip;
            if (flip == flashingFrame)
            {
                flip = 0;
                renderer.enabled = !renderer.enabled;
            }
            await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        renderer.enabled = true;
        damaged = false;
    }
}
