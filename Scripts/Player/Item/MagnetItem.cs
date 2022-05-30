using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    private ISEHandler se;
    [SerializeField] Transform player;
    [SerializeField] PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        se = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) // item
        {

            switch (collision.gameObject.tag)
            {
                case "Acorn":
                    se.PlaySE(se.SE_GETITEM);
                    SceneObj.Item item = collision.gameObject.GetComponent<SceneObj.Item>();
                    playerController.Acorns += item.Score;
                    collision.gameObject.SetActive(false);
                    break;
                case "EndingItem": // 아이템으로는 도토리만 먹을 수 있게 설정.
                default:
                    return;
            }
        }
    }
}
