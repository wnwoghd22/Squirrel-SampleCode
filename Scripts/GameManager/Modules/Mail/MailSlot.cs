using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailSlot : MonoBehaviour
{
    [SerializeField] Text sentence;
    [SerializeField] Image reward;
    private int index;
    public int Index => index;
    private Mail mail;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(IRewardHandler rewardHandler, int index, Mail m, Sprite sprite)
    {
        this.index = index;
        mail = m;
        sentence.text = m.sentence;
        reward.sprite = sprite;

        GetComponent<Button>().onClick.AddListener(call: () => rewardHandler.GetReward(index, this));

        if (!m.available)
            SetUnavailable();
    }

    public void SetUnavailable()
    {
        // 보상을 받았다면 글씨를 흐리게
        Color textColor = sentence.color;
        textColor.a = 0.5f;
        sentence.color = textColor;

        // 이미지도 흐리게?
    }
}
