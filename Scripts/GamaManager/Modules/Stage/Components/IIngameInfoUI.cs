using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIngameInfoUI
{
    void HandleStageText(int chapter, int stage);    // 스테이지 이름
    void HandleHealthUI(int health);   // 생명력
    void HandleAcornUI(int acorn);     // 스테이지에서 먹은 도토리 개수
    void HandleEndingItemUI(int endingItem);  // 스테이지에서 먹은 엔딩아이템 개수
}