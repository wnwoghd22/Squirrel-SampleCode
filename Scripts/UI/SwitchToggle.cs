using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public class SwitchToggle : MonoBehaviour, IToggle
{
    [SerializeField] private RectTransform handleRect;
    [SerializeField] private Image background;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color defaultColor;

    [SerializeField] private Toggle toggle;

    Vector2 handlePosition;

    public bool Value => toggle.isOn;

    public void SetAction(UnityAction<bool> action)
    {
        toggle.onValueChanged.AddListener(action);
    }

    // Start is called before the first frame update
    void Start()
    {
        // toggle = GetComponent<Toggle>();

        handlePosition = handleRect.anchoredPosition;

        toggle.onValueChanged.AddListener(OnSwitch);

        background.color = defaultColor;

        if (toggle.isOn)
            OnSwitch(true);
    }

    void OnSwitch(bool on)
    {
        if (on)
            UniTask.Create(() => SetTrueAsync());
        else 
            UniTask.Create(() => SetFalseAsync()); 
        
    }
    async UniTask SetTrueAsync()
    {
        float alpha = 1f;

        while (alpha > 0f)
        {
            handleRect.anchoredPosition = (handlePosition * -1) * (1f - alpha) + handlePosition * alpha;
            background.color = activeColor * (1f - alpha) + defaultColor * alpha;

            alpha -= 0.05f;

            await UniTask.Yield();
        }
    }
    async UniTask SetFalseAsync()
    {
        float alpha = 0f;

        while (alpha < 1f)
        {
            handleRect.anchoredPosition = (handlePosition * -1) * (1f - alpha) + handlePosition * alpha;
            background.color = activeColor * (1f - alpha) + defaultColor * alpha;

            alpha += 0.05f;

            await UniTask.Yield();
        }
    }
}
