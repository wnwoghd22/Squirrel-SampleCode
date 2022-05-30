using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookSlot : MonoBehaviour
{
    [SerializeField] Sprite originSprite;

    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private GameObject lockObject;

    [SerializeField] private GameObject innerBoxInfo;
    [SerializeField] private GameObject effect;  // ÈÄ±¤ ÀÌÆåÆ®

    private Achievement achievement;
    private Archive archive;

    // Start is called before the first frame update
    void Start()
    {
        archive = FindObjectOfType<Archive>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearSlot()
    {

    }

    public void SetSlot(int page, Achievement achievement, Sprite sprite)
    {
        this.achievement = achievement;
        effect.SetActive(false);

        if (achievement.IsLocked)
        {
            LockSlot();
            return;
        }

        innerBoxInfo.SetActive(true);
        lockObject.SetActive(false);  // Àá±ÝÇ¥½Ã ¾ø¾Ú
        image.sprite = sprite;  // ±×¸² ³Ö±â
        text.text = achievement.name;

        if (page > 1) effect.SetActive(achievement.state == Achievement.NEW); // ¾÷Àû¸¸ ÈÄ±¤
    }

    public void SetEmptySlot()
    {
        achievement = null;
        innerBoxInfo.SetActive(false);
    }

    private void LockSlot()
    {
        innerBoxInfo.SetActive(true);
        lockObject.SetActive(true);
        image.sprite = originSprite;
        text.text = "???";
    }

    private void UnlitEffect()
    {
        // ÄÑÁ®ÀÖ´Ù¸é ²¨ÁÜ
        if (effect.activeSelf)
            effect.SetActive(false);
    }

    public void HandleClick()
    {
        if (achievement == null) return;

        UnlitEffect();
        archive.HandleClick(achievement);
    }
}
