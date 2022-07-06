using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBox : MonoBehaviour
{
    [SerializeField] private MailManager mailManager;

    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform scrollViewContent; // slot을 child로 추가해야 하는 transform
    [SerializeField] private Sprite[] rewardSprites;

    private List<MailSlot> mailSlots = new List<MailSlot>();
    public List<MailSlot> MailSlots => mailSlots;

    [SerializeField] private GameObject mailSlotPrefab;
    private float slotHeight;

    [SerializeField] private Text mailCount;

    [SerializeField] private GameObject mailScroll;
    [SerializeField] private GameObject emptyPrompt;

    // Start is called before the first frame update
    void Start()
    {
        // mailSlots = new List<MailSlot>();
        // slotHeight = mailSlotPrefab.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 메일함을 열 때 호출하는 함수.
    /// 메일함을 열 때마다 모든 슬롯을 파괴한 후 다시 만들어냅니다.
    /// </summary>
    /// <param name="mails"></param>
    public void SetMails(List<Mail> mails)
    {
        slotHeight = mailSlotPrefab.GetComponent<RectTransform>().rect.height;

        // 슬롯 파괴
        foreach (MailSlot slot in mailSlots)
            Destroy(slot.gameObject);
        mailSlots = new List<MailSlot>();
        emptyPrompt.SetActive(false);
        mailScroll.SetActive(false);

        mailCount.text = mailManager.CurrentMailCount + " / " + MailManager.MAX_MAIL_COUNT;

        // 메일이 없다면 메일 없음을 표시하고 바로 메소드 종료
        if (mailManager.CurrentMailCount == 0)
        {
            emptyPrompt.SetActive(true);
            return;
        }

        mailScroll.SetActive(true);

        int index = 0;
        foreach (Mail mail in mails)
        {
            GameObject slot = Instantiate(mailSlotPrefab);
            RectTransform slotRect = slot.GetComponent<RectTransform>();
            slotRect.sizeDelta = new Vector2(viewport.rect.width, slotRect.rect.height);
            slot.transform.SetParent(scrollViewContent);

            MailSlot mailSlot = slot.GetComponent<MailSlot>();
            mailSlot.SetValue(mailManager, index++, mail, rewardSprites[(int)mail.reward]);

            mailSlots.Add(mailSlot);
        }
    }
}
