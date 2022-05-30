using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 요소를 정사각형으로 만들어줍니다.
/// 요구 조건: 앵커의 가로 길이는 화면 비율에 맞게 박혀있어야 합니다.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class RectNormalizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(1f, rect.rect.width);
    }
}
