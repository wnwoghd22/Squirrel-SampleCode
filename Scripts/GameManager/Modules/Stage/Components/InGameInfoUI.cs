using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInfoUI : MonoBehaviour, IIngameInfoUI
{
    [SerializeField] private Text stageText;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private Text acronText;
    [SerializeField] private Image endingItemImage;
    [SerializeField] private Text endingItemText;
    [SerializeField] private Sprite[] sprites;

    public void HandleHealthUI(int health)
    {
        for (int i = 0; i < hearts.Length; ++i)
        {
            hearts[i].SetActive(i < health ? true : false);
        }
    }

    public void HandleStageText(int chapter, int stage)
    {
        switch (chapter)
        {
            case 1:
                stageText.text = "봄 - " + stage;
                endingItemImage.sprite = sprites[chapter - 1];
                break;
            case 2:
                stageText.text = "여름 - " + stage;
                endingItemImage.sprite = sprites[chapter - 1];
                break;
            case 3:
                stageText.text = "가을 - " + stage;
                endingItemImage.sprite = sprites[chapter - 1];
                break;
            case 4:
                stageText.text = "겨울 - " + stage;
                endingItemImage.sprite = sprites[chapter - 1];
                break;
        }
    }

    public void HandleAcornUI(int acorn)
    {
        acronText.text = "X " + acorn;
    }

    public void HandleEndingItemUI(int endingItem)
    {
        endingItemText.text = "X " + endingItem;
    }
}
