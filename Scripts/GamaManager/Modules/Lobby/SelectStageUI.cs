using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageUI : MonoBehaviour
{
    [SerializeField] GameObject selectStageTab;

    [SerializeField] Image[] stageImages;
    [SerializeField] Sprite[] sprites;

    [SerializeField] Text[] stageTexts;

    [SerializeField] Text[] itemCountTexts;

    private IDataHandler dataHandler;
    private IGameFlowHandler flowHandler;

    private void Awake()
    {
        dataHandler = FindObjectOfType<GameManager>();
        flowHandler = FindObjectOfType<GameManager>();
    }

    public void ShowSelectStageTab()
    {
        selectStageTab.SetActive(true);

        SetUIValues();
    }
    public void CloseStageTab() => selectStageTab.SetActive(false);

    private void SetUIValues()
    {
        for(int i = 0; i < stageImages.Length; i++)
        {
            int index = (flowHandler.ChapterNum - 1) * 5 + i;

            stageImages[i].sprite = sprites[flowHandler.ChapterNum - 1];

            if (i != 0 && dataHandler.Data.ClearStatus[flowHandler.ChapterNum - 1].intArr[i] == 0)
                stageImages[i].color = new Color(0.5f, 0.5f, 0.5f);
            else
                stageImages[i].color = new Color(1f, 1f, 1f);

            switch(flowHandler.ChapterNum)
            {
                case 1:
                    stageTexts[i].text = "봄 - " + i;
                    break;
                case 2:
                    stageTexts[i].text = "여름 - " + i;
                    break;
                case 3:
                    stageTexts[i].text = "가을 - " + i;
                    break;
                case 4:
                    stageTexts[i].text = "겨울 - " + i;
                    break;
            }

            itemCountTexts[i].text = dataHandler.Data.MyItemPerStage[index] + " / 3";
            //itemCountTexts[i].gameObject.SetActive(dataHandler.Data.MyItemPerStage[index] > 0);
            /// 0스테이지 빼고 클리어 시에만 파밍개수 보여주기
            itemCountTexts[i].gameObject.SetActive(dataHandler.Data.ClearStatus[flowHandler.ChapterNum - 1].intArr[i] == 2 && i != 0);
        }

    }
}
