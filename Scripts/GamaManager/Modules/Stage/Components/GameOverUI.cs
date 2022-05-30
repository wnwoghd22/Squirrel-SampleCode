using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Text acornCount;
    [SerializeField] Text endingItemCount;

    [SerializeField] Sprite[] itemSprites;
    [SerializeField] Image endingItemImage;

    [SerializeField] GameObject seasonUnlockTab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues(int acorns, int chapter, int endingItems, bool seasonUnLock = false)
    {
        acornCount.text = "x" + acorns;

        endingItemImage.sprite = itemSprites[chapter - 1];
        endingItemCount.text = "x" + endingItems;

        if (seasonUnLock)
            seasonUnlockTab?.SetActive(seasonUnLock); // null reference 체크가 문제군요...
    }
}
