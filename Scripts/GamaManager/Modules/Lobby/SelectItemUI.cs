using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemUI : MonoBehaviour
{
    [SerializeField] GameObject selectItemTab;

    private IDataHandler dataHandler;
    private IGameFlowHandler flowHandler;
    private IFileHandler fileHandler;
    private Inventory inventory;

    [SerializeField] GameObject[] effectUI;
    [SerializeField] Text[] countText;

    [SerializeField] Image endingItemImage;
    [SerializeField] Sprite[] endingItemSprites;

    [SerializeField] Text myItemCount;

    [SerializeField] Text stageText;

    [SerializeField] WarningPopUp warningPopUp;

    [SerializeField] Text ItemResetPopUpText;

    [SerializeField] GameObject Stage0;
    [SerializeField] Text Stage0Text;

    private void Awake()
    { 
        GameManager gm = FindObjectOfType<GameManager>();
        dataHandler = gm;
        flowHandler = gm;
        fileHandler = gm;
        inventory = gm.Inventory;
    }

    public void ShowSelectItemTab()
    {
        SetUIValues();
        InitItems();
        foreach (GameObject o in effectUI) o.SetActive(false);

        selectItemTab.SetActive(true);
    }
    public void CloseSelectItemTab() => selectItemTab.SetActive(false);

    /// <summary>
    /// 창이 떴을 때 초기화 해주는 부분
    /// 아이템을 선택했다가 뒤로가기 버튼 누른뒤 다시 들어가면 아이템을 골랐던 걸 초기화 해줌
    /// </summary>
    private void InitItems()
    {
        for(int i = 0; i < 4; i++)
        {
            if(flowHandler.ShopItemContainer[i] == true)  // 아이템이 선택되어 있으면
            {
                inventory.UnuseItem(i);
                //fileHandler.Save();
                if(i != 3)  // 초기화권이 아니면 빨간 불도 끄고 개수도 원래대로
                {
                    effectUI[i].SetActive(false);
                    countText[i].text = fileHandler.Data.MyItemCount[i].ToString();
                }
            }

            // 원래 개수가 0개인데 선택해서 불이 들어온 경우 나갔다 들어오면 불만 꺼줌
            if(i != 3 && fileHandler.Data.MyItemCount[i] == 0 && effectUI[i].activeSelf == true)
            {
                effectUI[i].SetActive(false);
            }
        }
    }

    private void SetUIValues()
    {
        for (int i = 0; i < countText.Length; ++i)
        {
            Debug.Log(i + " " + dataHandler);
            countText[i].text = dataHandler.Data.MyItemCount[i].ToString();
        }

        endingItemImage.sprite = endingItemSprites[flowHandler.ChapterNum - 1];

        int currentStage = (flowHandler.ChapterNum - 1) * 5 + flowHandler.StageNum;
        myItemCount.text = dataHandler.Data.MyItemPerStage[currentStage].ToString();

        stageText.text = "STAGE " + flowHandler.ChapterNum + "-" + flowHandler.StageNum;

        string itemName = flowHandler.ChapterNum == 1 ? "새싹" : flowHandler.ChapterNum == 2 ? "물방울" : flowHandler.ChapterNum == 3 ? "단풍" : "눈꽃";
        ItemResetPopUpText.text = flowHandler.ChapterNum + "-" + flowHandler.StageNum + "스테이지의 " + itemName + "을\n다시 모으시겠습니까?";

        /// 0스테이지일 경우 엔딩아이템이 없다는 표시를 준다.
        Stage0.SetActive(flowHandler.StageNum == 0);
        Stage0Text.text = "여기는 " + itemName + "이 없다람!";

    }

    public void SelectItem(int index)
    {
        // inventory.SelectItem(index);
        // effectUI[index].SetActive(!effectUI[index].activeSelf);

        effectUI[index].SetActive(inventory.SelectItem(index));

        countText[index].text = fileHandler.Data.MyItemCount[index].ToString();
    }

    /// <summary>
    /// 초기화 아이템은 다른 아이템이랑 조금 다르게 작동해서 함수 하나 그냥 만들었습니다...
    /// </summary>
    /// <param name="check">초기화 아이템 선택시 나오는 창에서 예 누르면 check 1, 아니오 누르면 check 0</param>
    public void SelectInitStageItem(bool check)
    {
        // 예 누르면
        if(check)
        {
            /// 스테이지 초기화 아이템을 사용했는데 해당 스테이지가 클리어 되지 않았다면 사용 불가
            if (dataHandler.Data.ClearStatus[flowHandler.ChapterNum - 1].intArr[flowHandler.StageNum] == 1)
            {
                warningPopUp.gameObject.SetActive(true);
                warningPopUp.setText("해당 스테이지를 클리어해야\n사용가능 합니다.");
                return;
            }
            else
            {
                // 예를 눌렀는데 이미 선택되었다면 이미 선택됐다고 알려주기
                if(flowHandler.ShopItemContainer[3])
                {
                    warningPopUp.gameObject.SetActive(true);
                    warningPopUp.setText("스테이지 초기화권을 이미\n사용하고 있습니다.");
                    return;
                }
                else
                {
                    if (dataHandler.Data.MyItemCount[3] == 0)
                    {
                        warningPopUp.gameObject.SetActive(true);
                        warningPopUp.setText("스테이지 초기화권이 없습니다.");
                        return;
                    }
                    inventory.UseItem(3);
                    //fileHandler.Save();
                }
            }
        }
        // 아니오 누르면
        else
        {
            // 선택되었을 경우 취소하기
            if (flowHandler.ShopItemContainer[3])
            {
                inventory.UnuseItem(3);
                //fileHandler.Save();
            }
        }
    }
}
